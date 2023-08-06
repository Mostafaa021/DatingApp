export interface Pagination {
    currentPage:number ;
    itemsPerPage : number;
    totalItems : number;
    totalPages : number;
}

export class PaginationResult<T>
{
    results? : T; 
    pagination? : Pagination
}
