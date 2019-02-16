import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { FileShareStorageComponent } from './file-share-storage/file-share-storage.component';

@NgModule({
  declarations: [
    AppComponent,
    FileShareStorageComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [FileShareStorageComponent]
})
export class AppModule { }
