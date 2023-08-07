import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination, PaginationResult } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent  implements OnInit{
  // members$ : Observable<Member[]> |undefined ;
  members :Member[] = []
  pagination : Pagination | undefined;
  userParams : UserParams | undefined;
  user : User | undefined;
  genderList = [{value : 'male' , display : 'Males'},
                {value : 'female' , display : 'Females'}]
 
  showBoundaryLinks = true;
  
  constructor(private memberservice: MemberService , private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next : user=>{
        if(user) {
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    })
  
  }
  ngOnInit(): void {
   // this.members$  = this.memberservice.getMembers();
   this.loadMembers()
  }
 
  loadMembers(){
    if(!this.userParams) return ; 
    this.memberservice.getMembers(this.userParams).subscribe({
      next:response =>{
        if(response.results && response.pagination){
          this.members = response.results
          this.pagination = response.pagination
        }
      }
    })
  }

   resetFilters(){
    if(this.user){
      this.userParams = new UserParams(this.user);
      this.loadMembers();
    }
   }
pageChanged(event:any){
if(this.userParams && this.userParams?.pageNumber !== event.page)
  this.userParams.pageNumber = event.page
  this.loadMembers();
}

}

