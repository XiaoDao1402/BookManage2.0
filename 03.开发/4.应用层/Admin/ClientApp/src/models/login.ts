/*
 * @Author: 何煜杰
 * @Date: 2020-07-06 17:33:26
 * @LastEditors: 何煜杰
 * @LastEditTime: 2020-07-06 18:54:54
 * @Description: file content
 */

import { Reducer } from 'redux';
import { Effect } from 'dva';
import { stringify } from 'querystring';
import { router } from 'umi';

import { fakeAccountLogin } from '@/services/login';
import { setAuthority, setToken, clearToken } from '@/utils/authority';
import { getPageQuery } from '@/utils/utils';
import { LoginModel } from './user';

export interface StateType {
  status?: 'ok' | 'error';
  type?: string;
  currentAuthority?: 'user' | 'guest' | 'admin';
}

export interface LoginModelType {
  namespace: string;
  state: LoginModel;
  effects: {
    login: Effect;
    logout: Effect;
  };
  reducers: {
    changeLoginStatus: Reducer<LoginModel>;
  };
}

const Model: LoginModelType = {
  namespace: 'login',

  state: {
    admin: undefined,
    token: undefined,
  },

  effects: {
    *login({ payload }, { call, put }) {
      const response: ApiModel<LoginModel> = yield call(fakeAccountLogin, payload);
      // Login successfully
      if (response.result && response.result.success) {
        yield put({
          type: 'changeLoginStatus',
          payload: response,
        });
        setToken(response.value.token!);
        yield put({
          type: 'user/saveToken',
          payload: response.value.token,
        });

        const urlParams = new URL(window.location.href);
        const params = getPageQuery();
        let { redirect } = params as { redirect: string };
        if (redirect) {
          const redirectUrlParams = new URL(redirect);
          if (redirectUrlParams.origin === urlParams.origin) {
            redirect = redirect.substr(urlParams.origin.length);
            if (redirect.match(/^\/.*#/)) {
              redirect = redirect.substr(redirect.indexOf('#') + 1);
            }
          } else {
            window.location.href = '/';
            return;
          }
        }
        router.replace(redirect || '/');
      } else {
        setAuthority([]);
      }
    },

    logout() {
      const { redirect } = getPageQuery();
      clearToken();
      if (window.location.pathname !== '/user/login' && !redirect) {
        router.replace({
          pathname: '/user/login',
          search: stringify({
            redirect: window.location.href,
          }),
        });
      }
    },
  },

  reducers: {
    changeLoginStatus(state, { payload }) {
      return {
        ...state,
        admin: payload.value.admin,
        token: payload.value.token,
        status: payload.result.status,
      };
    },
  },
};

export default Model;
