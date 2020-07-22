import { BookCategoryEntity } from '@/models/category';
import request from '@/utils/request';

const { post } = request;

// 查询图书分类
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

// 批量删除图书分类
export async function deleteCategory(
  bookCategoryIds: number[],
): ApiModelPromise<BookCategoryEntity> {
  return post('/api/Category/DeleteCategory', { data: bookCategoryIds });
}
