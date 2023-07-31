import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member:Member|undefined ; // here we defined member or undefined to go around initializing problem
  constructor(){

  }
  ngOnInit(): void {
    
  }

}
/* 
--here we created child component member card that takes member as input from parent component (member list)
--then  inside html we make layout of card 
--in memberlist html we passed child component  as selector <app-member-card [member]="member"></app-member-card> 
 */