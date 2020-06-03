import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EnvironmentCardComponent } from './environment-card/environment-card.component';
import { EnvironmentListComponent } from './environment-list/environment-list.component';
import { EnvironmentDetailsComponent } from './environment-details/environment-details.component'; 
import { EnvironmentMigrateDialogComponent } from './environment-migrate-dialog/environment-migrate-dialog.component'; 

import {MatCardModule} from '@angular/material/card'; 
import {MatRippleModule} from '@angular/material/core'; 
import {MatBadgeModule} from '@angular/material/badge';
import {MatSidenavModule} from '@angular/material/sidenav'; 
import {MatTableModule} from '@angular/material/table'; 
import {MatButtonModule} from '@angular/material/button'; 
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatDialogModule} from '@angular/material/dialog';
import {MatSnackBarModule} from '@angular/material/snack-bar'; 
import {MatProgressBarModule} from '@angular/material/progress-bar'; 
import {MatIconModule} from '@angular/material/icon'; 
import { SharedModule } from '../shared/shared.module';


@NgModule({
  entryComponents: [
    EnvironmentMigrateDialogComponent
  ],
  declarations: [
    EnvironmentCardComponent, 
    EnvironmentListComponent, 
    EnvironmentDetailsComponent, 
    EnvironmentMigrateDialogComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatRippleModule,
    MatBadgeModule,
    MatSidenavModule,
    MatTableModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatIconModule,
    SharedModule
  ]
})
export class EnvironmentModule { }
