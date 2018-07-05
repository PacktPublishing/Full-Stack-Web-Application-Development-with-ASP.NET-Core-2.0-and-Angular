import { Inject, Injectable, NgZone } from '@angular/core';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { accessTokenKey, baseUrl } from './constants';
import { Logger } from './logger.service';
import { LocalStorageService } from './local-storage.service';

@Injectable()
export class HubClient {
  private _connection: HubConnection;
  private _connect: Promise<any>;

  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _logger: Logger,
    private _storage: LocalStorageService,
    private _ngZone: NgZone
  ) {}
  
  public events$: Subject<any> = new Subject();
  
  public connect(): Promise<any> {    
    if (this._connect) return this._connect;

    var options: IHttpConnectionOptions = {
      logger: this._logger,
      accessTokenFactory: () => this._storage.get({
        name: accessTokenKey
      }),
      logMessageContent: true
    };

    this._connect = new Promise((resolve,reject) => {
      this._connection =
        this._connection || new HubConnectionBuilder()          
          .withUrl(`${this._baseUrl}hub`, options)
          .build();

      this._connection.on('events', value => {        
        this._ngZone.run(() => this.events$.next(value));
      });
      
      this._connection.start()
        .then(() => resolve())
        .catch(() => reject());
    });

    return this._connect;
  }

  public disconnect() {
    if (this._connection) {
      this._connection.stop();
      this._connect = null;
      this._connection = null;
    }
  }
}
