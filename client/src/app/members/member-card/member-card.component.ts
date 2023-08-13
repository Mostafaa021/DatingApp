import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member:Member|undefined ; // here we defined member or undefined to go around initializing problem
  constructor(private memberService : MemberService , private toastr:ToastrService){

  }
  ngOnInit(): void {
    
  }
  addLike(member:Member){
    this.memberService.addLike(member.userName).subscribe({
      next : () => this.toastr.success("You have liked " + member.knownAs)
    })
  }
}
/* 
--here we created child component member card that takes member as input from parent component (member list)
--then  inside html we make layout of card 
--in memberlist html we passed child component  as selector <app-member-card [member]="member"></app-member-card> 
 */