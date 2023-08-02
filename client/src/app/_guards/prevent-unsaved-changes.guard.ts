import { Injectable } from '@angular/core';

import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard  {
  canDeactivate(component: MemberEditComponent): boolean {
    if(component.editForm?.dirty){
      return confirm("Are you sure you want to leave ? any Unsaved changes will be lost")
    }
    return true;
  }
  
}
