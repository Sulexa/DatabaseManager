import { DatabaseEnvironment } from '../migration-api/migration-api.module';
import { Observable } from 'rxjs';

export interface IScript{
  name: string;
  description: string;
  environments: DatabaseEnvironment[];
  api: (databaseEnvironment: DatabaseEnvironment) => Observable<void>
}