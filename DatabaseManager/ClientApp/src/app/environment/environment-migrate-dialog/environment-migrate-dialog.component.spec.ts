import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EnvironmentMigrateDialogComponent } from './environment-migrate-dialog.component';

describe('EnvironmentMigrateDialogComponent', () => {
  let component: EnvironmentMigrateDialogComponent;
  let fixture: ComponentFixture<EnvironmentMigrateDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EnvironmentMigrateDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EnvironmentMigrateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
