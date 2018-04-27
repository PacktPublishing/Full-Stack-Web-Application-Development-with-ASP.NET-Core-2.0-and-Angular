import { Component, ElementRef, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

declare var Quill: any;

@Component({
  templateUrl: './quill-text-editor.component.html',
  styleUrls: ['./quill-text-editor.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => QuillTextEditorComponent),
      multi: true
    }
  ],
  selector: 'app-quill-text-editor'
})
export class QuillTextEditorComponent implements ControlValueAccessor {
  constructor(private _elementRef: ElementRef) {
    this.onTextChanged = this.onTextChanged.bind(this);
  }

  public writeValue(value: any): void {
    if (this.qlEditorNativeElement && value) this.qlEditorNativeElement.innerHTML = value;
  }

  public registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  public setDisabledState?(isDisabled: boolean): void {}

  public onTouchedCallback: () => void = () => {};

  public onChangeCallback: (_: any) => void = () => {};

  public ngAfterViewInit() {
    this._quill = new Quill(this.nativeElement, {
      theme: 'bubble',
      placeholder: this.editorPlaceholder
    });

    this._quill.on('text-change', this.onTextChanged);
  }

  public onTextChanged(delta, oldDelta, source) {
    this.onChangeCallback(this.qlEditorNativeElement.innerHTML);
  }

  @Input() public editorPlaceholder: string;

  public get nativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector('.editor') as HTMLElement;
  }

  public get qlEditorNativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector('.ql-editor') as HTMLElement;
  }

  private _quill: any;
}
