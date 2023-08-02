import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm:NgForm|undefined // to get access to AngularForm (edit Form)
  @HostListener('window:beforeunload',['$event']) unloadNotification($event:any){
    if(this.editForm?.dirty){
      $event.returnValue = true
    }
  } // access to hosting ( prevent browser from going to another url during edit in form using built in events in angular )
  member:Member|undefined;
  user : User | null = null;
  constructor(private accountservice:AccountService, 
    private memberService:MemberService ,
    private toastr:ToastrService) {
    this.accountservice.currentUser$.pipe(take(1)).subscribe({
      next : user => this.user = user
    })
  }
  ngOnInit(): void {
    this.loadMembers();
  }
  loadMembers(){
    if(!this.user) return ; 
    this.memberService.getMember(this.user.userName).subscribe({
      next: member=> this.member = member
    })
  }
  UpdateProfile(){
    this.memberService.updateMemeber(this.editForm?.value).subscribe({
      next: ()=> {
        this.toastr.success("Succesfully Profile Updated");
        this.editForm?.reset(this.member)
      }
    })
  }
  }

