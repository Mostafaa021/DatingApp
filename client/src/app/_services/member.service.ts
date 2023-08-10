import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of, take } from 'rxjs';
import { Pagination, PaginationResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl  = environment.apiUrl ;  // assign environment apiurl property 
  members : Member[] = [] // here we implment member in service as life time of service along Angular not like component just destroyed after it has been called
  membersCache = new Map();
  userParams: UserParams|undefined;
  user: User |undefined;
  
  constructor(private http:HttpClient , private accountService: AccountService)  // in constructor inject http client 
  {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next : user=>{
        if(user) {
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    })
  }
  
  getUserParams(){
    return this.userParams
  }
  setUserParams(userParams:UserParams){
    this.userParams = userParams
  }

  resetUserParams(){
    if(this.user){
      this.userParams = new UserParams(this.user);
      return this.userParams
    }
    return
  }
 // make method to return  list of members from responce with function return options of authorization in header 
  getMembers(userParams:UserParams){
    const response  = this.membersCache.get(Object.values(userParams).join('-'));
    if(response) return of(response)
    let params = this.getPaginationHeaders(userParams.pageNumber , userParams.pageSize);
    params = params.append('minAge' , userParams.minAge);
    params = params.append('maxAge' , userParams.maxAge);
    params = params.append('gender' , userParams.gender);
    params = params.append('orderBy' , userParams.orderBy)
    return this.getPaginatedResults<Member[]>(this.baseUrl ,params).pipe(
      map(response =>{
        this.membersCache.set(Object.values(userParams).join('-'),response)
        return response
      })
    )
  }
  getMember(username : string){
    const member : Member = [...this.membersCache.values()]
    .reduce((arr ,elem)=> arr.concat(elem.results), [])
    .find((m:Member)=> m.userName === username )

    if(member) return of(member)
 
 return this.http.get<Member>(this.baseUrl + 'users/' +  username )
 
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
}
