import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Environment, EnvironmentMigration, MigrationClient } from 'src/app/migration-api/migration-api.module';
import { EnvironmentService } from '../environment.service';
import { environment } from 'src/environments/environment';

export interface IEnvironmentMigrateDialogDate {
	environment: Environment;
	targetMigration: EnvironmentMigration;
}


enum MigrationsState {
	UPGRADE,
	DOWNGRADE,
	REPAIR
}


@Component({
	selector: 'app-environment-migrate-dialog',
	templateUrl: './environment-migrate-dialog.component.html',
	styleUrls: ['./environment-migrate-dialog.component.scss']
})
export class EnvironmentMigrateDialogComponent implements OnInit {

	MigrationState = MigrationsState;
	migrationState: MigrationsState;
	actualMigration: EnvironmentMigration;
	migrationsToApply: EnvironmentMigration[] = [];
	getSqlIsLoading: boolean = false;
	constructor(
		public dialogRef: MatDialogRef<EnvironmentMigrateDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: IEnvironmentMigrateDialogDate,
		public environmentService: EnvironmentService,
		public migrationClient: MigrationClient,) { }

	ngOnInit(): void {
		this.actualMigration = this.environmentService.GetCurrentMigration(this.data.environment);
		this.migrationsToApply = this.environmentService.GetMigrationsToApply(this.data.environment, this.data.targetMigration);
		this.setMigrationState();

	}

	setMigrationState(): void{
		if(this.actualMigration.id === this.data.targetMigration.id){
			this.migrationState = MigrationsState.REPAIR;
		}	else	{
			if(this.data.targetMigration.applied){
				this.migrationState = MigrationsState.DOWNGRADE;
			} else	{
				this.migrationState = MigrationsState.UPGRADE;
			}
		}
	}

	getSql(){
		this.getSqlIsLoading = true;
		let actualMigrationId = this.actualMigration ? this.actualMigration.id : undefined;
		this.migrationClient.sql(this.data.environment.databaseEnvironment, actualMigrationId, this.data.targetMigration.id)
		.subscribe((result) => {	
            var newBlob = new Blob([result.content], { type: "text/plain" });

            // IE doesn't allow using a blob object directly as link href
            // instead it is necessary to use msSaveOrOpenBlob
            if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                window.navigator.msSaveOrOpenBlob(newBlob);
                return;
            }

            // For other browsers: 
            // Create a link pointing to the ObjectURL containing the blob.
            const data = window.URL.createObjectURL(newBlob);

            var link = document.createElement('a');
			link.href = data;
			
			let name = "";
			if(this.actualMigration){
				name += `From_${this.actualMigration.name}_`
			}
			name += `To_${this.data.targetMigration.name}.sql`

            link.download = name;
            // this is necessary as link.click() does not work on the latest firefox
            link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

            setTimeout(function () {
                // For Firefox it is necessary to delay revoking the ObjectURL
                window.URL.revokeObjectURL(data);
                link.remove();
            }, 100);
		}, () => {}
		,() => {
			this.getSqlIsLoading = false;
		});
	}
}
