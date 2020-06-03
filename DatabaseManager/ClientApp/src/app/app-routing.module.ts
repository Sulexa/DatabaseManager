import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EnvironmentListComponent } from './environment/environment-list/environment-list.component';
import { ScriptListComponent } from './script/script-list/script-list.component';
import { DocumentationHomeComponent } from './documentation/documentation-home/documentation-home.component';
import { DocumentationDbTableComponent } from './documentation/documentation-db-table/documentation-db-table.component';
import { DocumentationDataTableComponent } from './documentation/documentation-data-table/documentation-data-table.component';


const routes: Routes = [
  {path: '', redirectTo: '/migrations', pathMatch: 'full'},
  {path: 'migrations', component: EnvironmentListComponent},
  {path: 'scripts', component: ScriptListComponent},
  {path: 'documentation', component: DocumentationHomeComponent},
  {path: 'documentation-db-table', component: DocumentationDbTableComponent },
  {path: 'documentation-data-table', component: DocumentationDataTableComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
