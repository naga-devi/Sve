import { Injectable } from "@angular/core";
import {
  HttpResponseBase,
  HttpResponse,
  HttpErrorResponse,
} from "@angular/common/http";
import { UploadFileModel } from "../models/upload-file.model";

@Injectable()
export class Utilities {
  public static readonly captionAndMessageSeparator = ":";
  public static readonly noNetworkMessageCaption = "No Network";
  public static readonly noNetworkMessageDetail =
    "The server cannot be reached";
  public static readonly accessDeniedMessageCaption = "Access Denied!";
  public static readonly accessDeniedMessageDetail = "";
  public static readonly notFoundMessageCaption = "Not Found";
  public static readonly notFoundMessageDetail =
    "The target resource cannot be found";

  public static getHttpResponseMessages(
    data: HttpResponseBase | any
  ): string[] {
    const responses: string[] = [];

    if (data instanceof HttpResponseBase) {
      if (this.checkNoNetwork(data)) {
        responses.push(
          `${this.noNetworkMessageCaption}${this.captionAndMessageSeparator} ${this.noNetworkMessageDetail}`
        );
      } else {
        const responseObject = this.getResponseBody(data);

        if (
          responseObject &&
          (typeof responseObject === "object" ||
            responseObject instanceof Object)
        ) {
          for (const key in responseObject) {
            if (key) {
              responses.push(
                `${key}${this.captionAndMessageSeparator} ${responseObject[key]}`
              );
            } else if (responseObject[key]) {
              responses.push(responseObject[key].toString());
            }
          }
        }
      }

      if (!responses.length) {
        if ((<any>data).body) {
          responses.push(`body: ${(<any>data).body}`);
        }

        if ((<any>data).error) {
          responses.push(`error: ${(<any>data).error}`);
        }
      }
    }

    if (!responses.length) {
      if (this.getResponseBody(data)) {
        responses.push(this.getResponseBody(data).toString());
      } else {
        responses.push(data.toString());
      }
    }

    if (this.checkAccessDenied(data)) {
      responses.splice(
        0,
        0,
        `${this.accessDeniedMessageCaption}${this.captionAndMessageSeparator} ${this.accessDeniedMessageDetail}`
      );
    }

    if (this.checkNotFound(data)) {
      let message = `${this.notFoundMessageCaption}${this.captionAndMessageSeparator} ${this.notFoundMessageDetail}`;
      if (data.url) {
        message += `. ${data.url}`;
      }

      responses.splice(0, 0, message);
    }

    return responses;
  }

  public static getHttpResponseMessage(data: HttpResponseBase | any): string {
    const httpMessage =
      Utilities.findHttpResponseMessage(
        Utilities.noNetworkMessageCaption,
        data
      ) ||
      Utilities.findHttpResponseMessage(
        Utilities.notFoundMessageCaption,
        data
      ) ||
      Utilities.findHttpResponseMessage("error_description", data) ||
      Utilities.findHttpResponseMessage("error", data) ||
      Utilities.getHttpResponseMessages(data).join();

    return httpMessage;
  }

  public static getHttpErrorMessage(error: any): string {
    return `Unable to retrieve response from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(
      error
    )}"`;
  }

  public static findHttpResponseMessage(
    messageToFind: string,
    data: HttpResponse<any> | any,
    seachInCaptionOnly = true,
    includeCaptionInResult = false
  ): string {
    const searchString = messageToFind.toLowerCase();
    const httpMessages = this.getHttpResponseMessages(data);

    for (const message of httpMessages) {
      const fullMessage = Utilities.splitInTwo(
        message,
        this.captionAndMessageSeparator
      );

      if (
        fullMessage.firstPart &&
        fullMessage.firstPart.toLowerCase().indexOf(searchString) != -1
      ) {
        return includeCaptionInResult
          ? message
          : fullMessage.secondPart || fullMessage.firstPart;
      }
    }

    if (!seachInCaptionOnly) {
      for (const message of httpMessages) {
        if (message.toLowerCase().indexOf(searchString) != -1) {
          if (includeCaptionInResult) {
            return message;
          } else {
            const fullMessage = Utilities.splitInTwo(
              message,
              this.captionAndMessageSeparator
            );
            return fullMessage.secondPart || fullMessage.firstPart;
          }
        }
      }
    }

    return null;
  }

  public static getResponseBody(response: HttpResponseBase) {
    if (response instanceof HttpResponse) {
      return response.body;
    }

    if (response instanceof HttpErrorResponse) {
      return response.error || response.message || response.statusText;
    }
  }

  public static checkNoNetwork(response: HttpResponseBase) {
    if (response instanceof HttpResponseBase) {
      return response.status == 0;
    }

    return false;
  }

