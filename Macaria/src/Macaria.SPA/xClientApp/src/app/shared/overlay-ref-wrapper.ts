import { OverlayRef } from '@angular/cdk/overlay';

export class OverlayRefWrapper {
  constructor(private overlayRef: OverlayRef) { }

  close(): void {
    this.overlayRef.dispose();
  }
}
