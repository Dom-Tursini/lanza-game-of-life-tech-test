import { Component, OnInit } from '@angular/core';
import { GridService } from 'src/app/services/grid.service';

@Component({
  selector: 'app-controls-panel',
  templateUrl: './controls-panel.component.html',
  styleUrls: ['./controls-panel.component.scss'],
})
export class ControlsPanelComponent implements OnInit {
  selectedGridSize = '64 x 36';
  selectedSeed = 'Random';
  gameState = 'reset';

  constructor(private gridService: GridService) {}

  ngOnInit(): void {
    this.gridService.connectionEstablished$.subscribe((connected) => {
      if (connected) {
        console.log('📡 SignalR ready — sending initial grid size');
        this.gridSizeChanged();
        this.seedPatternChanged();
      }
    });

    this.gridService.status$.subscribe((status) => {
      this.gameState = status;
    });
  }

  start(): void {
    console.log('▶ Start clicked');
    this.seedPatternChanged();
    this.gridService.start();
  }

  stop(): void {
    console.log('⏹ Stop clicked');
    this.gridService.stop();
  }

  reset(): void {
    console.log('🔄 Reset clicked');
    this.gridService.reset();
  }

  gridSizeChanged(): void {
    const [cols, rows] = this.selectedGridSize
      .split('x')
      .map((v) => parseInt(v.trim(), 10));

    console.log(`📏 Grid size changed to: ${cols} x ${rows}`);
    this.gridService.setGridSize(cols, rows);
  }

  seedPatternChanged(): void {
    console.log(`🌱 Seed pattern changed to: ${this.selectedSeed}`);
    this.gridService.sendSeedPattern(this.selectedSeed);
  }
}
