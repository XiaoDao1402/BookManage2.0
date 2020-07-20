/*
 * @Author: 曾敏辉
 * @Date: 2020-07-08 15:02:21
 * @LastEditors: 林忠凯
 * @LastEditTime: 2020-07-09 19:28:54
 * @Description: file content
 */

import { Effect } from 'dva';
import { Reducer } from 'redux';
import { AdminEntity } from './admin';
import { GoodsEntity } from './goods';
import { GoodsSeriesEntity } from './series';

export interface GoodsSeriesExtendsEntity {
  commoditySeriesExtendsId?: number;
  commoditySeriesId?: number;
  commodityId?: number | { value?: number | string; label?: string; goods?: GoodsEntity };
  adminId?: number;
  createDate?: Date;
  modifyDate?: Date;
  admin?: AdminEntity;
  commodity?: GoodsEntity;
  commoditySeries?: GoodsSeriesEntity;
}
export interface GoodsSeriesExtendsModelState {
  page: RequestData<GoodsSeriesExtendsEntity>;
}
export interface GoodsSeriesExtendsModelType {
  namespace: 'goodsSeriesExtends';
  state: GoodsSeriesExtendsModelState;
  effects: {
    fetch: Effect;
  };
  reducers: {
    query: Reducer<GoodsSeriesExtendsModelState>;
  };
}

const GoodsSeriesExtendsModel: GoodsSeriesExtendsModelType = {
  namespace: 'goodsSeriesExtends',
  state: {
    page: { data: [] },
  },
  effects: {
    *fetch({ payload }, { call, put }) {
      const respose: ApiModel<RequestData<GoodsSeriesExtendsEntity>> = yield call('', payload);
      if (respose.result.success) {
        yield put({
          type: 'query',
          payload: respose.value,
        });
      }
    },
  },
  reducers: {
    query(state, { payload }) {
      return {
        ...state!,
        page: payload,
      };
    },
  },
};

export default GoodsSeriesExtendsModel;
