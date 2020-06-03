import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { IAppConfig } from './app.config.model';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AppConfig {

    static settings: IAppConfig;
    jsonFile: string;

    constructor(private http: HttpClient) {}

    load() {
        if (environment.production) {
            this.jsonFile = 'assets/config/app.config.prod.json';
        } else {
            this.jsonFile = 'assets/config/app.config.json';
        }
        return new Promise<void>((resolve, reject) => {
            this.http.get(this.jsonFile).toPromise().then((response: IAppConfig) => {
                AppConfig.settings = response;
               resolve();
            }).catch((response: any) => {
               reject(`Could not load file '${this.jsonFile}': ${JSON.stringify(response)}`);
            });
        });
    }
}
