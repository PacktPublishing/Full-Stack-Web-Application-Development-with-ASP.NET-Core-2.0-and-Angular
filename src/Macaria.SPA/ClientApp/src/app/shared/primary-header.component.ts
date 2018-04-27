import { Component } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  templateUrl: './primary-header.component.html',
  styleUrls: ['./primary-header.component.css'],
  selector: 'app-primary-header'
})
export class PrimaryHeaderComponent {
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
