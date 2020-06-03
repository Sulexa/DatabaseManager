import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { IScript } from '../script.definition';
import { DatabaseEnvironment } from 'src/app/migration-api/migration-api.module';
import { SnackBarService } from 'src/app/shared/snack-bar/snack-bar.service';

@Component({
  selector: 'app-script-details',
  templateUrl: './script-details.component.html',
  styleUrls: ['./script-details.component.scss']
})
export class ScriptDetailsComponent implements OnChanges {
  @Input() script: IScript;
  loading: boolean;
  selectedEnvironment: DatabaseEnvironment = null;
  constructor(public snackBarService: SnackBarService) { }

  ngOnChanges() {
    this.selectedEnvironment = null;
  }

  apply(){
    this.loading = true;
    this.script.api(this.selectedEnvironment)
    .subscribe(() => {
        this.snackBarService.success("Script appliqué avec succés");
    }, (error) => {
      var response = JSON.parse(error.response);
      this.snackBarService.error(response.title);
    }, () => {
        this.loading = false;
    } )
  }
}
