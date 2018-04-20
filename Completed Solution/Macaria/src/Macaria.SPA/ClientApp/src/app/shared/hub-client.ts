import { Injectable, NgZone } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { HubConnection } from "@aspnet/signalr";
import { LocalStorageService } from "./local-storage.service";


@Injectable()
export class HubClient {
  private _licenseHubProxy: any;
  private _connection: HubConnection;
  public events: Subject<any> = new Subject();
  public static instance;

  constructor(
    private _storage: LocalStorageService,
    private _ngZone: NgZone) {
  }

  private _connect: Promise<any>;

  public connect(): Promise<any> {

    if (this._connect)
      return this._connect;

    this._connect = new Promise((resolve) => {      
      this._connection = this._connection || new HubConnection(`/hub`);

      this._connection.on("message", (value) => {        
        this._ngZone.run(() => this.events.next(value));
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
