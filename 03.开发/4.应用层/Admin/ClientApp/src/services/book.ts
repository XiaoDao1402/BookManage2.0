import { BookEntity } from '@/models/book';
import request from '@/utils/request';

const { post } = request;

// 查询图书
export async function queryBook(params?: TableListParams): Promise<RequestData<BookEntity>> {
  return post('/api/Book/QueryBook');
}
