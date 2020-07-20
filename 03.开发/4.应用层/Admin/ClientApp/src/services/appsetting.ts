/*
 * @Author: 曾敏辉
 * @Date: 2020-07-07 17:20:46
 * @LastEditors: 曾敏辉
 * @LastEditTime: 2020-07-07 17:24:05
 * @Description: file content
 */

import { AppSettingEntity } from '@/models/appsetting';
import request from '@/utils/request';

const { post } = request;

export async function queryAppSetting(
  params?: TableListParams,
): Promise<RequestData<AppSettingEntity>> {
  const response: ApiModel<RequestData<AppSettingEntity>> = await request('/api/App/Query', {
    params,
  });
  if (typeof response.value !== undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}

export async function createAppSetting(
  entity: AppSettingEntity,
): ApiModelPromise<AppSettingEntity> {
  return post('/api/App/Create', { data: entity });
}

export async function modifyAppSetting(
  baseId: number,
  entity: AppSettingEntity,
): ApiModelPromise<AppSettingEntity> {
  return post(`/api/App/Modify/${baseId}`, { data: entity });
}

export async function deleteAppSetting(baseId: number[]): ApiModelPromise<AppSettingEntity> {
  return post(`/api/App/Delete`, { data: baseId });
}
