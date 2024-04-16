import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import * as jwt_decode from "jwt-decode";
import { Router } from '@angular/router';


@Injectable({
  providedIn : 'root' // survive for the whole lifetime of application
})
// Making an Account Service Which send Data to Back End Url to bind Data on Data Base 
// inject Http Client Request in Service Constructor 
// when Login function Called => Post Client Object (Model) to API Url which Return Observable
export class AccountService {
  baseUrl = environment.apiUrl;
 private currentUserSource = new BehaviorSubject<User|null>(null) ; // using Behaviour Subject Observable to make All components see Current User
 currentUser$ = this.currentUserSource.asObservable(); // make public property as Observable
  
  constructor(private http:HttpClient , private router:Router ) {}
  // using Rxjs library which deal with observable  , transform observable
  // Login Function Carry Business Logic
  login(model:any){
     return this.http.post<User>(this.baseUrl+'account/login' , model).pipe( 
      map((responce:User) =>{
        const user = responce;
        if(user){
          // localStorage.setItem('user' , JSON.stringify(user)) // Key Value Pair as String
          // this.currentUserSource.next(user); // if user logged in succesfully update CurrentUserSource with user object 
          this.setCurrentUser(user);
          this.router.navigateByUrl(this.baseUrl + "members")
        }
      })
     )
  } 
  // Register Function Carry Business Logic
  register(model:any){
    return this.http.post<User>(this.baseUrl+'account/Register', model).pipe(
      map(user=>{
        if(user){
          // localStorage.setItem('user',JSON.stringify(user))
          // this.currentUserSource.next(user)// if user Registered succesfully update CurrentUserSource with user object
          this.setCurrentUser(user);
          this.router.navigateByUrl(this.baseUrl + "members")
        }
      })
    )
  }

  setCurrentUser(user:User){
     user.roles = [] ; 
     // get roles from payload
     const roles = this.getDecodedToken(user.token).role;
      // if Array of roles or just one role 
      Array.isArray(roles)? user.roles = roles : user.roles.push(roles)

    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user) // Function to be used out from component which inject Account Service to set value in currentUser
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null) // if user logged out set null to object
    this.router.navigateByUrl(this.baseUrl)
  }

  // i`m adding roles to token 
  // so what about geting roles from Payload of Token =>  header.payload.signature [0].[1].[2]
   getDecodedToken(token : string){
    //return jwt_decode.jwtDecode(token) //here another solution to use library to decode token 
    let result = JSON.parse(atob(token.split('.')[1])) // built in function in javascript  ==> atob
    return  result
   }

}
