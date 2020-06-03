import { Component, Input, OnChanges } from '@angular/core';
import { Environment } from 'src/app/migration-api/migration-api.module';

@Component({
  selector: 'app-environment-card',
  templateUrl: './environment-card.component.html',
  styleUrls: ['./environment-card.component.scss']
})
export class EnvironmentCardComponent implements OnChanges {
  @Input() environment: Environment;
  migrationToApply: number;
  constructor() { }

  ngOnChanges() {
    this.setState();
  }

  public setState(){
    this.migrationToApply = this.environment.migrations.filter(function (migration) {
      return migration.applied === false;
    })
    .length;
  }

}
