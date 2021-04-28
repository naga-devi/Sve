import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpEvent, HttpResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { JX_MODULE_CONFIG } from '../../_config/jx-module.config.token';
import { JxModuleConfig } from '../../_config/jx-module.config';

@Injectable()
export class UploadDownloadService {
    constructor(public httpClient: HttpClient, @Inject(JX_MODULE_CONFIG) private jxConfig: JxModuleConfig,) { }

    public downloadFileFromApi(url: string, filename: string = '') {

        return this.httpClient.get(
            `${url}`,
            {
                reportProgress: true,
                responseType: 'blob',// as 'json',
                observe: 'response',
                //withCredentials:true
            })
            .pipe(
                catchError(err => {
                    console.log('Handling error locally and rethrowing it...', err);
                    return throwError(err);
                })
            )
            .subscribe(
                (data) => {

                    this.downloadFileFromBlob(data, filename);
                },
                (error) => {
                    console.log(error);
                }
            );
    }   

    public downloadFileFromPath(
		downloadFiles: any
	): Observable<HttpEvent<Blob>> {
		return this.httpClient.request(
			new HttpRequest(
				"POST",
				`${this.jxConfig.baseApiUrl}/downloader/files`,
				downloadFiles,
				{
					reportProgress: true,
					responseType: "blob",
				}
			)
		);
	}
    

    //download file from blob
    public downloadFileFromBlob(data: HttpResponse<Blob>, filename: string = '') {
        const contentDisposition = data.headers && data.headers.get('content-disposition');
        if (contentDisposition) {
            filename = this.getFilenameFromContentDisposition(contentDisposition);
        }

        const blob = new Blob([data.body], { type: data.body.type });
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            // IE11 and Edge
            window.navigator.msSaveOrOpenBlob(blob, filename);
        } else {
            // Chrome, Safari, Firefox, Opera
            let url = URL.createObjectURL(blob);
            this.openLink(url, filename);
            // Remove the link when done
            setTimeout(function () {
                window.URL.revokeObjectURL(url);
            }, 5000);
        }

        //const blob = new Blob([data.body], { type: data.body.type });
        //const url = window.URL.createObjectURL(blob);
        //const anchor = document.createElement("a");
        //anchor.setAttribute('style', 'display:none;');
        //anchor.download = filename;
        //anchor.href = url;
        //anchor.target = '_blank';
        //anchor.click();
        //document.body.removeChild(anchor);
    }

    public printFileFromApi(url: string, filename: string = '') {

        return this.httpClient.get(
            `${url}`,
            {
                reportProgress: true,
                responseType: 'blob',// as 'json',
                observe: 'response',
                //withCredentials:true
            })
            .pipe(
                catchError(err => {
                    console.log('Handling error locally and rethrowing it...', err);
                    return throwError(err);
                })
            )
            .subscribe(
                (response: HttpResponse<Blob>) => {
                    //this.printFileFromBlob(data, filename);
                    //var blob = new Blob([response.blob()], {type: 'application/pdf'});
                    const blob = new Blob([response.body], { type: response.body.type });
                    const blobUrl = URL.createObjectURL(blob);
                    const iframe = document.createElement('iframe');
                    iframe.style.display = 'none';
                    iframe.src = blobUrl;
                    document.body.appendChild(iframe);
                    iframe.contentWindow.print();
                },
                (error) => {
                    console.log(error);
                }
            );
    }

    public printFileFromBlob(data: HttpResponse<Blob>, filename: string = '') {
        const contentDisposition = data.headers && data.headers.get('content-disposition');
        if (contentDisposition) {
            filename = this.getFilenameFromContentDisposition(contentDisposition);
        }

        const blob = new Blob([data.body], { type: data.body.type });
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            // IE11 and Edge
            window.navigator.msSaveOrOpenBlob(blob, filename);
        } else {
            // Chrome, Safari, Firefox, Opera
            let url = URL.createObjectURL(blob);
            this.openLink(url, filename);
            // Remove the link when done
            setTimeout(function () {
                window.URL.revokeObjectURL(url);
            }, 5000);
        }

        //const blob = new Blob([data.body], { type: data.body.type });
        //const url = window.URL.createObjectURL(blob);
        //const anchor = document.createElement("a");
        //anchor.setAttribute('style', 'display:none;');
        //anchor.download = filename;
        //anchor.href = url;
        //anchor.target = '_blank';
        //anchor.click();
        //document.body.removeChild(anchor);
    }

    private openLink(url: string, filname: string) {
        let a = document.createElement('a');
        // Firefox requires the link to be in the body
        document.body.appendChild(a);
        a.style.display = 'none';
        a.href = url;
        a.download = filname;
        a.click();
        // Remove the link when done
        document.body.removeChild(a);
    }

    private getFilenameFromContentDisposition(contentDisposition: string): string {
        const regex = /filename=(?<filename>[^,;]+);/g;
        const match = regex.exec(contentDisposition);
        const filename = match.groups.filename;
        return filename;
    }
}
