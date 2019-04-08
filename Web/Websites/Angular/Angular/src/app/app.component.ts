import { AppService } from './app.service';
import { Component, OnInit } from '@angular/core';

import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public user = 'mteixeira';
  public pass = 'abc123';
  public fullname = 'Maycon Teixeira';
  public document = '11709501677';
  public birthdate = new Date(1992, 2, 12);

  private _hubConnection: any;

  constructor(private appService: AppService) {

  }

  public ngOnInit(): void {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:56856/jesus')
      .build();

    this._hubConnection.start();
    this._hubConnection.on('Notify', () => {
      this.aaa();
    });
  }

  public aaa(): void {
    console.log('USUARIO INVALIDO!');
  }

  public submit(): void {
    const user = {
      username: this.user,
      password: this.pass,
      fullname: this.fullname,
      document: this.document,
      birthdate: this.birthdate
    };

    this.appService
      .signUpUser(user)
      .subscribe(res => console.log('RES => ', res), err => console.log('ERR => ', err));
  }

}
