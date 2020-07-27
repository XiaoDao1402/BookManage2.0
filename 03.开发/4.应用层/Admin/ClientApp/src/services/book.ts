import { BookEntity } from '@/models/book';
import request from '@/utils/request';

const { post } = request;

// 查询图书
export async function queryBook(data?: TableListParams): Promise<RequestData<BookEntity>> {
  const response = await post('/api/Book/QueryBook', { data });
  if (typeof response.value != undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}

// 批量删除图书
export async function deleteBook(bookIds: number[]): ApiModelPromise<BookEntity> {
  return post('/api/Book/DeleteBook', { data: bookIds });
}

// 新增图书
export async function addBook(entity: BookEntity): ApiModelPromise<BookEntity> {
  return post('/api/Book/AddBook', { data: entity });
}

// 修改图书
export async function updateBook(bookId: number, entity: BookEntity): ApiModelPromise<BookEntity> {
  return post(`/api/Book/UpdateBook/${bookId}`, { data: entity });
}
