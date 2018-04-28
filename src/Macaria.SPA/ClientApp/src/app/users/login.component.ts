import { Component, Input, Renderer, ElementRef, HostListener } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { takeUntil, tap, map } from 'rxjs/operators';
import { MatSnackBarRef, SimpleSnackBar } from '@angular/material';
import { ENTER } from '@angular/cdk/keycodes';
import { AuthService } from '../core/auth.service';
import { LoginRedirectService } from '../core/redirect.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../core/error.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  selector: 'app-login'
})
export class LoginComponent {
  constructor(
    private _authService: AuthService,
    private _elementRef: ElementRef,
    private _errorService: ErrorService,
    private _loginRedirectService: LoginRedirectService,
    private _renderer: Renderer
  ) {}

  ngOnInit() {
    this._authService.logout();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngAfterContentInit() {
    this._renderer.invokeElementMethod(this.usernameNativeElement, 'focus', []);
  }

  public username: string;

  public password: string;

  private _snackBarRef: MatSnackBarRef<SimpleSnackBar>;

  public form = new FormGroup({
    username: new FormControl(this.username, [Validators.required]),
    password: new FormControl(this.password, [Validators.required])
  });

  public get usernameNativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector('#username');
  }

  @HostListener('window:click')
  public dismissSnackBar() {
    if (this._snackBarRef) this._snackBarRef.dismiss();
  }

  public tryToLogin($event) {
    this._authService
      .tryToLogin({
        username: $event.value.username,
        password: $event.value.password
      })
      .pipe(takeUntil(this.onDestroy))
      .subscribe(
        () => this._loginRedirectService.redirectPreLogin(),
        errorResponse => this.handleErrorResponse(errorResponse)
      );
  }

  public handleErrorResponse(errorResponse) {
    this._errorService
      .handle$(errorResponse, 'Login Failed')
      .pipe(takeUntil(this.onDestroy), map(snackBarRef => (this._snackBarRef = snackBarRef)))
      .subscribe();
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