  public static checkAccessDenied(response: HttpResponseBase) {
    if (response instanceof HttpResponseBase) {
      return response.status == 403;
    }

    return false;
  }

  public static checkNotFound(response: HttpResponseBase) {
    if (response instanceof HttpResponseBase) {
      return response.status == 404;
    }

    return false;
  }

  public static checkIsLocalHost(url: string, base?: string) {
    if (url) {
      const location = new URL(url, base);
      return (
        location.hostname === "localhost" || location.hostname === "127.0.0.1"
      );
    }

    return false;
  }

  public static getQueryParamsFromString(paramString: string) {
    if (!paramString) {
      return null;
    }

    const params: { [key: string]: string } = {};

    for (const param of paramString.split("&")) {
      const keyValue = Utilities.splitInTwo(param, "=");
      params[keyValue.firstPart] = keyValue.secondPart;
    }

    return params;
  }

  public static splitInTwo(
    text: string,
    separator: string
  ): { firstPart: string; secondPart: string } {
    const separatorIndex = text.indexOf(separator);

    if (separatorIndex == -1) {
      return { firstPart: text, secondPart: null };
    }

    const part1 = text.substr(0, separatorIndex).trim();
    const part2 = text.substr(separatorIndex + 1).trim();

    return { firstPart: part1, secondPart: part2 };
  }

  public static safeStringify(object: {
    [x: string]: any;
    hasOwnProperty: (arg0: string) => any;
  }) {
    let result: string;

    try {
      result = JSON.stringify(object);
      return result;
    } catch (error) {}

    const simpleObject = {};

    for (const prop in object) {
      if (!object.hasOwnProperty(prop)) {
        continue;
      }
      if (typeof object[prop] == "object") {
        continue;
      }
      if (typeof object[prop] == "function") {
        continue;
      }
      simpleObject[prop] = object[prop];
    }

    result = "[***Sanitized Object***]: " + JSON.stringify(simpleObject);

    return result;
  }

  public static JsonTryParse(value: string) {
    try {
      return JSON.parse(value);
    } catch (e) {
      if (value === "undefined") {
        return void 0;
      }
      return value;
    }
  }

  public static TestIsObjectEmpty(obj: any) {
    for (const prop in obj) {
      if (obj.hasOwnProperty(prop)) {
        return false;
      }
    }

    return true;
  }

  public static TestIsUndefined(value: any) {
    return typeof value === "undefined";
    // return value === undefined;
  }

  public static TestIsString(value: any) {
    return typeof value === "string" || value instanceof String;
  }

  public static capitalizeFirstLetter(text: string) {
    if (text) {
      return text.charAt(0).toUpperCase() + text.slice(1);
    } else {
      return text;
    }
  }

  public static toTitleCase(text: string) {
    return text.replace(/\w\S*/g, (subString) => {
      return (
        subString.charAt(0).toUpperCase() + subString.substr(1).toLowerCase()
      );
    });
  }

  public static toLowerCase(items: string);
  public static toLowerCase(items: string[]);
  public static toLowerCase(items: any): string | string[] {
    if (items instanceof Array) {
      const loweredRoles: string[] = [];

      for (let i = 0; i < items.length; i++) {
        loweredRoles[i] = items[i].toLowerCase();
      }

      return loweredRoles;
    } else if (typeof items === "string" || items instanceof String) {
      return items.toLowerCase();
    }
  }

  public static uniqueId() {
    return this.randomNumber(1000000, 9000000).toString();
  }

  public static randomNumber(min: number, max: number) {
    return Math.floor(Math.random() * (max - min + 1) + min);
  }

  public static baseUrl() {
    let base = "";

    if (window.location.origin) {
      base = window.location.origin;
    } else {
      base =
        window.location.protocol +
        "//" +
        window.location.hostname +
        (window.location.port ? ":" + window.location.port : "");
    }

    return base.replace(/\/$/, "");
  }

  public static getUploadFileFromFileInput(controlValue: any): UploadFileModel {    
    if (controlValue) {
          let files = new Array<UploadFileModel>();
          controlValue.forEach(file => {
            if(file){
                files.push({ data: file.preview, name: file.file.name });
            }
          });
          if(files.length > 0){
              return files[0];
          }
    }
    return new UploadFileModel();
  }

  public static getUploadFilesFromFileInput(controlValue: any): Array<UploadFileModel> { 
    let files = new Array<UploadFileModel>();   
    if (controlValue) {
          controlValue.forEach(file => {
            if(file){
                files.push({ data: file.preview, name: file.file.name });
            }
          });
    }
    return files;
  }

  public static prepareFormData(inputModel: any): FormData {
    let formData = new FormData();
    Object.keys(inputModel).forEach((controlName) =>
      formData.append(controlName, inputModel[controlName])
    );

    return formData;
  }

