import { Injectable } from '@angular/core';
import { Environment, EnvironmentMigration } from '../migration-api/migration-api.module';

@Injectable({
	providedIn: 'root'
})
export class EnvironmentService {

	constructor() { }

	public GetMigrationsOrderedByDate(environment: Environment): EnvironmentMigration[]{
		return [...environment.migrations].sort((a, b) => {
            return (a.date > b.date) ? 1 : -1;
        });
	}
	public GetMigrationsOrderedByDateDesc(environment: Environment): EnvironmentMigration[]{
		return [...environment.migrations].sort((a, b) => {
            return (a.date < b.date) ? 1 : -1;
        });
	}

	public HasUnappliedMigrationBeforeCurrent(environment: Environment): boolean{
		var orderedMigration = this.GetMigrationsOrderedByDate(environment);

		var foundNotAppliedMigration = false;
		for (let index = 0; index < orderedMigration.length; index++) {
            let migration = orderedMigration[index];
			if(migration.applied === false){
				foundNotAppliedMigration = true;
				continue;
			}
			if(foundNotAppliedMigration === true){
				return true;
			}
		}

		return false;
	}
	
	public GetCurrentMigration(environment: Environment): EnvironmentMigration{
		let currentMigration: EnvironmentMigration;

		var orderedDescMigration = this.GetMigrationsOrderedByDateDesc(environment);

        for (let index = orderedDescMigration.length - 1; index >= 0; index--) {
            let migration = orderedDescMigration[index];
            if(migration.applied){
                currentMigration = migration;
            }
		}
		return currentMigration
	}

	public GetMigrationsToApply(environment: Environment, targetMigration: EnvironmentMigration){

		var orderedMigration = this.GetMigrationsOrderedByDate(environment);
		var currentMigration = this.GetCurrentMigration(environment);
		let migrations: EnvironmentMigration[] = [];
		const applied = targetMigration.applied;
		var currentMigrationIndex = orderedMigration.indexOf(currentMigration) + 1;
		var targetMigrationIndex = orderedMigration.indexOf(targetMigration) + 1;
		if(applied){//Downgrade
			migrations = orderedMigration.slice(targetMigrationIndex, currentMigrationIndex);
		}	else	{//Upgrade
			migrations = orderedMigration.slice(currentMigrationIndex, targetMigrationIndex);
		}
		
		migrations = migrations.filter((item) => {
			return item.applied === applied;
		});
		
		return migrations;
	}

}
