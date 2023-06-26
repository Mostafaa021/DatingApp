import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
//in NavBar Component Constructor Inject Account Service
// in Login Function which Return Observable from injected Service => subscribe Observable
export class NavbarComponent implements OnInit {
  model : any = {};
  
  constructor(public accountService:AccountService ,  // make public injectable to can be used through navbar component Template 
     private router:Router ,
    private toastr:ToastrService){ 

  }
  
  ngOnInit(): void {

  }
  
 // Subscriber of Observable 
  login(){
  this.accountService.login(this.model).subscribe({
    next : () => this.router.navigateByUrl('/members'), 
    error : error => this.toastr.error(error.error)
  })
}
logout(){
  this.accountService.logout();
  this.router.navigateByUrl('/');
 
}
}
