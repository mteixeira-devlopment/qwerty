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
      brand: 'visa',
      number: '4012001038443335',
      cvv: '123',
      expiration_month: '05',
      expiration_year: '2018'
    };

    paymentProvider.paymentProvider.pay(paymentInformations, (response) => {
      let paymentInfo = {
        value: 500,
        accountId: '4afa6d0c-9459-4acf-8e0a-4c6df7e47037',
        creditCardName: 'Maria',
        creditCardNumber: paymentInformations.number,
        creditCardExpirationYear: paymentInformations.expiration_year,
        creditCardExpirationMonth: paymentInformations.expiration_month,
        creditCardSecurityNumber: paymentInformations.cvv,
        creditCardMask: response.data.card_mask,
        paymentToken: response.data.payment_token
      };

      console.log(response.data);

      this.appService
        .PayCreditCard(paymentInfo)
        .subscribe(res => console.log('FOI'), err => console.log('ERR => ', err));

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
      .subscribe(res => console.log('FOI'), err => console.log('ERR => ', err, err.error.errors[0]));
  }
}
