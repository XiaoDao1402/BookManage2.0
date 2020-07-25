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
