export type Response<DataType> = {
  success: boolean;
  hasErrors: boolean;
  errors?: string[];
  data?: DataType;
};
