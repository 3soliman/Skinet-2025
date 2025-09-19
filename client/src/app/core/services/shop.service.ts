import { Injectable, inject } from '@angular/core'; // أضف inject
import { HttpClient } from '@angular/common/http';
import { Pagination } from '../../chared/models/pagination';
import { Product } from '../../chared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl= 'http://localhost:5000/api/'
  private http=inject(HttpClient);
  types: string[]=[];
  brands:string[]=[];

  getProducts(){
     return this.http.get<Pagination<Product>>(this.baseUrl + 'products?')
  }

  getBrands(){
    if(this.brands.length>0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: Response=> this.brands=Response
    })
 }
 getTypes(){
  if(this.types.length>0) return;
  return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
    next: Response=> this.types=Response
  })
}
}
