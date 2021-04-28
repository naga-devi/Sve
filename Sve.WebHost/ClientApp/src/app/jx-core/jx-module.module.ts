import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA, ModuleWithProviders } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { JxMatDateAdapter, JXMAT_DATE_FORMATS } from './_adapter/format-datepicker';
import { JxModuleConfig, JX_MODULE_CONFIG } from './_config';
// custom jx elements
import { JxFileComponent } from './jx-elements/file/file.component';
import { JxDownloadComponent } from './jx-elements/download/download.component';
// pipes
import {
	EntityStatusPipe,
	AvatarLetterPipe,
	EntityApprovePipe,
	FilterPipe,
	BooleanTextPipe,
	TruncatePipe,
	TooltipTextPipe,
	NullOrEmptyPipe,
	SafeHtmlPipe
} from './_pipes';


// directives
import { JxDownloaderDirective } from './_directives/downloader.directive';

// custom module


//import { NoRightClickDirective } from './_directives/no-right-click.directive';
import {ErrorHandlerService} from './_services/http/error-handler.service';
import { ErrorDialogComponent } from './_services/http/error-dialog/error-dialog.component';
import { SuccessDialogComponent } from './_services/http/success-dialog/success-dialog.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { JxMatFormControlsModule} from './jx-elements/mat/mat-form-field.module';
import { MaterialModule, JxSharedModule } from './shared';
import { CopyClipboardDirective} from './_directives/copy-clipboard/copy-clipboard.directive';
import { JxImgComponent } from './jx-elements/image/img.component';
import { ConnectionServiceModule, ConnectionServiceOptions, ConnectionServiceOptionsToken, HttpService, KtDialogService, LayoutUtilsService, MessageService, NotificationService, StatusCheckComponent, UploadDownloadService } from './_services';
import { JxNetAuthModule } from './auth-module';
import { ActionNotificationComponent, AlertComponent, DeleteEntityDialogComponent, ErrorComponent, FetchEntityDialogComponent, NoticeComponent, UpdateStatusDialogComponent } from './components';

@NgModule({
	declarations: [		
		EntityStatusPipe,
		AvatarLetterPipe,
		EntityApprovePipe,
		FilterPipe,
		BooleanTextPipe,
		TruncatePipe,
		TooltipTextPipe,
		NullOrEmptyPipe,
		SafeHtmlPipe,
		CopyClipboardDirective,
		SuccessDialogComponent,
    	ErrorDialogComponent,
		JxFileComponent,
		JxDownloadComponent,
		JxDownloaderDirective,
		JxImgComponent,
		StatusCheckComponent,
		NoticeComponent,
		ErrorComponent,
		ActionNotificationComponent,
		DeleteEntityDialogComponent,
		FetchEntityDialogComponent,
		UpdateStatusDialogComponent,
		AlertComponent,
		
		//NoRightClickDirective,		
	],
	exports: [
		JxSharedModule,
		MaterialModule,
		FlexLayoutModule,
		//custom modules
		JxMatFormControlsModule,

		EntityStatusPipe,
		AvatarLetterPipe,
		EntityApprovePipe,
		FilterPipe,
		BooleanTextPipe,
		TruncatePipe,
		TooltipTextPipe,
		NullOrEmptyPipe,
		SafeHtmlPipe,
		JxDownloaderDirective,
		CopyClipboardDirective,
		//NoRightClickDirective,
		SuccessDialogComponent,
		ErrorDialogComponent,
		JxFileComponent,
		JxDownloadComponent,
		JxImgComponent,
		StatusCheckComponent,
		NoticeComponent,
		ErrorComponent,
		ActionNotificationComponent,
		DeleteEntityDialogComponent,
		FetchEntityDialogComponent,
		UpdateStatusDialogComponent,
		AlertComponent,
	],
	imports: [
		JxSharedModule,
		MaterialModule,
		FlexLayoutModule,
		//custom modules
		JxNetAuthModule,
		JxMatFormControlsModule,
		ConnectionServiceModule
	],
	entryComponents: [
		StatusCheckComponent,
		SuccessDialogComponent,
		ErrorDialogComponent,
		DeleteEntityDialogComponent,
		FetchEntityDialogComponent,
		UpdateStatusDialogComponent,
	],
	schemas: [
		CUSTOM_ELEMENTS_SCHEMA,
		NO_ERRORS_SCHEMA
	],
	providers: [
		{ provide: DateAdapter, useClass: JxMatDateAdapter },
		{ provide: MAT_DATE_FORMATS, useValue: JXMAT_DATE_FORMATS },
		UploadDownloadService,
		MessageService,
		HttpService,
		ErrorHandlerService,
		NotificationService,
		KtDialogService,
		LayoutUtilsService,
		{
			provide: ConnectionServiceOptionsToken,
			useValue: <ConnectionServiceOptions>{
				enableHeartbeat: false,
				heartbeatUrl: 'iam/heart-beat',
				requestMethod: 'get',
				heartbeatInterval: 3000
			}
		}		
	]
})
export class JxNetCoreModule {
	static forRoot(jxModuleConfig: JxModuleConfig): ModuleWithProviders<JxNetCoreModule> {
		return {
		  ngModule: JxNetCoreModule,
		  providers: [
			{
			  provide: JX_MODULE_CONFIG,
			  useValue: jxModuleConfig
			}
		  ]
		};
	  }
 }
