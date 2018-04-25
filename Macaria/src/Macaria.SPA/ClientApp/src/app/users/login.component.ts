import {
  Component,
  ChangeDetectionStrategy,
  Input,
  OnInit,
  EventEmitter,
  Output,
  AfterViewInit,
  AfterContentInit,
  Renderer,
  ElementRef,
  ViewEncapsulation,
  HostListener
} from "@angular/core";

import { Subject } from 'rxjs';

import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";

import { AuthService } from "./auth.service";
import { LoginRedirectService } from "./redirect.service";
import { HubClient } from "../core/hub-client";
import { takeUntil, tap, map } from "rxjs/operators";
import { MatSnackBar } from "@angular/material";
import { TranslateService } from "@ngx-translate/core";
import { AddTagOverlayComponent } from "../tags/add-tag-overlay.component";
import { ENTER } from "@angular/cdk/keycodes";

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

    this.form.patchValue({
      username: this.username,
      password: this.password
    });
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
        errorResponse => this.handleError(errorResponse));
  }

  public handleError(errorResponse) {
    this._matSnackBar.open(this.translations['Login Failed'], this.translations[errorResponse.error.messages[0]], {
      duration: 0
    });
  }

  @HostListener('window:keyup', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.keyCode == ENTER)
      this.tryToLogin({ value: this.form.value });
  }
  
  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
