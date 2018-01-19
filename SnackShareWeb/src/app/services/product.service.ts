import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';

import { BaseService } from './base.service';
import { OAuthService } from 'angular-oauth2-oidc';
import { AppConfig } from '../appConfg';
import { Product } from '../models/products/product';
import { ProductStock } from '../models/products/product-stock';

@Injectable()
export class ProductService extends BaseService {

  constructor(private oAuthService: OAuthService,
    private http: Http,
    private appConfig: AppConfig) { 
    
    super(oAuthService)
  }

  getProducts(): Observable<Array<Product>> {
    let reqOptions = this.getHttpRequestOptions();
    let url = this.appConfig.apiBaseUrl + 'products';

    return this.http.get(url, reqOptions)
      .map((response: Response) => response.json() as Product[]);
  }

  getProduct(id: number): Observable<Product> {
    let reqOptions = this.getHttpRequestOptions();
    let url = this.appConfig.apiBaseUrl + 'products/' + id;

    return this.http.get(url, reqOptions)
      .map((response: Response) => response.json() as Product);
  }

  createProduct(product: Product): Observable<Product> {
    let reqOptions = this.getHttpRequestOptions();
    let url = this.appConfig.apiBaseUrl + 'products';

    let body = JSON.stringify(product).toString();

    return this.http.post(url, body, reqOptions)
      .map((response: Response) => response.json() as Product);
  }

  updateProduct(product: Product): Observable<Product> {
    let reqOptions = this.getHttpRequestOptions();
    let url = this.appConfig.apiBaseUrl + 'products/' + product.id;

    let body = JSON.stringify(product).toString();

    return this.http.put(url, body, reqOptions)
      .map((response) => response.json() as Product);
  }

  getProductStockHistory(productId: number): Observable<Array<ProductStock>> {
    let reqOptions = this.getHttpRequestOptions();
    let url = this.appConfig.apiBaseUrl + 'products/' + productId + '/stock';

    return this.http.get(url, reqOptions)
      .map((response: Response) => response.json() as ProductStock[]);
  }

  restockProduct(stock: ProductStock): Observable<ProductStock> {
    let reqOptions = this.getHttpRequestOptions();
    let url = this.appConfig.apiBaseUrl + 'products/' + stock.productId + '/stock';

    let body = JSON.stringify(stock).toString();

    return this.http.post(url, body, reqOptions)
      .map((response: Response) => response.json() as ProductStock);
  }
}
