import request from '@/utils/request';
import { UserBookEntity } from '@/models/userbook';

const { post } = request;

// 查询用户借书记录
export async function queryUserBook(data?: TableListParams): Promise<RequestData<UserBookEntity>> {
  const response = await post('/api/UserBook/QueryUserBook', { data });
  if (typeof response.value != undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}
