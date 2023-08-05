import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent :any ; // Recievieng Data from Parent Component  just for Testing
  @Output() cancelRegister = new EventEmitter();// Passing or Emit Data  from Child to Parent component
  model : any = {};
  /* using aprroch of Reactive Forms instead of Standard Form  ,
   reactive form can be controlled through component.ts so will be easly to tested
   standard form can be controlled through HTML component so it will be diffcult to be tested  */

  registerForm: FormGroup = new FormGroup({}) // Group of FormContral for Each Instance for reactive Angualr Forms
  constructor( private accountService:AccountService ,
    private toastr:ToastrService)
  {

  }
  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username : new FormControl('',[Validators.required]),
      password : new FormControl('',[Validators.required,
        Validators.minLength(4),Validators.maxLength(8)]),
      confirmPassword : new FormControl('',[Validators.required , this.matchValues('password')]),
    })
    // here to update validation of confirm password if password changed
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: ()=> this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
 //Customize validator 
  matchValues(matchTo:string): ValidatorFn {
    return (control:AbstractControl) =>{ // Everything in Reactive form Drived from Abstract Control 
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching : true}
    } 
  }
  register(){
    console.log(this.registerForm?.value)
    // this.accountService.register(this.model).subscribe({
    //   next:() => {
    //     this.cancel();
    //   },
    //   error: error => {
    //    this.toastr.error(error.error.errors[0])
    //     console.log(error)
    //   }
    // })
  }

  cancel(){
    this.cancelRegister.emit(true)
  }
}
