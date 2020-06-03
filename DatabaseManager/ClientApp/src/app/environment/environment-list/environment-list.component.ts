import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { MigrationClient, Environment } from 'src/app/migration-api/migration-api.module';

@Component({
	selector: 'app-environment-list',
	templateUrl: './environment-list.component.html',
	styleUrls: ['./environment-list.component.scss']
})
export class EnvironmentListComponent implements OnInit {
	environments: Environment[] = [];
	selectedEnvironment: Environment = null;
	loading = false;
	
	@ViewChild('environmentDrawer', {static: false}) public environmentSidenav: MatSidenav;
	constructor(public migrationClient: MigrationClient,
		public changeDetectorRef: ChangeDetectorRef) { }

	ngOnInit() {
		this.loading = true;
		this.migrationClient.listAll()
		.subscribe((environments) => {
			this.environments = environments;
			this.loading = false;
		},
		(error) => {
			
		});
	}

	public onEnvironmentChange(val: Environment){
		let env = this.environments.find((e) => e.databaseEnvironment == val.databaseEnvironment);
		env.migrations = val.migrations;
	}

	public onClickEnvironmentCard(environment: Environment): void{
		//If already on the environment
		if(this.selectedEnvironment == environment){
			this.selectedEnvironment = null;
			this.toggleDrawer();
			return;
		}

		//If the drawer is not open
		if(this.selectedEnvironment === null){
			this.selectedEnvironment = environment;
			this.toggleDrawer();
		}	else	{
			this.selectedEnvironment = environment;
		}

	}

	private toggleDrawer(): void{
		this.environmentSidenav.toggle();
	}

}
