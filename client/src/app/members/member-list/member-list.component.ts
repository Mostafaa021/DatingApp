import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination, PaginationResult } from 'src/app/_models/pagination';
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
  pageNumber = 1 ;
  pageSize = 5;
  showBoundaryLinks = true;
  
  constructor(private memberservice: MemberService) {
  
  }
  ngOnInit(): void {
   // this.members$  = this.memberservice.getMembers();
   this.loadMembers()
  }
 
  loadMembers(){
    this.memberservice.getMembers(this.pageNumber,this.pageSize).subscribe({
      next:response =>{
        if(response.results && response.pagination){
          this.members = response.results
          this.pagination = response.pagination
        }
      }
    })
  }
pageChanged(event:any){
if(this.pageNumber != event.page)
  this.pageNumber = event.page
  this.loadMembers();
}

}

