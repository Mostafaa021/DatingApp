import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';
import { MemeberMessageComponent } from '../memeber-message/memeber-message.component';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';

@Component({
  standalone : true,
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'] ,
  imports : [CommonModule , TabsModule, GalleryModule , TimeagoModule , MemeberMessageComponent ]
  
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static : true}) memberTabs? : TabsetComponent
  member :Member = {} as Member;
  images : GalleryItem[] = [];
  users?:User;
  activeTab ?: TabDirective
  messages : Message[] = []
// There is a new version of ngx-gallery realesed for Angular 16 called ng-gallery ==> Leason 113

  constructor(private memberService:MemberService , private route:ActivatedRoute , private messageService:MessageService,
    private accountService:AccountService){
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next:user=> {
          if(user) this.users = user
        }
      })
  
  }
  ngOnInit(): void {
    // we should here make  route resolver to get data before even component to be constructed 
    //this.loadMembers();
    // route resolver make sure that we get data before component constructed 
  this.route.data.subscribe({
    next: data => this.member = data['member'] // => member which is key is the member that passed as object key in routing module as resolver
  })

  this.route.queryParams.subscribe({
    next: params => {
      params['tab'] && this.selectTab(params['tab'])  
      
    }
   })

   this.getImages();
   
   
  }

  selectTab(heading:string){
    if(this.memberTabs) {
      this.memberTabs.tabs.find(x=>x.heading === heading)!.active = true
    }
  }
  onActivatedTab(data:TabDirective){
    this.activeTab = data ; 
    if(this.activeTab.heading ==="Messages"){
      this.loadMessages();
    }
  }
  loadMessages(){
    if(this.member){
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
    
   }
  getImages() {
    if(!this.member) return;
  
    for(const photo of this.member.photos){
      this.images.push(new ImageItem({src: photo.url , thumb:photo.url}))
    }
   
  }
//  loadMembers(){
//   const  username = this.route.snapshot.paramMap.get('username')
//   if(!username) return;
//   this.memberService.getMember(username).subscribe({
//     next : member => {
//       this.member = member;
//       //  legacy code while using ngx-gallery
//       //this.galleryImages = this.getImages(); //here we added getimages() in loadMembers() to ensure that member photos already loaded 
//     }
//   })
//   }
 }

