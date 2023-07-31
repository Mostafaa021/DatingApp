
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  users:any;
  registerMode : boolean = true ; // make field to use in Structural directive (adding or Removing From DOM)
 constructor(){

 }
  ngOnInit(): void {

  }
  
  registerToggle(){ // Toggle Register mode when called from true to False
    this.registerMode = !this.registerMode; 
  }
  
  cancelRegisterMode(event : boolean){
    this.registerMode = event
  }
}
