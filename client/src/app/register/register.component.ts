import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import {  Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent :any ; // Recievieng Data from Parent Component  just for Testing
  @Output() cancelRegister = new EventEmitter();// Passing or Emit Data  from Child to Parent component
  /* using aprroch of Reactive Forms instead of Standard Form  ,
   reactive form can be controlled through component.ts so will be easly to tested
   standard form can be controlled through HTML component so it will be diffcult to be tested  */

  registerForm: FormGroup = new FormGroup({});// Group of FormContral for Each Instance of  for reactive Angualr Forms
   maxDate : Date = new Date();
   validationErrors ?: string[]
  constructor( private accountService:AccountService ,
    private toastr:ToastrService ,
    private formbuilder : FormBuilder,
    private router:Router)
  {

  }
  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
  }

  initializeForm(){
    this.registerForm = this.formbuilder.group({
      username : ['',Validators.required],
      gender : ['male'],
      knownAs : ['',Validators.required],
      dateOfBirth : ['',Validators.required],
      city : ['',Validators.required],
      country : ['',Validators.required],
      password : ['',[Validators.required,
        Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword : ['',[Validators.required , this.matchValues('password')]],
    })
    // here to update validation of confirm password if password changed
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: ()=> this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
 //Customize validator extend ValidatorFn interface
  matchValues(matchTo:string): ValidatorFn {
    return (control:AbstractControl) =>{ // Everything in Reactive form Drived from Abstract Control 
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching : true}
    } 
  }
  register(){
   const birthDate  = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value); // set date from register form in variable 
   const values = {...this.registerForm.value , dateOfBirth : birthDate} // use spread operator, then use overload to get value of values dateOfBirth : birthDate 
   this.accountService.register(values).subscribe({
      next:() => {
        this.router.navigateByUrl('/members');
      },
      error: error => {
        this.validationErrors = error;
      }
    })
  }

    // this function to handle date constant to UTC for all client  as browser server for local time zone only 
  private getDateOnly(dateOfBirth? : string ){
    if (!dateOfBirth) return; 
    let BirthDate = new Date(dateOfBirth) ;
    return new Date(BirthDate.setMinutes(BirthDate.getMinutes()-BirthDate.getTimezoneOffset()))
    .toISOString().slice(0,10);
  }

  cancel(){
    this.cancelRegister.emit(true)
  }
}
