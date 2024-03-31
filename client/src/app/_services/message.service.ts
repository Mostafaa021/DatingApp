import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginatedResults, getPaginationHeaders } from './PaginationHelper';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = environment.apiUrl

  constructor( private http:HttpClient) { }

  getMessages(pageNumber : number , pageSize : number , container : string ) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append("Container", container); 
    return getPaginatedResults<Message[]>(this.baseUrl + 'Message' , params,this.http);
  }
}
