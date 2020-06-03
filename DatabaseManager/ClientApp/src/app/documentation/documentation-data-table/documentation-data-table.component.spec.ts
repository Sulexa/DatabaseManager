import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentationDataTableComponent } from './documentation-data-table.component';

describe('DocumentationDataTableComponent', () => {
  let component: DocumentationDataTableComponent;
  let fixture: ComponentFixture<DocumentationDataTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentationDataTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentationDataTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
