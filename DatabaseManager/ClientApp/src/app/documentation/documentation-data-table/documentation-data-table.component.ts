import { Component, OnInit } from '@angular/core';
import { Environment, MigrationClient, DataTable } from 'src/app/migration-api/migration-api.module';

@Component({
  selector: 'app-documentation-data-table',
  templateUrl: './documentation-data-table.component.html',
  styleUrls: ['./documentation-data-table.component.scss']
})
export class DocumentationDataTableComponent implements OnInit {
  environments: Environment[] = [];
  selectedEnvironment: Environment = null;
  tables: string[] = [];
  selectedTable: string = null;
  tableContent: DataTable = null;
  loading = true;

  constructor(public migrationClient: MigrationClient) { }

  ngOnInit() {
		this.loading = true;
      this.migrationClient.listAll()
      .subscribe((environments) => {
        this.environments = environments;
      },
      (error) => {
        
      },
      () => {
        this.loading = false;
      });
  }

  selectEnvironment(environment: Environment) {
    this.selectedEnvironment = environment;
		this.loading = true;
    this.migrationClient.tableList(environment.databaseEnvironment)
      .subscribe((tables) => {
        this.tables = tables;
      },
      (error) => {
        
      },
      () => {
        this.loading = false;
      });
  }

  resetEnvironment() {
    this.selectedEnvironment = null;
    this.tables = [];
    this.tableContent = null;
    this.resetTable();
  }

  selectTable(table: string) {
    this.selectedTable = table;
    this.migrationClient.tableContent(this.selectedEnvironment.databaseEnvironment, table)
    .subscribe((table) => {
      this.tableContent = table;
    },
    (error) => {
      
    },
    () => {
      this.loading = false;
    });
  }

  resetTable() {
    this.selectedTable = null;
  }

}
