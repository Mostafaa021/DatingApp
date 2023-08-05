import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorsInterceptor implements HttpInterceptor {

  constructor(private toastr:ToastrService , private router:Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error : HttpErrorResponse) =>{
        if(error){
          switch(error.status){
            case 400 :  // in case of 400 we have 2 choices as Validation error or Bad Request so 
            if(error.error.errors){ // in case of validation error we got HttpErorrResponce with error key have array of erros as objects
              const modelStateErrors = []; // make an empty array to just show array of error then push error in it 
              for (const key in error.error.errors) {
                if(error.error.errors[key]){
                  modelStateErrors.push(error.error.errors[key]) 
                }
              }
              throw modelStateErrors.flat();
            }
            else {
              this.toastr.error(error.error , error.status.toString()); // if bad request error  dipslay Toastr
            }
            break;
            case 401: 
            this.toastr.error('UnAuthorized' , error.error.toString()) //Display UnAuthorized Toastr
            break;
            case 404:
            this.router.navigateByUrl('/not-found') // navigate to new url 
            break;
            case 500:
              const navigiationExtras:NavigationExtras = {state :{error :error.error}}
              this.router.navigateByUrl('/server-error' , navigiationExtras) // navigate to new url 
              break;
              default :
              this.toastr.error('Something Unexpected Went Wrong'); // display toastr
              console.log(error);
              break;
          }
        }
        throw error;
      })
    );
  }
}
