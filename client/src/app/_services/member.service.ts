import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl  = environment.apiUrl ;  // assign environment apiurl property 
  members : Member[] = [] // here we implment member in service as life time of service along Angular not like component just destroyed after it has been called

  constructor(private http:HttpClient)  // in constructor inject http client 
  {


  }
  getMember(username : string){
    const member = this.members.find(m=>m.userName == username)
    if(member) return of(member)
 return this.http.get<Member>(this.baseUrl + 'users/' +  username )
 
  }
 // make method to return  list of members from request with function return options of authorization in header 
  getMembers(){
    if(this.members.length>0)  return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map( members => {
        this.members = members;
        return members ;
      })
    )
  }
  updateMemeber(member:Member){
    return this.http.put(this.baseUrl + 'users' , member).pipe(
      map(()=>{
       const index =  this.members.indexOf(member)
       this.members[index] = {...this.members[index] , ...member} // using spread operator to update data from sent parameter to already existing parameter with using cachability 
       
      })
    )
  }

  SetMainPhoto(photoId:number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId , {});
  }

  deletePhoto(photoId:number) {
return this.http.delete(this.baseUrl + 'users/delete-photo/'+ photoId )
  }
}
