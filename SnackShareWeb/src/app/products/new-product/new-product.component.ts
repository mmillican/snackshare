import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/products/product';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-new-product',
  templateUrl: './new-product.component.html',
  styleUrls: ['./new-product.component.css'],
  providers: [ ProductService ]
})
export class NewProductComponent implements OnInit {
  model: Product = new Product();

  successMessage: string;
  errorMessage: string;
  processing: boolean = false;
  
  constructor(private productService: ProductService) { }

  ngOnInit() {

  }

  saveProduct(): void {
    this.processing = true;

    this.productService.createProduct(this.model).subscribe(result => {
      this.processing = false;
      this.successMessage = 'The product has been created';
      this.errorMessage = '';
    }, error => {
      this.processing = false;
      this.successMessage = '';
      this.errorMessage = 'There was an error creating the product.  Please try again.';
    });
  }

}
