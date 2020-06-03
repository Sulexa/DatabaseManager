import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { IScript } from '../script.definition';
import { DatabaseEnvironment, MigrationClient } from 'src/app/migration-api/migration-api.module';

@Component({
  selector: 'app-script-list',
  templateUrl: './script-list.component.html',
  styleUrls: ['./script-list.component.scss']
})
export class ScriptListComponent implements OnInit {
  selectedScript: IScript = null;
  scripts: IScript[] = [
    {
      name: "Nettoyage de la base de données",
      description: "Script de nettoyage de base données. (Changement des mots de passe, nettoyage des token mobile, changement des dns...)",
      environments: [
        DatabaseEnvironment.DEV, 
        DatabaseEnvironment.INT,
        DatabaseEnvironment.UAT,
      ],
      api: this.migrationClient.clean.bind(this.migrationClient)
    },
    {
      name: "Restauration du backup de la base de donnée",
      description: "Script permettant de restaurer une backup lms du jour (Applique aussi le script de nettoyage de base de données)",
      environments: [
        DatabaseEnvironment.DEV, 
        DatabaseEnvironment.INT,
        DatabaseEnvironment.UAT,
      ],
      api: this.migrationClient.restore.bind(this.migrationClient)
    },
    {
      name: "Backup de la base de donnée",
      description: "Script permettant d'effectuer une backup de la base de donnée",
      environments: [
        DatabaseEnvironment.PROD,
      ],
      api: this.migrationClient.backup.bind(this.migrationClient)
    },
  ];
  
	@ViewChild('scriptDrawer', {static: false}) public scriptSidenav: MatSidenav;
  constructor(public migrationClient: MigrationClient,) { }

  ngOnInit() {
  }

  public onClickScriptCard(script: IScript){
		//If already on the environment
		if(this.selectedScript == script){
			this.selectedScript = null;
			this.toggleDrawer();
			return;
		}

		//If the drawer is not open
		if(this.selectedScript === null){
			this.selectedScript = script;
			this.toggleDrawer();
		}	else	{
			this.selectedScript = script;
		}
  }

	private toggleDrawer(): void{
		this.scriptSidenav.toggle();
	}
}
