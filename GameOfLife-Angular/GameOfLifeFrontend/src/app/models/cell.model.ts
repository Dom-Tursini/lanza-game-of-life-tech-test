export interface Cell {
  x: number;
  y: number;
  state: 'dead' | 'alive' | 'dying';
}
