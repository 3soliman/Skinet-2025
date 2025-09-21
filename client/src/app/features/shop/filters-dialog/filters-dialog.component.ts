// src/app/features/shop/filters-dialog/filters-dialog.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ShopService } from '../../../core/services/shop.service';

@Component({
  selector: 'app-filters-dialog',
  imports: [
    CommonModule,
    FormsModule,
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  brands: string[] = []; // سيتم تحميلها من الـ API
  types: string[] = [];  // سيتم تحميلها من الـ API
  selectedBrands: string[] = this.data.selectedBrands || [];
  selectedTypes: string[] = this.data.selectedTypes || [];

  ngOnInit(): void {
    this.loadBrandsAndTypes();
  }

  loadBrandsAndTypes(): void {
    // تحميل الـ brands
    this.shopService.getBrands().subscribe({
      next: (brands: string[]) => {
        this.brands = brands;
      },
      error: (error: any) => {
        console.error('Error loading brands:', error);
      }
    });

    // تحميل الـ types
    this.shopService.getTypes().subscribe({
      next: (types: string[]) => {
        this.types = types;
      },
      error: (error: any) => {
        console.error('Error loading types:', error);
      }
    });
  }

  applyFilters(): void {
    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedTypes: this.selectedTypes
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }
}