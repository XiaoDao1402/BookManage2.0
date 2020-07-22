import { BookCategoryEntity } from '@/models/category';
import request from '@/utils/request';

export async function queryCategory(
  params?: TableListParams,
): Promise<RequestData<BookCategoryEntity>> {
  const response: ApiModel<RequestData<
    BookCategoryEntity
  >> = await request('/api/Category/QueryCategory', { params });
  if (typeof response.value != undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}
