import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { LaunchSettings } from './launch-settings';

export interface ILogger {
  log(logLevel: LogLevel, title:string, message: string): void;
  error(tite: string, message: string): void;
  trace(title:string, message: string): void;
}

export enum LogLevel {
  Trace = 0,
  Information,
  Warning,
  Error,
  None
}

@Injectable()
export class LoggerService implements ILogger {
  constructor(launchSettings: LaunchSettings) {
    launchSettings.logLevel$
      .pipe(map(x => this._minimumLogLevel = x))
      .subscribe();
  }

  private _minimumLogLevel: LogLevel;

  public log(logLevel: LogLevel, title:string, message: string) {
    if (logLevel >= this._minimumLogLevel) console.log(`${LogLevel[logLevel]}: (${title}) ${message}`);
  }

  public trace(title:string, message: string) {
    this.log(LogLevel.Trace, title, message);
  }

  public error(title:string, message: string) {
    this.log(LogLevel.Error,title, message);
  }
}
