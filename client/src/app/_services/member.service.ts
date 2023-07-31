import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl  = environment.apiUrl ;  // assign environment apiurl property 

  constructor(private http:HttpClient)  // in constructor inject http client 
  {


  }
  getMember(username : string){
 return this.http.get<Member>(this.baseUrl + 'users/' + + username , this.getHttpOptions())
  }
 // make method to return  list of members from request with function return options of authorization in header 
  getMembers(){ 
    return this.http.get<Member[]>(this.baseUrl + 'users'  , this.getHttpOptions())
  }

  getHttpOptions(){
  const userString  = localStorage.getItem("user");
  if(!userString) return ; 
    const user = JSON.parse(userString);
    return{
      headers : new HttpHeaders({
        Authorization : 'Bearer ' + user.token  // don`t forget Space Between Bearer and user.token
      })
    }
  }
}
