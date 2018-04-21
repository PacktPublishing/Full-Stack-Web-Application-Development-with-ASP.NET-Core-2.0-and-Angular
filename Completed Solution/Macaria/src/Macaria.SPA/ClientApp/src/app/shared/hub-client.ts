import { Injectable, NgZone, Inject } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { HubConnection } from "@aspnet/signalr";
import { LocalStorageService } from "./local-storage.service";
import { constants } from "./constants";

@Injectable()
export class HubClient {
  private _connection: HubConnection;
  public messages$: Subject<any> = new Subject();
  
  constructor(
    @Inject(constants.BASE_URL)private _baseUrl:string,
    private _storage: LocalStorageService,
    private _ngZone: NgZone) {
  }

  private _connect: Promise<any>;

  public connect(): Promise<any> {

    if (this._connect)
      return this._connect;

    this._connect = new Promise((resolve) => {      
      this._connection = this._connection || new HubConnection(`${this._baseUrl}hub?token=${this._storage.get({ name: constants.ACCESS_TOKEN_KEY })}`);
      
      this._connection.on("message", (value) => {
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
