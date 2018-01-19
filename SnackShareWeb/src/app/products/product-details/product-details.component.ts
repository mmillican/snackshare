import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/products/product';
import { ProductStock } from '../../models/products/product-stock';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
  providers: [ProductService]
})
export class ProductDetailsComponent implements OnInit {
  private productId: number;
  model: Product;

  stockHistory: Array<ProductStock>;
  editStock: ProductStock = new ProductStock();

  successMessage: string;
  errorMessage: string; 
  isEditing: boolean = false;
  isProcessing: boolean = false;
  
  constructor(private route: ActivatedRoute,
    private productService: ProductService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.productId = parseInt(params.get('id'));

      this.refreshProduct(this.productId);
      this.refreshStockHistory(this.productId);
    });

    // Defaults
    this.editStock.purchaseDate = new Date().toString();
  }

  editProduct() {
    this.isEditing = true;
  }

  saveProduct() {
    this.isProcessing = true;

    this.productService.updateProduct(this.model).subscribe(result => {
      this.isProcessing = false;
      this.successMessage = 'Product has been updated';
      this.errorMessage = null;
      this.isEditing = false;
    }, error => {
      this.isProcessing = false;
      this.errorMessage = 'Error updating product.  Try again.';
      this.successMessage = null;
    });
  }

  restockProduct(): void {
    this.editStock.productId = this.productId;

    this.productService.restockProduct(this.editStock).subscribe(result => {
      this.refreshStockHistory(this.productId);
      this.successMessage = 'The product has been restocked';
      this.editStock = new ProductStock();
    });
  }

  private refreshProduct(productId: number): void {
    this.productService.getProduct(productId).subscribe(result => {
      this.model = result;
    }, error => {
      console.error(error);
    });
  }

  private refreshStockHistory(productId: number): void {
    this.productService.getProductStockHistory(productId).subscribe(result => {
      this.stockHistory = result;
    });
  }
}
