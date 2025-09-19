import { Component } from '@angular/core';
import { OnInit, inject } from '@angular/core'; // أضف inject
import { HttpClient } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { Product } from '../../chared/models/product';
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
@Component({
  selector: 'app-shop',
  imports: [
    MatCardModule,
    ProductItemComponent,
    MatButton,
    MatIcon
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  private shopService=inject(ShopService);
  private dialogService = inject(MatDialog);
  products: Product[]=[];
  selectedBrands:string[]=[];
  selectedTypes:string[]=[];


  ngOnInit(): void {
    this.initializeShop();
    
  }
  initializeShop(){
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.shopService.getProducts().subscribe({
      next:Response=>this.products=Response.data,
      error:error=>console.log(error),
    })
  }
  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.selectedBrands,
        selectedTypes: this.selectedTypes
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        console.log(result);
        this.selectedBrands = result.selectedBrands;
        this.selectedTypes = result.selectedTypes;
      }
    });
  }
}