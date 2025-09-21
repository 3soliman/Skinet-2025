// src/app/features/shop/shop.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { Product } from '../../chared/models/product';
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { ShopParams } from '../../chared/models/shopParams';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../chared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  imports: [
    CommonModule,
    MatCardModule,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenuModule,
    MatSelectionList,
    MatListOption,
    MatPaginatorModule,
    FormsModule,
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  
  products?: Pagination<Product>;

  // خيارات الترتيب
  sortOptions = [
    { value: 'name', label: 'Name: A to Z' },
    { value: 'price', label: 'Price: Low to High' },
    { value: 'pricedesc', label: 'Price: High to Low' }
  ];
  shopParams=new ShopParams();
  totalItems = 0;
  pageSizeOptions=[5,10,15,20];

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    
    this.shopService.getProducts(
      this.shopParams
    ).subscribe({
      next: (response: any) => {
        this.products = response;
        this.totalItems=response.totalItems;
      },
      error: (error: any) => {
        console.error('Error loading products:', error);
      }
    });
  }


  // دالة محسنة لتطبيق الفلاتر
 

  openFiltersDialog(): void {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      width: '400px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.shopParams.brands = result.selectedBrands;
        this.shopParams.types = result.selectedTypes;
        this.shopParams.pageIndex=1;
        this.loadProducts(); // إعادة تحميل مع الفلاتر الجديدة
      }
    });
  }
  onSearchChange(event: any) {
    this.shopParams.pageIndex=1;
    this.loadProducts();
  }
  handlePageEvent(event: PageEvent) {
    this.shopParams.pageIndex = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.loadProducts();
  }
 // دالة جديدة للترتيب
onSortChange(event: MatSelectionListChange) {
  const selectedOption = event.options[0];
  if (selectedOption && selectedOption.selected) {
    this.shopParams.sort = selectedOption.value;
    this.shopParams.pageIndex=1;
    console.log('Selected sort:', this.shopParams.sort);
    this.loadProducts(); // إعادة تحميل المنتجات
  }
}

  // مسح كل الفلاتر
  clearFilters(): void {
    this.shopParams.brands = [];
    this.shopParams.types = [];
    this.shopParams.sort = 'name';
    this.loadProducts();
  }
}