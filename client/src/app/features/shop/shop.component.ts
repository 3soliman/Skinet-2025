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
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  
  products: Product[] = [];
  selectedBrands: string[] = [];
  selectedTypes: string[] = [];
  selectedSort = 'name';
  isLoading = false;

  // خيارات الترتيب
  sortOptions = [
    { value: 'name', label: 'Name: A to Z' },
    { value: 'price', label: 'Price: Low to High' },
    { value: 'pricedesc', label: 'Price: High to Low' }
  ];

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoading = true;
    
    this.shopService.getProducts(
      this.selectedBrands, 
      this.selectedTypes, 
      this.selectedSort
    ).subscribe({
      next: (response: any) => {
        this.products = response.data;
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Error loading products:', error);
        this.isLoading = false;
      }
    });
  }


  // دالة محسنة لتطبيق الفلاتر
 

  openFiltersDialog(): void {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      width: '400px',
      data: {
        selectedBrands: this.selectedBrands,
        selectedTypes: this.selectedTypes
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.selectedBrands = result.selectedBrands;
        this.selectedTypes = result.selectedTypes;
        this.loadProducts(); // إعادة تحميل مع الفلاتر الجديدة
      }
    });
  }

 // دالة جديدة للترتيب
onSortChange(event: MatSelectionListChange) {
  const selectedOption = event.options[0];
  if (selectedOption && selectedOption.selected) {
    this.selectedSort = selectedOption.value;
    console.log('Selected sort:', this.selectedSort);
    this.loadProducts(); // إعادة تحميل المنتجات
  }
}

  // مسح كل الفلاتر
  clearFilters(): void {
    this.selectedBrands = [];
    this.selectedTypes = [];
    this.selectedSort = 'name';
    this.loadProducts();
  }
}