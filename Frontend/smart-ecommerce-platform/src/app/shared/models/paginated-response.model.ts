export interface PaginatedResponse<T> {
  data: T[];
  totalItems: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}
