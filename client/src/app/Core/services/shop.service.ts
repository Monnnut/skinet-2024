import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Product } from '../../shared/models/product';
import { Pagination } from '../../shared/models/pagination';
import { ApiResponse } from '../../shared/models/ApiReponse';
import { ShopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root', //provide automatically to the entire application
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  types: string[] = []; //store data so it dont get destroyed
  brands: string[] = [];

  getProducts(shopParams:ShopParams) {
  //step 7(filter):get new brands and types from filter and append from http params
    let params = new HttpParams();

    if (shopParams.brands.length > 0){
      params = params.append('brands', shopParams.brands.join(','));
    }
    if (shopParams.types.length > 0){
      params = params.append('types', shopParams.types.join(','));
    }
    if(shopParams.sort) {
      params = params.append('sort', shopParams.sort)
    }
    if(shopParams.search){
      params = params.append('search', shopParams.search)
    }

    params = params.append('pageSize',shopParams.pageSize);
    params = params.append('pageIndex',shopParams.pageNumber);


      return this.http.get<ApiResponse<Pagination<Product>>>(
        this.baseUrl + 'products', {params}
      ); //need to return this
  }

  getBrands() {
    if (this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: (response) => (this.brands = response),
    });
  }

  getTypes() {
    if (this.types.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: (response) => (this.types = response),
    });
  }
}