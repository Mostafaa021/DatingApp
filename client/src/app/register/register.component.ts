import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent :any ; // Recievieng Data from Parent Component  just for Testing
  @Output() cancelRegister = new EventEmitter();// Passing or Emit Data  from Child to Parent component
  model : any = {};
  constructor( private accountService:AccountService){

  }
  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next:() => {
        this.cancel();
      },
      error: error => console.log(error)
    })
  }

  cancel(){
    this.cancelRegister.emit(true)
  }
}
