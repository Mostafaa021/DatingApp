import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  standalone : true , 
  selector: 'app-memeber-message',
  templateUrl: './memeber-message.component.html',
  styleUrls: ['./memeber-message.component.css'],
  imports : [CommonModule,TimeagoModule,FormsModule]
})
export class MemeberMessageComponent implements OnInit {
@ViewChild('messageForm') messageForm?:NgForm
@Input() username?:string
@Input() messages : Message[] = [];
loading : boolean = false ;
messageContent = "";



constructor(private messageService:MessageService) {
}
  ngOnInit(): void {
   
  }

  sendMessage() {
    if(this.username) {
      this.messageService.sendMessage(this.username,this.messageContent).subscribe({
        next : message => {
          this.messages.push(message)
          this.messageForm?.reset();
        }
      })
    }}

    
  }
   

