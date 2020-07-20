/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday March 4th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Wednesday, 4th March 2020 4:57:31 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、管理员
 */
import { queryAdmins } from '@/services/admin';
import { Effect } from 'dva';
import { Reducer } from 'redux';

export interface AdminEntity {
  name?: string;
  adminId?: number;
  account?: string;
  isAdministrator?: boolean;
  createTime?: Date;
  modifyTime?: Date;
}

export interface AdminModelState {
  page: RequestData<AdminEntity>;
}

export interface AdminModelType {
  namespace: 'admin';
  state: AdminModelState;
  effects: {
    fetch: Effect;
  };
  reducers: {
    query: Reducer<AdminModelState>;
  };
}

const AdminModel: AdminModelType = {
  namespace: 'admin',
  state: {
    page: { data: [] },
  },
  effects: {
    *fetch({ payload }, { call, put }) {
      const respose: ApiModel<RequestData<AdminEntity>> = yield call(queryAdmins, payload);
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

export default AdminModel;
