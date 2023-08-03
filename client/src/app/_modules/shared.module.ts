import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';





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
      type:'line-spin-fade' 
    }),
    FileUploadModule
  ] ,
  exports :[
    TooltipModule,
    BsDropdownModule,
    TabsModule,
    ToastrModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule
  ]
})
export class SharedModule { }
