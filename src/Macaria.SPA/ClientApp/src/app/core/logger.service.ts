import { Injectable, Inject } from '@angular/core';
import { minimumLogLevel } from './constants';

export interface ILogger {
  log(logLevel: LogLevel, message: string): void;
  error(message: string): void;
  trace(message: string): void;
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
  constructor(@Inject(minimumLogLevel) private _minimumLogLevel: LogLevel) {}

  public log(logLevel: LogLevel, message: string) {
    if (logLevel >= this._minimumLogLevel) console.log(`${LogLevel[logLevel]}: ${message}`);
  }

  public trace(message: string) {
    this.log(LogLevel.Trace, message);
  }

  public error(message: string) {
    this.log(LogLevel.Error, message);
  }
}
