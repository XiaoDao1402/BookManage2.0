import request from '@/utils/request';
import { UsersEntity } from '@/models/users';

const { post } = request;
// 查询用户
export async function queryUser(data?: TableListParams): Promise<RequestData<UsersEntity>> {
  const response: ApiModel<RequestData<UsersEntity>> = await post('/api/User/QueryUser', {
    data,
  });
  if (typeof response.value != undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}
