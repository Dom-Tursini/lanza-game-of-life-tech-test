import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { ControlsPanelComponent } from './components/controls-panel/controls-panel.component';
import { GameGridComponent } from './components/game-grid/game-grid.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    ControlsPanelComponent,
    GameGridComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
