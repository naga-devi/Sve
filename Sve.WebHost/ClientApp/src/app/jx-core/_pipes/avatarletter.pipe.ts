import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml} from '@angular/platform-browser';
import { Utilities } from '../utilities/utilities';
// import { EntityStatus } from '@/_enums';
@Pipe({
  name: 'letterAvatar'
})

export class AvatarLetterPipe implements PipeTransform {

  constructor(private _sanitizer: DomSanitizer) { }

  transform(value: string): SafeHtml {

	  let formattedHtml = Utilities.getLetterAvatar(value);

	  return this._sanitizer.bypassSecurityTrustHtml(formattedHtml);
  }
}
