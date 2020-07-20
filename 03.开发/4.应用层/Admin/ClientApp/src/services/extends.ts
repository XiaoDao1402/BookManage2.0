/*
 * @Author: 曾敏辉
 * @Date: 2020-07-08 15:08:25
 * @LastEditors: 林忠凯
 * @LastEditTime: 2020-07-09 18:47:57
 * @Description: file content
 */

import request from '@/utils/request';
import { GoodsSeriesExtendsEntity } from '@/models/extends';

const { post } = request;

/**
 * 查询全部分类
 */
export async function queryGoodsSeriesExtends(
  params?: TableListParams,
): Promise<RequestData<GoodsSeriesExtendsEntity>> {
  const response: ApiModel<RequestData<GoodsSeriesExtendsEntity>> = await request(
    '/api/GoodsSeriesExtends/Search',
    {
      params,
    },
  );
  if (typeof response.value !== undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}

/**
 * 新建
 * @param entity
 */
export async function createSeriesExtends(
  entity: GoodsSeriesExtendsEntity,
): ApiModelPromise<GoodsSeriesExtendsEntity> {
  return post('/api/GoodsSeriesExtends/CreateSeriesExtends', { data: entity });
}

/**
 * 修改
 * @param seriesExtendsId
 * @param entity
 */
export async function modifySeriesExtends(
  entity: GoodsSeriesExtendsEntity,
): ApiModelPromise<GoodsSeriesExtendsEntity> {
  return post(`/api/GoodsSeriesExtends/ModifySeriesExtends`, { data: entity });
}

/**
 * 删除商品系列
 * @param seriesExtendsIds
 */
export async function deleteGoodsSeriesExtends(
  seriesExtendsId: number,
): ApiModelPromise<GoodsSeriesExtendsEntity> {
  return request('/api/GoodsSeriesExtends/DeleteSeriesExtends', { params: { seriesExtendsId } });
}
