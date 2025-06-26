import { Component, OnInit } from '@angular/core';
import { GridService } from 'src/app/services/grid.service';
import { Cell } from 'src/app/models/cell.model';

@Component({
  selector: 'app-game-grid',
  templateUrl: './game-grid.component.html',
  styleUrls: ['./game-grid.component.scss'],
})
export class GameGridComponent implements OnInit {
  grid: Cell[][] = [];

  constructor(private gridService: GridService) {}

  ngOnInit(): void {
    this.gridService.grid$.subscribe((grid: Cell[][]) => {
      this.grid = grid;
    });
  }
}
