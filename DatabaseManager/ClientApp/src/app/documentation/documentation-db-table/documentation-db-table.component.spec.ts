import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentationDbTableComponent } from './documentation-db-table.component';

describe('DocumentationDbTableComponent', () => {
  let component: DocumentationDbTableComponent;
  let fixture: ComponentFixture<DocumentationDbTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentationDbTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentationDbTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
