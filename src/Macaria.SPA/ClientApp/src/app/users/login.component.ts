import {
  Component,
  Input,
  Renderer,
  ElementRef,
  HostListener
} from "@angular/core";

import { Subject } from 'rxjs';

import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";

import { takeUntil, tap, map } from "rxjs/operators";
import { MatSnackBar } from "@angular/material";
import { TranslateService } from "@ngx-translate/core";
import { ENTER } from "@angular/cdk/keycodes";
import { AuthService } from "../core/auth.service";
import { LoginRedirectService } from "../core/redirect.service";
import { Output } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"],
    selector: "app-login"
})
export class LoginComponent { 
  constructor(
    private _authService: AuthService,
    public _elementRef: ElementRef,
    public _loginRedirectService: LoginRedirectService,
    public _matSnackBar: MatSnackBar,
    public _renderer: Renderer,
    public _translateService: TranslateService
  ) { }

  ngOnInit() {
    this._authService.logout();
    this._translateService.get(["Login Failed", "An error ocurr.Try it again."])
      .pipe(
        takeUntil(this.onDestroy),
        map((translations) => this.translations = translations)
      )
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public translations;

  ngAfterContentInit() {
    this._renderer.invokeElementMethod(this.usernameNativeElement, 'focus', []);
  }

  @Input()
  public username: string;

  @Input()
  public password: string;
  
  public form = new FormGroup({
    username: new FormControl(this.username, [Validators.required]),
    password: new FormControl(this.password, [Validators.required])
  });

  public get usernameNativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector("#username");
  }

  @HostListener("window:click")
  public dismissSnackBar() {
    this._matSnackBar.dismiss();
  }

  @Output()
  public tryToLogin($event) { 
    this._authService.tryToLogin({
      username: $event.value.username,
      password: $event.value.password
    })
      .subscribe(() =>
        this._loginRedirectService.redirectPreLogin(),
      errorResponse => {
        return this.handleError(errorResponse);
      });
  }

  public handleError(errorResponse) {
    if (!errorResponse.error && !errorResponse.error.messages) {
      this._matSnackBar.open(this.translations['Login Failed'], errorResponse.statusText, {
        duration: 0
      });
      return;
    }
    this._matSnackBar.open(this.translations['Login Failed'], this.translations[errorResponse.error.messages[0]], {
      duration: 0
    });
  }

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
