import { Injectable } from '@angular/core';
import { LaunchSettings } from './launch-settings';
import { ILogger, LogLevel } from '@aspnet/signalr';

@Injectable()
export class Logger implements ILogger {
  log(logLevel: LogLevel, message: string): void {
    if (logLevel >= this.logLevel) console.log(`${LogLevel[logLevel]}: ${message}`);
  }
  constructor(launchSettings: LaunchSettings) {
    launchSettings.logLevel$
      .subscribe(x => this.logLevel = x);
  }

  public logLevel: LogLevel = 0;

  public trace(title:string, message: string) {
    this.log(LogLevel.Trace, `(${title}) ${message}`);
  }

  public error(title:string, message: string) {
    this.log(LogLevel.Error, `(${title}) ${message}`);
  }
}
