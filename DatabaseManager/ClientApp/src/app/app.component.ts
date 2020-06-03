import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'migrations-front';
  navLinks: INavLink[] = [
    {
      label: "Migrations",
      path: "migrations"
    },
    {
      label: "Scripts",
      path: "scripts"
    },
    {
      label: "Documentation",
      path: "documentation"
    }
  ];
}

interface INavLink{
  path: string;
  label: string;
}
