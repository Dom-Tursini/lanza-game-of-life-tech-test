import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class GridService {
  private hubConnection!: signalR.HubConnection;
  public grid$ = new BehaviorSubject<any[][]>([]);
  public status$ = new BehaviorSubject<string>('reset');
  public connectionEstablished$ = new BehaviorSubject<boolean>(false);

  constructor() {
    this.startConnection();
  }

  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5171/gamehub')
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('✅ SignalR connected');
        this.connectionEstablished$.next(true);
      })
      .catch((err) => console.error('❌ SignalR connection error:', err));

    this.hubConnection.on('updateGrid', (data) => {
      this.grid$.next(data.grid);
    });

    this.hubConnection.on('status', (data) => {
      this.status$.next(data.state);
    });
  }

  send(event: string, payload: any): void {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      console.log(`📤 Sending to backend: "${event}"`, payload);
      this.hubConnection.invoke(event, payload).catch((err) => {
        console.error(`❌ Failed to invoke "${event}":`, err);
      });
    } else {
      console.error(
        '❌ Cannot send data if the connection is not in the Connected state.'
      );
    }
  }

  start(): void {
    this.send('SendMessage', { type: 'start' });
  }

  stop(): void {
    this.send('SendMessage', { type: 'stop' });
  }

  reset(): void {
    this.send('SendMessage', { type: 'reset' });
  }

  setGridSize(cols: number, rows: number): void {
    this.send('SendMessage', {
      type: 'setGridSize',
      cols,
      rows,
    });
  }

  sendSeedPattern(pattern: string): void {
    this.send('SendMessage', {
      type: 'seed',
      pattern,
    });
  }
}
