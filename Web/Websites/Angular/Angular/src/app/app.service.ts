import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AppService {

  constructor(private http: HttpClient) {

  }

  public signUpUser(user: any): Observable<any> {
    return this.http.post('http://localhost:7000/identity-api/signup', user);
  }
}
