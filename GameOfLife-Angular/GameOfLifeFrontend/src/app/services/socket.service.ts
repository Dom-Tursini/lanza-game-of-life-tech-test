import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SocketService {
  private hubConnection!: signalR.HubConnection;
  private connectionEstablished = new BehaviorSubject<boolean>(false);
  public connectionEstablished$ = this.connectionEstablished.asObservable();

  connect(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5171/gamehub')
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('✅ SignalR connected');
        this.connectionEstablished.next(true);
      })
      .catch((err) => console.error('❌ SignalR error', err));
  }

  on(event: string, callback: (data: any) => void): void {
    this.hubConnection.on(event, callback);
  }

  send(event: string, payload: any = {}): void {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      console.warn(`⚠️ Cannot send "${event}" — not connected yet.`);
      return;
    }

    console.log(`📤 Sending to backend: "${event}"`, payload);
    this.hubConnection.invoke(event, payload).catch((err) => {
      console.error(`❌ Failed to invoke "${event}":`, err);
    });
  }
}
