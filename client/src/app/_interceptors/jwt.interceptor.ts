import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next : user => {
        if(user){
          request = request.clone({
            setHeaders : {
              Authorization : `Bearer ${user.token}`
            }
          })
        }
      }
    })
    return next.handle(request);
  }
}
 /**
 * here instead of sending jwt authentication token inside indvidual method getHttpOptions in member service
 *  making standalone interceptor to be responsible for sending jwt token 
 * inject account service then instead of subscribe and unsubscribe for observable 
 * use pipe(take(1)) that is means what we get back from account service observable will be completed and no longer resources will be consumed 
 * then add interceptor to app.module in providers
 * then remove old funtion of getHttpOptions and make sure every thing works correctly 
 * 
 */