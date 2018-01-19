import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/products/product';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
  providers: [ ProductService ]
})
export class ProductListComponent implements OnInit {
  products: Array<Product> = new Array<Product>();
  
  constructor(private productService: ProductService) { }

  ngOnInit() {
    this.refreshProducts();
  }

  private refreshProducts(): void {
    this.productService.getProducts().subscribe(response => {
      this.products = response;
    });
  }

}
