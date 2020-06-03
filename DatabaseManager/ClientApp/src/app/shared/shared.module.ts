import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StateIndicatorComponent } from './state-indicator/state-indicator.component';



@NgModule({
  declarations: [StateIndicatorComponent],
  imports: [
    CommonModule,
  ],
  exports: [StateIndicatorComponent]
})
export class SharedModule { }
