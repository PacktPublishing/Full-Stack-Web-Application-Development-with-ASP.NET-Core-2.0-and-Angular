import { Injectable, NgZone, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey, baseUrl } from './constants';
import { filter } from 'rxjs/operators';
import { Logger } from './logger.service';

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
  
  public messages$: Subject<any> = new Subject();

  public connect(): Promise<any> {    
    if (this._connect) return this._connect;

    this._connect = new Promise(resolve => {
      this._connection =
        this._connection || new HubConnectionBuilder()
          .configureLogging(this._logger)
          .withUrl(`${this._baseUrl}hub?token=${this._storage.get({ name: accessTokenKey })}`)
          .build();

      this._connection.on('message', value => {
        this._logger.trace(`HubClient`, JSON.stringify(value));

        this._ngZone.run(() => this.messages$.next(value));
      });

      this._connection.start().then(() => resolve());
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
