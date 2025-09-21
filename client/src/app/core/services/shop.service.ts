import { Injectable, inject } from '@angular/core'; // أضف inject
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination } from '../../chared/models/pagination';
import { Product } from '../../chared/models/product';
import { Observable } from 'rxjs';
import { ShopParams } from '../../chared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl= 'http://localhost:5000/api/'
  private http=inject(HttpClient);
  types: string[]=[];
  brands:string[]=[];
  getProducts(shopParams: ShopParams): Observable<Pagination<Product>> {
    let params = new HttpParams();
    
    // إضافة جميع معاملات ShopParams إلى query parameters
    params = params.append('pageIndex', shopParams.pageIndex.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());
    
    if (shopParams.brands && shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }
    
    if (shopParams.types && shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }
    
    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }
    if(shopParams.search){
      params=params.append('search',shopParams.search);
    }
    
  
    return this.http.get<Pagination<Product>>(`${this.baseUrl}products`, { params });
    
  }
 
// src/app/core/services/shop.service.ts
getBrands(): Observable<string[]> {
  return this.http.get<string[]>(`${this.baseUrl}products/brands`);
}

getTypes(): Observable<string[]> {
  return this.http.get<string[]>(`${this.baseUrl}products/types`);
}

}
