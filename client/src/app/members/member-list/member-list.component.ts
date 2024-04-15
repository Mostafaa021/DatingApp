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
  pagination ?: Pagination;
  userParams ?: UserParams;
  genderList = [{value : 'male' , display : 'Males'},
                {value : 'female' , display : 'Females'}]

  showBoundaryLinks = true;

  constructor(private memberservice: MemberService ) {
    this.userParams = this.memberservice.getUserParams()
  }
  ngOnInit(): void {
   // this.members$  = this.memberservice.getMembers();
   this.loadMembers()
  }

  loadMembers(){
    if(this.userParams) {
      this.memberservice.setUserParams(this.userParams)
    this.memberservice.getMembers(this.userParams).subscribe({
      next:response =>{
        if(response.results && response.pagination){
          this.members = response.results
          this.pagination = response.pagination
        }
      }
    })
  }
  }

   resetFilters(){
      this.userParams = this.memberservice.resetUserParams();
      this.loadMembers();
   }
pageChanged(event:any){
if(this.userParams && this.userParams?.pageNumber !== event.page)
  this.userParams.pageNumber = event.page;
  if(this.userParams)
  this.memberservice.setUserParams(this.userParams);
  this.loadMembers();
}

}

