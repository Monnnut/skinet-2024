import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../Core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatCardModule } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import {
  MatListModule,
  MatSelectionList,
  MatSelectionListChange,
} from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  imports: [
    MatCardModule,
    ProductItemComponent,
    MatButtonModule,
    MatIcon,
    MatMenuModule,
    MatListModule,
    MatPaginator,
    FormsModule
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  products?: Pagination<Product>;

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price:Low-High', value: 'priceAsc' },
    { name: 'Price:High-Low', value: 'priceDesc' },
  ];

  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 15, 20];
  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  }

  getProducts() {
    //step 8 (filter): update product according to filters
    this.shopService.getProducts(this.shopParams).subscribe({
      //my data is store in value.data
      next: (response) => (this.products = response.value),
      error: (error) => console.log(error),
    });
  }

  onSearchChange(){
    this.shopParams.pageNumber =1;
    this.getProducts();
  }

  handlePageEvent(event:PageEvent){
    this.shopParams.pageNumber = event.pageIndex +1;
    this.shopParams.pageSize = event.pageSize
    this.getProducts();
  }

  onSortChange(event: MatSelectionListChange) {
    //change selectedSort base on click event
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber =1;
      this.getProducts();
    }
  }

  openFilterDialog() {
    //step 1(filter): open filter dialog here
    //step 2(filter):pass the data to selectedBrands/selected types
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types,
      },
    });
    //step 6(filter): recieve data and send to shop service to change the data
    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber =1;
          this.getProducts();
        }
      },
    });
  }
}
