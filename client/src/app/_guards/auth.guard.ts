import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { map } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';


export const authGuard : CanActivateFn = (route , state) => { // Guards in Angular now don`t need to implement CanActivate Interface 
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  
      
   return accountService.currentUser$.pipe(
    map(user => {
      if(user) return true ; 
      else {
        toastr.error("you don`t have Permission")
        return false
      }
    }
    )
   )
  }
  

