import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

@Injectable()
export class ErrorService {
  constructor(private _snackBar: MatSnackBar, private _translateService: TranslateService) {}

  public handle(
    httpErrorResponse: HttpErrorResponse,
    message: string = 'Error',
    action: string = 'An error ocurr.Try it again.'
  ): MatSnackBarRef<SimpleSnackBar> {
    return this._snackBar.open(this._translateService.instant(message), this._translateService.instant(action), {
      duration: 0
    });
  }
}
