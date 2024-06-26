import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {

  @Input() appHasRole : string[] = []; 
  user:User = {} as User ;

  constructor( private viewContainerRef:ViewContainerRef ,
               private templateRef :TemplateRef<any>,
                private accountService:AccountService) {
                  this.accountService.currentUser$.pipe(take(1)).subscribe({
                    next: data => {
                      if (data) this.user = data
                    }
                  })

                 }
  ngOnInit(): void {
    // if user has same roles from roles that i will pass in array 
    if(this.user.roles.some(r =>this.appHasRole.includes(r))) // (r:any) in case  roles has type of any in User interface
    {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
    else{
      this.viewContainerRef.clear();
    }
  }

}
