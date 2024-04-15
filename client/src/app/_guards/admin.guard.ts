import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';

export const adminGuard: CanActivateFn = (route, state) => {

  const accountService  = inject(AccountService);
  const toastr = inject(ToastrService)

   return accountService.currentUser$.pipe(
    map((user)=>{
      if(!user) return false;
      if(user.roles.includes('Admin') ||user.roles.includes('Moderator')) {
        return true ;
      }
      else {
        toastr.error("Not Authorized Area")
        return false ;
      }
    })
   )
};
