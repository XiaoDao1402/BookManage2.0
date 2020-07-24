import { BookCategoryEntity } from '@/models/bookcategory';
import request from '@/utils/request';

const { post } = request;

// 查询所有图书分类
export async function queryBookCategory(
  params?: TableListParams,
): Promise<RequestData<BookCategoryEntity>> {
  const response: ApiModel<RequestData<
    BookCategoryEntity
  >> = await request('/api/BookCategory/QueryBookCategory', { params });
  if (typeof response.value != undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}

// 查询所有图书分类（用于树形下拉列表）
export async function queryBookCategoryTree() {
  return request('/api/BookCategory/QueryBookCategoryTree');
}

// 批量删除图书分类
export async function deleteBookCategory(
  bookCategoryIds: number[],
): ApiModelPromise<BookCategoryEntity> {
  return post('/api/BookCategory/DeleteBookCategory', { data: bookCategoryIds });
}

// 新增图书分类
export async function addBookCategory(
  entity: BookCategoryEntity,
): ApiModelPromise<BookCategoryEntity> {
  return post('/api/BookCategory/AddBookCategory', { data: entity });
}

// 修改图书分类
export async function updateBookCategory(
  bookCategoryId: number,
  entity: BookCategoryEntity,
): ApiModelPromise<BookCategoryEntity> {
  return post(`/api/BookCategory/UpdateBookCategory/${bookCategoryId}`, { data: entity });
}
