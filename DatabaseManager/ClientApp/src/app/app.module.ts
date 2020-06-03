import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {MatCardModule} from '@angular/material/card'; 
import { EnvironmentModule } from './environment/environment.module';
import { HttpClientModule } from '@angular/common/http';
import { MIGRATION_API_BASE_URL } from './migration-api/migration-api.module';
import {MatSidenavModule} from '@angular/material/sidenav'; 
import {MatButtonModule} from '@angular/material/button';
import {MatTabsModule} from '@angular/material/tabs'; 
import { ScriptModule } from './script/script.module';
import { AppConfig } from 'src/config/app.config';
import { SharedModule } from './shared/shared.module';
import { DocumentationModule } from './documentation/documentation.module';

export function initializeApp(appConfig: AppConfig): () => Promise<void> {
  return () => appConfig.load();
}

export function initializeBasePath(): string{
  return AppConfig.settings.SERVER_API.END_POINT;
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatCardModule,
    EnvironmentModule,
    ScriptModule,
    MatSidenavModule,
    MatButtonModule,
    MatTabsModule,
    DocumentationModule
  ],
  providers: [
    AppConfig,
    { provide: APP_INITIALIZER, useFactory: initializeApp, deps: [AppConfig], multi: true },
    { provide: MIGRATION_API_BASE_URL, useFactory: initializeBasePath }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
