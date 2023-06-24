import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '_models/User';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
//in NavBar Component Constructor Inject Account Service
// in Login Function which Return Observable from injected Service => subscribe Observable
export class NavbarComponent implements OnInit {
  model : any = {};
  
  constructor(public accountService:AccountService){ // make public injectable to can be used through navbar component Template 

  }
  
  ngOnInit(): void {

  }
  
 // Subscriber of Observable 
  login(){
  this.accountService.login(this.model).subscribe({
    next : responce =>{ 
      console.log(responce) ;
      
    }, 
    error : error => console.log(error)
  })
}
logout(){
  this.accountService.logout()
}
}
