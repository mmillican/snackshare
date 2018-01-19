import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { OAuthModule } from 'angular-oauth2-oidc';

import { AppComponent } from './app.component';
import { HomeComponent } from "./home/home.component";
import { AppConfig } from './appConfg';
import { ProductListComponent } from './products/product-list/product-list.component';
import { NewProductComponent } from './products/new-product/new-product.component';
import { ProductDetailsComponent } from './products/product-details/product-details.component';

const appRoutes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'products/new', component: NewProductComponent },
  { path: 'products/:id', component: ProductDetailsComponent },
  { path: '**', redirectTo: 'home' }
]

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProductListComponent,
    NewProductComponent,
    ProductDetailsComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    HttpModule,
    HttpClientModule,
    FormsModule,
    OAuthModule.forRoot(),
    RouterModule.forRoot(
      appRoutes, 
      // { enableTracing: true }
    )
  ],
  providers: [
    AppConfig
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor() {
    
  }
  


}
