/*
 * @Author: 林忠凯
 * @Date: 2020-06-10 18:15:18
 * @LastEditors: 曾敏辉
 * @LastEditTime: 2020-07-07 17:26:15
 * @Description: file content
 */
import request from '@/utils/request';
import { AdminEntity } from '@/models/admin';
import { UserEntity } from '@/models/user';

export async function query(params?: TableListParams): Promise<RequestData<UserEntity>> {
  const response: ApiModel<RequestData<UserEntity>> = await request('/api/User/Query', { params });
  if (typeof response.value !== undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}

export async function GetUsers(): Promise<ApiModel<UserEntity[]>> {
  return request('/api/User/GetUsers');
}

export async function queryCurrent(): Promise<ApiModel<AdminEntity>> {
  return request('/api/Security/CurrentUser');
}

export async function queryNotices(): Promise<any> {
  return request('/api/notices');
}
//根据类型Id查询
export async function queryUserByUserId(userId?: number): ApiModelPromise<UserEntity> {
  return request(`/api/User/GetUserById/${userId}`, {
    params: { userId },
  });
}
