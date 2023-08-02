import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';





@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TooltipModule.forRoot(), //importing ngx-bootstrap
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ToastrModule.forRoot({
      positionClass : 'toast-bottom-right' ,
      timeOut : 1000 ,
      easeTime : 300
    }),
    NgxGalleryModule,
    NgxSpinnerModule.forRoot({ 
      type:'ball-scale-multiple' 
    })
  ] ,
  exports :[
    TooltipModule,
    BsDropdownModule,
    TabsModule,
    ToastrModule,
    NgxGalleryModule,
    NgxSpinnerModule
  ]
})
export class SharedModule { }
