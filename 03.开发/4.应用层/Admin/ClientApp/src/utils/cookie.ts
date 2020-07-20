/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday March 4th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Wednesday, 4th March 2020 9:56:19 am
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、cookie 工具类
 */
import cookies from 'react-cookies';

/**
 * 设置cookie
 * @param key 键
 * @param value 值
 * @param expires 有效期
 */
export function setCookie(key: string, value: string | number | object, expires?: Date) {
  if (typeof value === 'object') {
    value = JSON.stringify(value);
  }
  cookies.save(key, value, { expires, path: '/' });
}

/**
 * 获取cookie
 * @param key 键
 * @type T 值类型
 */
export function getCookie<T extends object>(key: string): T | undefined {
  const value = cookies.load(key);
  if (typeof value === 'undefined') {
    return undefined;
  }
  try {
    const obj: T = JSON.parse(value);
    return obj;
  } catch {
    return value;
  }
}

/**
 * 清除cookie
 * @param key 键
 */
export function clearCookie(key: string) {
  cookies.remove(key);
}
