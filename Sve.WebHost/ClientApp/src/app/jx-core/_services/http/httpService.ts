import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject, throwError } from 'rxjs';
import { JxModuleConfig, JX_MODULE_CONFIG } from '../../_config';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { KtDialogService } from '../kt-dialog/kt-dialog.service';

@Injectable({
	providedIn: 'root'
})
export class HttpService {

	private taskPauser: Subject<any>;

	constructor(
		@Inject(JX_MODULE_CONFIG) private jxConfig: JxModuleConfig,
		private http: HttpClient,
		public ktdialog: KtDialogService,
		private router: Router) { }

	get(url: string, loading: boolean = true): Observable<any> {
		if(loading){
			this.ktdialog.show('Loading...');
		}
		return this.http.get(this.createCompleteRoute(url), this.getRequestHeaders()).pipe(finalize(() => {this.ktdialog.hide();
		}));
	}

	getBlob(url: any) :Observable<any>{
		return this.http.get(this.createCompleteRoute(url), this.getDownloadRequestHeaders());
	}

	post(url: string, body: any, isPut: boolean = false, loading = true): Observable<any> {
		if(loading){
			this.ktdialog.show('Loading...');
		}
		if (isPut)
			return this.http.put(this.createCompleteRoute(url), body, this.getRequestHeaders()).pipe(finalize(() => {this.ktdialog.hide();
			}));
        else
			return this.http.post(this.createCompleteRoute(url), body, this.getRequestHeaders()).pipe(finalize(() => {this.ktdialog.hide();
			}));
	}

	postForm(url: any, body: any, isPut: boolean = false): Observable<any> {
		if (isPut)
			return this.http.put(this.createCompleteRoute(url), body, this.getRequestFormUploadHeaders());
		else
			return this.http.post(this.createCompleteRoute(url), body, this.getRequestFormUploadHeaders());
	}

	put(url: string, body: any): Observable<any> {
		return this.http.put(this.createCompleteRoute(url), body, this.getRequestHeaders());
	}

	putForm(url: any, body: any): Observable<any> {
		return this.http.put(this.createCompleteRoute(url), body, this.getRequestFormUploadHeaders());
	}

	delete(url: any): Observable<any> {
		return this.http.delete(this.createCompleteRoute(url), this.getRequestHeaders());
	}

	private createCompleteRoute = (route: string) => {
		return `${this.jxConfig.baseApiUrl}${route}`;
		//return `${this.API_BASEROUTE}/${route}`;
	}

	getRequestHeaders(): { headers: HttpHeaders | { [header: string]: string | string[]; } } {
		const headers = new HttpHeaders({
			Authorization: 'Bearer ' + localStorage.getItem(this.jxConfig.authTokenKey),
			'Content-Type': 'application/json',
			Accept: `application/vnd.iman.v${this.jxConfig.appVersion}+json, application/json, text/plain, */*`,
			'App-Version': this.jxConfig.appVersion
		});

		return { headers };
	}

	getRequestFormUploadHeaders(): { headers: HttpHeaders | { [header: string]: string | string[]; } } {
		const headers = new HttpHeaders({
			Authorization: 'Bearer ' + localStorage.getItem(this.jxConfig.authTokenKey),
			// 'Content-Type': 'multipart/form-data;',
			// 'Accept': `application/vnd.iman.v${EndpointFactory.apiVersion}+json, application/json, text/plain, */*`,
			'App-Version': this.jxConfig.appVersion
		});

		return { headers };
	}

	getDownloadRequestHeaders(): any {
		const headers = new HttpHeaders({
			Authorization: 'Bearer ' + localStorage.getItem(this.jxConfig.authTokenKey),

			// 'Content-Type': 'multipart/form-data;',
			// 'Accept': `application/vnd.iman.v${EndpointFactory.apiVersion}+json, application/json, text/plain, */*`,
			'App-Version': this.jxConfig.appVersion
		});

		return {
			headers,
			reportProgress: true,
			responseType: 'blob',// as 'json',
			observe: 'response',
		};
	}

	handleError(error) {
		if (error.status === 401 || error.url && error.url.toLowerCase().includes(this.jxConfig.loginUrl.toLowerCase())) {
			localStorage.removeItem(this.jxConfig.authTokenKey);
			const redirect = this.jxConfig.loginUrl;
			this.router.navigate([redirect]);
			this.resumeTasks(false);
			return throwError((error.error && error.error.error_description) ? `session expired (${error.error.error_description})` : 'session expired');
		} else {
			return throwError(error);
		}
	}


	private resumeTasks(continueOp: boolean) {
		setTimeout(() => {
			if (this.taskPauser) {
				this.taskPauser.next(continueOp);
				this.taskPauser.complete();
				this.taskPauser = null;
			}
		});
	}
	 
	// private generateHeaders = () => {
	// 	return {
	// 		headers: new HttpHeaders({'Content-Type': 'application/json'})
	// 	}
	// }

	// private request<T>(method: string, url: string, options?: any): Observable<T> {
	// 	return Observable.create((observer: any) => {
	// 	  this.http.request<T>(new HttpRequest(method, this.createCompleteRoute(url), options))
	// 	  .subscribe(
	// 		(response: any) => {
	// 		  const responsTye = response as HttpEvent<any>
	// 		  switch (responsTye.type) {
	// 			case HttpEventType.Sent:
	// 			  console.log('Request sent!');
	// 			  break;
	// 			case HttpEventType.ResponseHeader:
	// 			  console.log('Response header received!');
	// 			  break;
	// 			case HttpEventType.DownloadProgress:
	// 			  const kbLoaded = Math.round(responsTye.loaded / 1024);
	// 			  console.log(`Download in progress! ${kbLoaded}Kb loaded`);
	// 			  break;
	// 			case HttpEventType.Response:
	// 			  observer.next(response.body);
	// 			  console.log('😺 Done!', responsTye.body);
	// 		  }
	// 		},
	// 		(error) => {
	// 		  switch (error.status) {
	// 			case 403:
	// 			  observer.complete();
	// 			  break;
	// 			default:
	// 			  observer.error(error);
	// 			  break;
	// 		  }
	// 		}
	// 	  );
	// 	});
	// }
}
