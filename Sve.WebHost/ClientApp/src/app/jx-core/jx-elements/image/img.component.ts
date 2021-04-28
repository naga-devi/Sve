import { Component, Input, OnChanges } from '@angular/core';

/**
 * Usage: <jx-img [src]="src" [alt]="alt" [default]="DEFAULT_IMAGE"></jx-img>
 */
@Component({
    selector: 'jx-img',
    templateUrl: 'img.component.html',
})
export class JxImgComponent implements OnChanges {
    @Input() 
    public src: string;
    @Input() 
    public default: string;
    @Input() 
    public alt: string = '';
    public cached = false;
    public loaded = false;
    public error = false;

    private lastSrc: string;

    constructor() { }

    public ngOnChanges() {
        if (this.src !== this.lastSrc) {
            this.lastSrc = this.src;
            this.loaded = false;
            this.error = false;
            this.cached = this.isCached(this.src);
        }

        if (!this.src) {
            this.error = true;
        }
    }

    public onLoad() {
        this.loaded = true;
    }

    public onError() {
        this.error = true;
    }

    private isCached(url: string): boolean {
        if (!url) {
            return false;
        }

        let image = new Image();
        image.src = url;
        let complete = image.complete;

        // console.log('isCached', complete, url);

        return complete;
    }
}
