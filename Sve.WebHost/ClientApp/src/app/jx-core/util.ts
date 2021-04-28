export function isNull(val: any): val is null {
	return val === null;
}

export function isUndefined(val: any): val is undefined {
	return val === undefined;
}

export function isNullOrUndefined(val: any): val is null | undefined {
	return isNull(val) || isUndefined(val);
}

export function isPromise<T>(val: T | Promise<T>): val is Promise<T> {
	return Promise.resolve(val) === val;
}

export function isEmptyOrWhitespace(val: any): val is null | undefined {
	return val.trim().length === 0;
}

export function isNullOrEmpty(value: any): boolean {
	return (
		// null or undefined
		isNull(value) ||
		isUndefined(value) ||
		// has length and it's zero
		(value.hasOwnProperty("length") && value.length === 0) ||
		// is an Object and has no keys
		(value.constructor === Object && Object.keys(value).length === 0)
	);
}

/**
 * The formatString provides a utility method to interpolate the message string and replace with the key values of the errorData
 * @param template
 * @param data
 */
export function formatString(template: string, data: any = {}) {
	Object.keys(data).forEach((key) => {
		template = template.replace(
			new RegExp("{{" + key + "}}", "g"),
			data[key]
		);
	});

	return template;
}

/**
 * Recibe un path y un array de ids.
 * Reemplaza en path el literal definido entre llaves {} con el valor del parametro id por orden de ocurrencia.
 * Itera y reemplaza la primera ocurrencia del literal con el primer id recibido, segunda ocurrencia con el segundo y así sucecivamente.
 * Utilizado cuando se tienen PATH PARAMS
 * Ejemplo de llamada:
 *
 * const replaced = replaceId(presupuesto/{versionPresupuesto}/proveedores/{idProveedor}, 78, 123);
 * console.log('string remplazado: ' + replaced);
 * > string remplazado: presupuesto/78/proveedores/123
 *
 * @param string path
 * @param number ids
 * @returns string
 */
export function replaceId(path: string, ...ids: any[]): string {
	for (const id of ids) {
		if (isNullOrUndefined(id)) {
			console.error(
				"Replace: clave para reemplazar en path no definida: URL: " +
					path +
					", Claves: " +
					(ids ? ids.toString() : "Vacío")
			);
		} else {
			path = path.replace(/{.*?}/, id.toString());
		}
	}
	return path;
}

export const monthNames = [
	"January",
	"February",
	"March",
	"April",
	"May",
	"June",
	"July",
	"August",
	"September",
	"October",
	"November",
	"December",
];

export const dayOfWeekNames = [
	"Sunday",
	"Monday",
	"Tuesday",
	"Wednesday",
	"Thursday",
	"Friday",
	"Saturday",
];

//date related functions
export function getDate(date = null, format = "dd-MM-yyyy") {
	const _date =
		date === undefined || date === null ? new Date() : new Date(date);
	//const d = new Date(_date);
	// let month = '' + (d.getMonth() + 1);
	// let day = '' + d.getDate();
	// const year = d.getFullYear();

	// let data = [{'DD':day},{'MM':month},{'YYYY':year}];

	// Object.keys(data).forEach(key => {
	//   format = format.replace(new RegExp('{{' + key + '}}', 'g'), data[key]);
	// });

	// return format;

	if (!format) {
		format = "dd-mm-yyyy";
	}
	let day = _date.getDate(),
		month = _date.getMonth(),
		year = _date.getFullYear(),
		hour = _date.getHours(),
		minute = _date.getMinutes(),
		second = _date.getSeconds(),
		miliseconds = _date.getMilliseconds(),
		h = hour % 12,
		hh = twoDigitPad(h),
		HH = twoDigitPad(hour),
		mm = twoDigitPad(minute),
		ss = twoDigitPad(second),
		aaa = hour < 12 ? "AM" : "PM",
		EEEE = dayOfWeekNames[_date.getDay()],
		EEE = EEEE.substr(0, 3),
		dd = twoDigitPad(day),
		M = month + 1,
		MM = twoDigitPad(M),
		MMMM = monthNames[month],
		MMM = MMMM.substr(0, 3),
		yyyy = year + "",
		yy = yyyy.substr(2, 2);
	// if (month.length < 2)
	// 	month = '0' + month;
	// if (day.length < 2)
	//   day = '0' + day;

	// checks to see if month name will be used
	format = format
		.replace("hh", hh)
		.replace("h", h.toString())
		.replace("HH", HH)
		.replace("H", hour.toString())
		.replace("mm", mm)
		.replace("m", minute.toString())
		.replace("ss", ss)
		.replace("s", second.toString())
		.replace("S", miliseconds.toString())
		.replace("dd", dd)
		.replace("d", day.toString())

		.replace("EEEE", EEEE)
		.replace("EEE", EEE)
		.replace("yyyy", yyyy)
		.replace("yy", yy)
		.replace("aaa", aaa);
	if (format.indexOf("MMM") > -1) {
		format = format.replace("MMMM", MMMM).replace("MMM", MMM);
	} else {
		format = format.replace("MM", MM).replace("M", M.toString());
	}

	return format;

	//return [year, month, day].join('-');
}

export function toServerDate(date: any = new Date()): string {
	let _date = new Date();
	if (date && typeof date === "string") {
		let d = date.split('-');
		_date = new Date(`${d[1]}-${d[0]}-${d[2]}`);
	} else {
		_date =
			date === undefined || date === null ? new Date() : new Date(date);
	}

	let month = (_date.getMonth() + 1).toString();
	let day = _date.getDate().toString();
	const year = _date.getFullYear();

	if (month.length < 2) month = "0" + month;
	if (day.length < 2) day = "0" + day;

	return `${year}-${month}-${day}`;
}

export function twoDigitPad(num) {
	return num < 10 ? "0" + num : num;
}
