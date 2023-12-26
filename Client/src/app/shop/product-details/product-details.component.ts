import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;
  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    let productId: number = +this.activatedRoute.snapshot.paramMap.get('id')!;
    this.shopService.getProduct(productId).subscribe({
      next: (product) => (this.product = product),
      error: (error) => console.log(error),
    });
  }
}
