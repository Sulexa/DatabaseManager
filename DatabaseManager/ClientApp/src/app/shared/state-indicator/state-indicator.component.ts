import { Component, OnInit } from '@angular/core';

export enum IndicatorState{
  ON,
  OFF
}

@Component({
  selector: 'app-state-indicator',
  templateUrl: './state-indicator.component.html',
  styleUrls: ['./state-indicator.component.scss']
})

export class StateIndicatorComponent implements OnInit {
  IndicatorState = IndicatorState;
  currentState : IndicatorState = IndicatorState.ON;
  

  constructor() { }

  ngOnInit() {
  }

}
