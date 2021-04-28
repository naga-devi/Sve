import { ResponseStatus } from "../_enums/ResponseStatus";

export class ResponseModel {
	newId: number = 0;
	code: number;
	message: string;
	returnObject: any;
    status: ResponseStatus = ResponseStatus.Success
}
