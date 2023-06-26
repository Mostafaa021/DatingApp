import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TooltipModule.forRoot(), //importing ngx-bootstrap
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass : 'toast-bottom-right' ,
      timeOut : 1000 ,
      easeTime : 300
    })

  ] ,
  exports :[
    TooltipModule,
    BsDropdownModule,
    ToastrModule
  ]
})
export class SharedModule { }
