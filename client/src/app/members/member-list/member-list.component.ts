import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent  implements OnInit{
  members : Member[] = [];
  
  constructor(private memberservice: MemberService) {

  }
  ngOnInit(): void {
    this.loadingMembers();
  }
  loadingMembers(){
    this.memberservice.getMembers().subscribe({
      next : members =>this.members = members,
      error : () => console.log("Error While loading Member List")
    })
  }
}
