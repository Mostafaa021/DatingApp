import { Component,Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {

  @Input() label = '' ;
  @Input() type = 'text' ;

  constructor(@Self() public ngControl:NgControl) {
    this.ngControl.valueAccessor = this ; // Value accessor inside ngcontrol of type NgControl who is base class = TextinputComponent : IControlValueAccesor
    
  }
  
  writeValue(obj: any): void {
    
  }
  registerOnChange(fn: any): void {
    
  }
  registerOnTouched(fn: any): void {
   
  }
  
  get control():FormControl{
    return this.ngControl.control as FormControl 
     // to get work around error of ngControl   FormControl<any>|null
     // getter control that be casted to typeof FormControl<any>
  }
 
}
