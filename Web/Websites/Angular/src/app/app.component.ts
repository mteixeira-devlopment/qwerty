import { AppService } from './app.service';
import { Component, OnInit } from '@angular/core';
import paymentProvider from '../gn.js';

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
    this._hubConnection.on('Notify', (a: string) => {
      this.aaa(a);
    });

    console.log('a => ', paymentProvider);
    console.log('b => ', paymentProvider.paymentProvider);

    const paymentInformations = {
      brand: 'mastercard',
      number: '5502095146333358',
      cvv: '687',
      expiration_month: '04',
      expiration_year: '2027'
    };

    paymentProvider.paymentProvider.pay(paymentInformations, (response) => {
      const paymentInfo = {
        accountId: '0fbf34a6-3572-425c-9c79-55c6f599b59c',
        value: 500,
        paymentToken: response.data.payment_token
      };

      console.log(response.data);

      this.appService
        .PayCreditCard(paymentInfo)
        .subscribe(res => console.log(res), err => console.log('ERR => ', err.error.errors[0]));

      console.log('fds');
    });
  }

  public aaa(a: string): void {
    console.log('RETORNO SIGNAL R -> ', a);
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
      .subscribe(res => console.log(res), err => console.log('ERR => ', err.error.errors[0]));
  }
}
