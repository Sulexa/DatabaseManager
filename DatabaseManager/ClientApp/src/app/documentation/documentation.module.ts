import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentationHomeComponent } from './documentation-home/documentation-home.component';
import { MatListModule } from '@angular/material/list';
import { DocumentationDbTableComponent } from './documentation-db-table/documentation-db-table.component';
import { DocumentationDataTableComponent } from './documentation-data-table/documentation-data-table.component';
import { RouterModule } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';


@NgModule({
  declarations: [DocumentationHomeComponent, DocumentationDbTableComponent, DocumentationDataTableComponent],
  imports: [
    CommonModule,
    RouterModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatTableModule
  ]
})
export class DocumentationModule { }
