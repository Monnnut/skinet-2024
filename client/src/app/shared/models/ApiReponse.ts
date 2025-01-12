export interface ApiResponse<T> {
    value: T;
    formatters: any[];
    contentTypes: any[];
    declaredType: string | null;
    statusCode: number;
  }