import { Component, OnInit, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { Environment, MigrationClient, EnvironmentMigration, ApiException } from 'src/app/migration-api/migration-api.module';
import { EnvironmentMigrateDialogComponent, IEnvironmentMigrateDialogDate } from '../environment-migrate-dialog/environment-migrate-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { SnackBarService } from 'src/app/shared/snack-bar/snack-bar.service';
import { EnvironmentService } from '../environment.service';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-environment-details',
    templateUrl: './environment-details.component.html',
    styleUrls: ['./environment-details.component.scss']
})
export class EnvironmentDetailsComponent implements OnChanges {
    @Input() environment: Environment;
    @Output() environmentChange: EventEmitter<Environment> = new EventEmitter<Environment>();
    displayedColumns: string[] = ['name', 'date', 'applied', 'action'];
    currentMigrationId: string = null;
    loading: boolean = false;
    hasUnappliedMigrationBeforeCurrent: boolean = false;
    constructor(public migrationClient: MigrationClient,
        public dialog: MatDialog,
        public snackBarService: SnackBarService,
        public environmentService: EnvironmentService) { }

    ngOnChanges() {        
        if(!this.environment){
            return;
        }
        this.treatMigrations();
    }

    treatMigrations(){
        this.currentMigrationId = null;
        this.hasUnappliedMigrationBeforeCurrent = this.environmentService.HasUnappliedMigrationBeforeCurrent(this.environment);
        this.environmentService.GetCurrentMigration(this.environment);
        this.environment.migrations = this.environment.migrations.sort((a, b) => {
            return (a.date > b.date) ? -1 : 1;
        });
        for (let index = this.environment.migrations.length-1; index >= 0; index--) {
            let migration = this.environment.migrations[index];
            if(migration.applied){
                this.currentMigrationId = migration.id;
            }
        }
    }

    repairMigrations(){
        var currentMigration = this.environment.migrations.find(
            (migration: EnvironmentMigration) => {
                return migration.id === this.currentMigrationId;
            }
        )
        
        this.openMigrateDialog(currentMigration);
    }

    
    openMigrateDialog(migration: EnvironmentMigration): void {
        let data: IEnvironmentMigrateDialogDate = {
            environment: this.environment,
            targetMigration: migration
        }

        const dialogRef = this.dialog.open(EnvironmentMigrateDialogComponent, {
            data: data
        });

        dialogRef.afterClosed().subscribe(result => {
            if(result === true){
                this.changeMigration(migration);
            }
        });
    }

    changeMigration(migration: EnvironmentMigration) {
        this.loading = true;
        this.migrationClient.set(this.environment.databaseEnvironment, migration.id)
        .pipe(finalize(() => this.loading = false))
        .subscribe((result: EnvironmentMigration[]) => {
            this.snackBarService.success("Migration effectué");
            this.environment.migrations = result;
            this.treatMigrations();
            this.environmentChange.emit(this.environment);
        }, (error: ApiException) => {
            var response = JSON.parse(error.response);
            this.snackBarService.error(response.title);
        });
    }

}
