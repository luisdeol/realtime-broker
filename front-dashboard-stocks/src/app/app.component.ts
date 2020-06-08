import { Component, EventEmitter } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  dictStocks = {
    "ITSA4": 0,
    "TAEE11": 0,
    "PETR4": 0
  };

  private _hubConnection: HubConnection;
  
  constructor() {  
    this.createConnection();  
    this.registerOnServerEvents();  
    this.startConnection();  
  }  
  
  connectToStock(symbol: string) {  
    this._hubConnection.invoke('ConnectToStock', symbol);  
  }  
  
  private createConnection() {  
    this._hubConnection = new HubConnectionBuilder()  
      .withUrl('http://localhost:5000/brokerhub')  
      .build();  
  }  
  
  private startConnection(): void {  
    this._hubConnection  
      .start()  
      .then(() => {   
        console.log('Hub connection started');  
        
        for (let key in this.dictStocks) {
          this.connectToStock(key);
        }
      })  
      .catch(() => {  
        setTimeout(function () { this.startConnection(); }, 5000);  
      });  
  }  
  
  private registerOnServerEvents(): void {  
    this._hubConnection.on('UpdatePrice', (data: any) => {
      this.dictStocks[data.symbol] = data.price;
    });  
  }  
}
