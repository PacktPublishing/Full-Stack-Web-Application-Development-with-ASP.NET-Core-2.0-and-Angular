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

import { Subject } from "rxjs/Subject";

import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";

import { constants } from "../shared/constants";
import { AuthService } from "./auth.service";
import { LoginRedirectService } from "./redirect.service";

@Component({
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"],
    selector: "app-login"
})
export class LoginComponent { 
  constructor(
    public _authService: AuthService,
    public _elementRef: ElementRef,
    public _loginRedirectService: LoginRedirectService,
    public _renderer: Renderer
  ) { }

  public onDestroy: Subject<void> = new Subject<void>();

  ngAfterContentInit() {

    this._renderer.invokeElementMethod(this.usernameNativeElement, 'focus', []);

    this.form.patchValue({
      username: this.username,
      password: this.password,
      rememberMe: this.rememberMe
    });
  }
  @Input()
  public username: string;

  @Input()
  public password: string;

  @Input()
  public rememberMe: boolean;

  @Input()
  public errorMessage: string = "";

  @Input()
  public customerKey: string = "";

  @Input()
  public debugMode: boolean = false;

  public form = new FormGroup({
    username: new FormControl(this.username, [Validators.required]),
    password: new FormControl(this.password, [Validators.required]),
    rememberMe: new FormControl(this.rememberMe, [])
  });

  public get usernameNativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector("#username");
  }

  @Output()
  public tryToLogin($event) {
    
    this._authService.tryToLogin({
      username: $event.value.username,
      password: $event.value.password
    }).subscribe(() => this._loginRedirectService.redirectPreLogin());
  }
  
  @HostListener('window:keyup', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.keyCode == constants.ENTER_KEY_CODE)
      this.tryToLogin({ value: this.form.value });
  }
  
  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
