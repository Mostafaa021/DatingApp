import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  users:any;
  registerMode : boolean = true ; // make field to use in Structural directive (adding or Removing From DOM)
 constructor(private http:HttpClient){

 }
  ngOnInit(): void {
    this.getUsers();
    
  }
  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe({
      next : responce => this.users =responce ,
      error : error => console.log(error),
      complete : ()=> console.log('Request has Completed')
    })
  }
  registerToggle(){ // Toggle Register mode when called from true to False
    this.registerMode = !this.registerMode; 
  }
  
  cancelRegisterMode(event : boolean){
    this.registerMode = event
  }
}
