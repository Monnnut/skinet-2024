import { Component, Input, input } from '@angular/core';
import { Product } from '../../../shared/models/product';
import { MatCardModule } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-product-item',
  imports: [MatCardModule, CurrencyPipe, MatIconModule, MatButtonModule, RouterLink],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
product = input<Product>(); //signal makes it a function
}
