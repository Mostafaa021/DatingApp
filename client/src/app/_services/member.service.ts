import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';
import { Pagination, PaginationResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl  = environment.apiUrl ;  // assign environment apiurl property 
  members : Member[] = [] // here we implment member in service as life time of service along Angular not like component just destroyed after it has been called
  paginatedResult : PaginationResult<Member[]> = new PaginationResult<Member[]>;
  
  constructor(private http:HttpClient)  // in constructor inject http client 
  {
    
  }
  getMember(username : string){
    const member = this.members.find(m=>m.userName == username)
    if(member) return of(member)
 return this.http.get<Member>(this.baseUrl + 'users/' +  username )
 
  }
 // make method to return  list of members from request with function return options of authorization in header 
  getMembers(pageNumber?:number , itemsPerPage?:number){
    let params = new HttpParams();
    if(pageNumber && itemsPerPage)
    {
      params = params.append('pageNumber' , pageNumber);
      params = params.append('pageSize' , itemsPerPage)
    }
    return this.http.get<Member[]>(this.baseUrl + 'users',{observe:"response",params}).pipe(
      map(response=>{
        if(response.body){
          this.paginatedResult.results = response.body
        }
        const pagination = response.headers.get('Pagination')
        if(pagination){
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult
      }
      )
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
