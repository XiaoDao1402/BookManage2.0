/*
 * @Author: 曾敏辉
 * @Date: 2020-07-07 14:28:51
 * @LastEditors: 曾敏辉
 * @LastEditTime: 2020-07-07 17:25:57
 * @Description: file content
 */

import { AdminEntity } from '@/models/admin';
import request from '@/utils/request';

const { post } = request;

export async function queryAdmins(params?: TableListParams): Promise<RequestData<AdminEntity>> {
  const response: ApiModel<RequestData<AdminEntity>> = await request('/api/Admin/Query', {
    params,
  });
  if (typeof response.value !== undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}

export async function createAdmin(entity: AdminEntity): ApiModelPromise<AdminEntity> {
  return post('/api/Admin/Create', { data: entity });
}
//管理员修改
export async function moodifyAdmin(
  adminId?: number,
  entity?: AdminEntity,
): ApiModelPromise<AdminEntity> {
  return post(`/api/Admin/Modify/${adminId}`, { data: entity });
}
//修改个人信息
export async function moodifyAdminInfo(
  adminId?: number,
  entity?: AdminEntity,
): ApiModelPromise<AdminEntity> {
  return post(`/api/Admin/ModifyInfo/${adminId}`, { data: entity });
}

/**
 * 删除管理员
 * @param adminIds
 */
export async function deleteAdmin(adminIds: number[]): ApiModelPromise<AdminEntity> {
  return post('/api/Admin/Delete', { data: adminIds });
}

/**
 * 重置密码
 * @param adminId
 */
export async function resetAdmin(adminId: number): ApiModelPromise<AdminEntity> {
  return request(`/api/Admin/Reset/${adminId}`);
}

export async function bindUser(adminId?: number, userId?: number): ApiModelPromise<AdminEntity> {
  return request(`/api/Admin/Bind/${adminId}`, { params: { userId } });
}
