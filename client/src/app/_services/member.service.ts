import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';
import { Pagination, PaginationResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

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
  getMembers(userParams:UserParams){
    let params = this.getPaginationHeaders(userParams.pageNumber , userParams.pageSize);
    params = params.append('minAge' , userParams.minAge);
    params = params.append('maxAge' , userParams.maxAge);
    params = params.append('gender' , userParams.gender);
    return this.getPaginatedResults<Member[]>(this.baseUrl ,params)
  }
  private getPaginatedResults<T>(url : string , params: HttpParams) {
    const paginatedResult : PaginationResult<T> = new PaginationResult<T>;
    return this.http.get<T>(url + 'users', { observe: "response", params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.results = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }
        return paginatedResult;
      }
      )
    );
  }

  private getPaginationHeaders(pageNumber:number , pageSize:number) {
    let params = new HttpParams();
      params = params.append('pageNumber',pageNumber);
      params = params.append('pageSize', pageSize);
    
    return params;
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
