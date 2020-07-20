/*
 * @Author: 林忠凯
 * @Date: 2020-05-20 16:44:56
 * @LastEditors: 林忠凯
 * @LastEditTime: 2020-06-04 14:28:57
 * @Description: file content
 */

/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Thursday April 23rd 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Thursday, 23rd April 2020 5:33:36 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、
 */
import moment from 'moment';

export function convertDate(dateRange: string[]) {
  if (dateRange && dateRange.length == 2) {
    const [s, e] = dateRange;
    const start = moment(s)
      .set('h', 0)
      .set('m', 0)
      .set('s', 0)
      .utc(true)
      .toDate();
    const end = moment(e)
      .set('h', 23)
      .set('m', 59)
      .set('s', 59)
      .utc(true)
      .toDate();
    return [start, end];
  }
  return undefined;
}

export function convertDateByMoment(dateRange: moment.Moment[]) {
  if (dateRange && dateRange.length == 2) {
    const [s, e] = dateRange;
    const start = s
      .set('h', 0)
      .set('m', 0)
      .set('s', 0)
      .utc(true)
      .toDate();

    const end = e
      .set('h', 23)
      .set('m', 59)
      .set('s', 59)
      .utc(true)
      .toDate();
    return [start, end];
  }
  return undefined;
}

/**
 * 是否默认时间
 * @param date 匹配日期
 */
export const isDefaultDate = (date: Date | string) => {
  if (typeof date === 'string') {
    date = new Date(date);
  }

  return (
    date.getFullYear() === 1900 &&
    date.getMonth() === 0 &&
    date.getDay() === 1 &&
    date.getHours() === 0 &&
    date.getMinutes() === 0 &&
    date.getSeconds() === 0 &&
    date.getMilliseconds() === 0
  );
};

/** 默认时间 */
export const DEFAULT_DATE = new Date(1900, 1, 1, 0, 0, 0, 0);