  public static prepareFileUploadFormData(
    inputModel: any,
    itemAlias: string,
    uploadedList: FileList
  ): FormData {
    let formData = new FormData();
    Object.keys(inputModel).forEach((controlName) =>
      formData.append(controlName, inputModel[controlName])
    );

    if (uploadedList && uploadedList !== null) {
      for (let i = 0; uploadedList.length > i; i++) {
        formData.append(itemAlias, uploadedList[i], uploadedList[i].name);
      }
    }

    return formData;
  }

  public static prepareSingleFileUploadFormData(
    inputModel: any,
    itemAlias: string,
    uploadedFile: File
  ): FormData {
    let formData = new FormData();
    Object.keys(inputModel).forEach((controlName) =>
      formData.append(controlName, inputModel[controlName])
    );

    if (uploadedFile && uploadedFile !== null) {
      formData.append(itemAlias, uploadedFile, uploadedFile.name);
    }

    return formData;
  }

  public static getLetterAvatar(name: string) {
    if (!name) {
      return name;
    }

    name = name || "";

    const nameSplit = String(name).toUpperCase().split(" ");
    let initials = "";

    if (nameSplit.length === 1) {
      initials = nameSplit[0] ? nameSplit[0].charAt(0) : "?";
    } else {
      initials = nameSplit[0].charAt(0) + nameSplit[1].charAt(0);
    }

    return initials;
  }
  /**
   * Convert number to string and addinng '0' before
   *
   * @param value: number
   */
  public static padNumber(value: number) {
    if (this.isNumber(value)) {
      return `0${value}`.slice(-2);
    } else {
      return "";
    }
  }

  /**
   * Checking value type equals to Number
   *
   * @param value: any
   */
  public static isNumber(value: any): boolean {
    return !isNaN(this.toInteger(value));
  }

  /**
   * Covert value to number
   *
   * @param value: any
   */
  public static toInteger(value: any): number {
    if (value == null || value === undefined) return 0;
    if (value && value.length === 0) return 0;
    return parseInt(`${value}`, 10);
  }

  public static toNumber(value: any): number {
    if (Utilities.isNumber(value)) return value;

    return 0;
  }

  public static parseFloat(value: any): number {
    if (!Utilities.isNullOrEmpty(value)) return parseFloat(value);

    return 0;
  }

  public static toFixed(value: any, roundRange: number = 2): number {
    if (!Utilities.isNullOrEmpty(value)) return value.toFixed(roundRange);

    return 0;
  }

  public static isNullOrEmpty(value: any): boolean {
    return (
      // null or undefined
      value == null ||
      // has length and it's zero
      (value.hasOwnProperty("length") && value.length === 0) ||
      // is an Object and has no keys
      (value.constructor === Object && Object.keys(value).length === 0)
    );
  }

  //download file from blob
  public static downloadFile(data: HttpResponse<Blob>, filename: string = "") {
    const contentDisposition =
      data.headers && data.headers.get("content-disposition");
    if (contentDisposition) {
      filename = Utilities.getFilenameFromContentDisposition(
        contentDisposition
      );
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

  private static openLink(url: string, filname: string) {
    let a = document.createElement("a");
    // Firefox requires the link to be in the body
    document.body.appendChild(a);
    a.style.display = "none";
    a.href = url;
    a.download = filname;
    a.click();
    // Remove the link when done
    document.body.removeChild(a);
  }

  public static getFilenameFromContentDisposition(
    contentDisposition: string
  ): string {
    const regex = /filename=(?<filename>[^,;]+);/g;
    const match = regex.exec(contentDisposition);
    const filename = match.groups.filename;
    return filename;
  }

  public static getFileMIMEType(fileName: string) {
    //file type extension
    let checkFileType = fileName.split(".").pop();
    let fileType: string;

    if (checkFileType == ".txt") {
      fileType = "text/plain";
    }
    if (checkFileType == ".pdf") {
      fileType = "application/pdf";
    }
    if (checkFileType == ".doc") {
      fileType = "application/vnd.ms-word";
    }
    if (checkFileType == ".docx") {
      fileType = "application/vnd.ms-word";
    }
    if (checkFileType == ".xls") {
      fileType = "application/vnd.ms-excel";
    }
    if (checkFileType == ".png") {
      fileType = "image/png";
    }
    if (checkFileType == ".jpg") {
      fileType = "image/jpeg";
    }
    if (checkFileType == ".jpeg") {
      fileType = "image/jpeg";
    }
    if (checkFileType == ".gif") {
      fileType = "image/gif";
    }
    if (checkFileType == ".csv") {
      fileType = "text/csv";
    }

    return fileType;
  }
}
