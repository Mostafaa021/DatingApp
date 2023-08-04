import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  standalone : true,
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'] ,
  imports : [CommonModule , TabsModule, GalleryModule ]
  
})
export class MemberDetailComponent implements OnInit {
  member :Member | undefined;
  images : GalleryItem[] = [];
  users?:User;
  
// There is a new version of ngx-gallery realesed for Angular 16 called ng-gallery ==> Leason 113

  constructor(private memberService:MemberService , private route:ActivatedRoute ,
    private accountService:AccountService){
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next:user=> {
          if(user) this.users = user
        }
      })
  
  }
  ngOnInit(): void {
   this.loadMembers();
   this.getImages();
   
  }
  getImages() {
    if(!this.member) return;
  
    for(const photo of this.member.photos){
      this.images.push(new ImageItem({src: photo.url , thumb:photo.url}))
    }
   
  }
 loadMembers(){
  const  username = this.route.snapshot.paramMap.get('username')
  if(!username) return;
  this.memberService.getMember(username).subscribe({
    next : member => {
      this.member = member;
      this.getImages()
      //  legacy code while using ngx-gallery
      //this.galleryImages = this.getImages(); //here we added getimages() in loadMembers() to ensure that member photos already loaded 
    }
  })
  }
 }

