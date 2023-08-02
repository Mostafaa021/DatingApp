import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount : number  = 0 

  constructor(private spinnerService:NgxSpinnerService) {

   }

   busy() {
    this.busyRequestCount ++ ; 
    this.spinnerService.show(undefined, {
      type:"line-spin-fade",
      fullScreen: true ,
      bdColor: "rgba(8,8,8,0.8)",
      color : "#fff"
    })
   }
   idle(){
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0 ){
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
   }
}
