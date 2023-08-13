import { Component, OnInit } from '@angular/core';
import { MemberService } from '../_services/member.service';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
 members : Member[] |undefined;
 perdicate = 'liked' ;
 pageNumber = 1 ; 
 pageSize = 5 ;
 pagination : Pagination | undefined;
 showBoundaryLinks = true;


  constructor( private memberService:MemberService) {

    
  }
  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes()
  {
    this.memberService.getLikes(this.perdicate ,this.pageNumber,this.pageSize ).subscribe({
      next : response=>{
        this.members = response.results ;
        this.pagination = response.pagination;

      }
    })
  }
  pageChanged(event:any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadLikes();
    }
    }

}
