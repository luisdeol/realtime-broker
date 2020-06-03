import { Component, EventEmitter } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'front-dashboard-stocks';
  itsa4Value: number;

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
        
        this.connectToStock("ITSA4");
      })  
      .catch(err => {  
        console.log('Error while establishing connection, retrying...');  
        setTimeout(function () { this.startConnection(); }, 5000);  
      });  
  }  
  
  private registerOnServerEvents(): void {  
    this._hubConnection.on('UpdatePrice', (data: any) => {  
      console.log(data);
      this.itsa4Value = data;
    });  
  }  
}
