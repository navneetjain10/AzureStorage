import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileShareStorageComponent } from './file-share-storage.component';

describe('FileShareStorageComponent', () => {
  let component: FileShareStorageComponent;
  let fixture: ComponentFixture<FileShareStorageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileShareStorageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileShareStorageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
