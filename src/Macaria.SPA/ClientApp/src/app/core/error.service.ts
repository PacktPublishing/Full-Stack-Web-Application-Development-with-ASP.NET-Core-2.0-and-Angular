import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

@Injectable()
export class ErrorService {
  constructor(private _snackBar: MatSnackBar, private _translateService: TranslateService) {}

  public translations$(values: Array<string>): Observable<any> {
    return this._translateService.get(values);
  }

  public handle$(
    httpErrorResponse: HttpErrorResponse,
    message: string = 'Error',
    action: string = 'An error ocurr.Try it again.'
  ) {
    return this.translations$([message, action]).pipe(
      map(translations =>
        this._snackBar.open(translations[message], translations[action], {
          duration: 0
        })
      )
    );
  }
}
