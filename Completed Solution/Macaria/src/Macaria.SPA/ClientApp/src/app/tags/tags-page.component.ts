import { Component, Injector } from "@angular/core";
import { Subject, pipe } from "rxjs";
import { TagsService } from "./tags.service";
import { Tag } from "./tag.model";
import { Observable } from "rxjs/Observable";
import { map, tap } from "rxjs/operators";
import { ColDef } from "ag-grid";
import { Overlay } from "@angular/cdk/overlay";
import { OverlayRefWrapper } from "../shared/overlay-ref-wrapper";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { AddTagOverlayComponent } from "./add-tag-overlay.component";
import { TagStore } from "./tag-store";
import { takeUntil } from "rxjs/operators";
import { MatSnackBar } from "@angular/material";
import { Notifications } from "../shared/notifications";
import { DeleteCellComponent } from "../ag-grid-components/delete-cell.component";

@Component({
  templateUrl: "./tags-page.component.html",
  styleUrls: ["./tags-page.component.css"],
  selector: "app-tags-page"
})
export class TagsPageComponent { 
  constructor(
    private _injector: Injector,
    private _overlay: Overlay,
    private _tagsService: TagsService,
    public _tagStore: TagStore,
    private _snackBar: MatSnackBar,
    private _notifications: Notifications
  ) {

  }
  
  ngOnInit() {
    this._tagsService.get()
      .pipe(
        takeUntil(this.onDestroy),
        map(x => this._tagStore.tags$.next(x.tags))
      )
      .subscribe();
  }

  public handleChange($event) {
    this._tagsService.save({ tag: $event.data })
      .pipe(
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Name", field: "name", onCellValueChanged: ($event) => this.handleChange($event), editable:true },
    { cellRenderer: "deleteRenderer", onCellClicked: ($event) => this.handleDelete($event), width:20 }
  ];

  public frameworkComponents = {
    deleteRenderer: DeleteCellComponent
  };

  public onGridReady(params) { params.api.sizeColumnsToFit(); }

  public get tags$(): Observable<Array<Tag>> {
    return this._tagStore.tags$;
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public handleDelete($event) {
    const tags = this._tagStore.tags$.value;
    const deletedTagIndex = tags.findIndex(x => x.tagId == $event.data.tagId);

    tags.splice(deletedTagIndex, 1);
    
    this._tagsService.remove({ tagId: $event.data.tagId })
      .pipe(
        takeUntil(this.onDestroy),
        tap(x => {
          this._tagStore.tags$.next([...tags]);
        })
      )
      .subscribe();
  }

  public handleCreateClick() {
    const positionStrategy = this._overlay.position()
      .global()
      .centerHorizontally()
      .centerVertically();

    const overlayRef = this._overlay.create({
      hasBackdrop: true,
      positionStrategy
    });

    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);

    const injectionTokens = new WeakMap();
    injectionTokens.set(OverlayRefWrapper, overlayRefWrapper);
    const injector = new PortalInjector(this._injector, injectionTokens);
    const overlayPortal = new ComponentPortal(AddTagOverlayComponent, null, injector);
    overlayRef.attach(overlayPortal);

    overlayRef.backdropClick()
      .subscribe(x => overlayRef.dispose());
  }
  
}