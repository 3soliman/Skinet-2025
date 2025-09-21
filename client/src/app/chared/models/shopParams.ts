export class ShopParams {
    pageIndex: number = 1;
    pageSize: number = 10;
    brands?: string[];
    types?: string[];
    sort='name';
    search='';
  
  }