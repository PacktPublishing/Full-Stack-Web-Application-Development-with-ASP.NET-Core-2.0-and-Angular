import { Overlay, OverlayRef } from "@angular/cdk/overlay";
import { Injectable } from "@angular/core";

@Injectable()
export class OverlayRefProvider {
  constructor(private _overlay: Overlay) { }

  public create(): OverlayRef {
    const positionStrategy = this._overlay.position()
      .global()
      .centerHorizontally()
      .centerVertically();

    return this._overlay.create({
      hasBackdrop: true,
      positionStrategy
    });
  }
}
