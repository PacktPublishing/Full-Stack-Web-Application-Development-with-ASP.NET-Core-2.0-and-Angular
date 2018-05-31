import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, EventEmitter, Input, Output, ViewChild, forwardRef } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatInput } from '@angular/material';
import { Observable, Subject } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './auto-complete-chip-list.component.html',
  styleUrls: ['./auto-complete-chip-list.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AutoCompleteChipListComponent),
      multi: true
    }
  ],
  selector: 'app-auto-complete-chip-list'
})
export class AutoCompleteChipListComponent implements ControlValueAccessor {
  constructor() {
    this.onChipClick = new EventEmitter();
  }

  writeValue(obj: any): void {
    obj = this.addItems.value;
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  public onTouchedCallback: () => void = () => {};

  public onChangeCallback: (_: any) => void = () => {};

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  @Output() public onChipClick: EventEmitter<any>;

  @ViewChild('chipInput') chipInput: MatInput;
  @Input() selectedItems: string[] = [];
  filteredItems: Observable<any[]>;
  addItems: FormControl;

  separatorKeysCodes = [ENTER, COMMA];

  get itemsData(): string[] {
    return this.selectedItems;
  }

  set itemsData(v: string[]) {
    this.selectedItems = v;
  }

  @Input()
  items$: Subject<Item[]>;

  items: Item[] = [];

  ngOnInit() {
    this.addItems = new FormControl();

    this.items$.pipe(map(x => {
      this.items = x;
      this.filteredItems = this.addItems.valueChanges.pipe(
        startWith(''),
        map(item => (item ? this.filterItems(item.toString()) : this.items.slice()))
      );
      }),
      takeUntil(this.onDestroy)
    )
      .subscribe();

    
  }

  filterItems(itemName: string) {
    return this.items.filter(item => item.name.toLowerCase().indexOf(itemName.toLowerCase()) === 0);
  }

  onRemoveItems(itemName: string): void {
    this.selectedItems = this.selectedItems.filter((name: string) => name !== itemName);
    this.itemsData = this.selectedItems;
    this.chipInput['nativeElement'].blur();
    this.onChangeCallback(this.selectedItems);
  }

  onAddItems(event: MatAutocompleteSelectedEvent) {
    const t: Item = event.option.value;

    if (this.selectedItems.length === 0) {
      this.selectedItems.push(t.name);
    } else {
      const selectLanguageStr = JSON.stringify(this.selectedItems);
      if (selectLanguageStr.indexOf(t.name) === -1) {
        this.selectedItems.push(t.name);
      }
    }

    this.itemsData = this.selectedItems;
    this.chipInput['nativeElement'].blur();
    this.chipInput['nativeElement'].value = '';
    this.onChangeCallback(this.selectedItems);
  }
}

export class Item {
  itemId: number;
  name: string;
  constructor(itemId: number, name: string) {}
}
