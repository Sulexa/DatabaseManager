import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScriptListComponent } from './script-list/script-list.component';


import {MatCardModule} from '@angular/material/card'; 
import { MatButtonModule } from '@angular/material/button';
import {MatGridListModule} from '@angular/material/grid-list'; 
import { MatSidenavModule } from '@angular/material/sidenav';
import { ScriptDetailsComponent } from './script-details/script-details.component';
import {MatSelectModule} from '@angular/material/select'; 
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import { MatRippleModule } from '@angular/material/core';


@NgModule({
  declarations: [ScriptListComponent, ScriptDetailsComponent],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatGridListModule,
    MatSidenavModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    FormsModule,
    MatRippleModule
  ]
})
export class ScriptModule { }
