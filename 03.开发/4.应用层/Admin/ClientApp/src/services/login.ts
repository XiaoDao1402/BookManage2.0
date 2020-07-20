/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday February 26th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Friday, 6th March 2020 10:18:29 am
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、安全服务
 */
import request from '@/utils/request';
import { LoginModel } from '@/models/user';

export interface LoginParamsType {
  userName: string;
  password: string;
  mobile: string;
  captcha: string;
}

export async function fakeAccountLogin(params: LoginParamsType): Promise<ApiModel<LoginModel>> {
  return request(`/api/Security/Login?account=${params.userName}&password=${params.password}`);
}

// export async function getCurrentAuthority(): ApiModelPromise<string[]> {
//   return request('/api/Security/CurrentAuthorize');
// }

export async function fakeChangePassword(
  oldPassword: string,
  password: string,
): ApiModelPromise<string> {
  return request(`/api/Security/ChangePassword`, { params: { oldPassword, password } });
}
