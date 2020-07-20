import { query, queryCurrent } from '@/services/user';
import { Effect } from 'dva';
import { Reducer } from 'redux';
import { AdminEntity } from './admin';
import { RequestData } from '@ant-design/pro-table';

export interface WxUserEntity {
  wxUserId?: number;
  userId?: number;
  openId?: string;
  nickName?: string;
  gender?: string;
  city?: string;
  province?: string;
  country?: string;
  avatarUrl?: string;
  unionId?: string;
  createTime?: Date;
  modifyTime?: Date;
}

export interface UserAccountEntity {
  userAccountId?: number;
  account?: string;
  password?: string;
  createTime?: Date;
  modifyTime?: Date;
}

export interface UserEntity {
  userId?: string;
  name?: string;
  portrait?: string;
  signature?: string;
  birthDay: Date;
  gender?: number;
  city?: string;
  lng?: string;
  lat?: string;
  phone?: string;
  totalPoints?: number;
  surplusPoints?: number;
  isWechat?: boolean;
  isAccount?: boolean;
  wxUser?: WxUserEntity;
  userAccount?: UserAccountEntity;
  createTime?: Date;
  modifyTime?: Date;
}

export interface LoginModel {
  admin?: AdminEntity;
  token?: string;
  status?: 'Error' | 'OK';
}

export interface UserModelState {
  currentUser?: LoginModel;
  page?: RequestData<UserEntity>;
}

export interface UserModelType {
  namespace: 'user';
  state: UserModelState;
  effects: {
    fetch: Effect;
    fetchCurrent: Effect;
  };
  reducers: {
    save: Reducer<UserModelState>;
    saveCurrentUser: Reducer<UserModelState>;
    changeNotifyCount: Reducer<UserModelState>;
    saveToken: Reducer<UserModelState>;
  };
}

const UserModel: UserModelType = {
  namespace: 'user',

  state: {
    currentUser: {},
    page: { data: [] },
  },

  effects: {
    *fetch(_, { call, put }) {
      const response = yield call(query);
      yield put({
        type: 'save',
        payload: response,
      });
    },
    *fetchCurrent(_, { call, put }) {
      const response: ApiModel<AdminEntity> = yield call(queryCurrent);

      yield put({
        type: 'saveCurrentUser',
        payload: response.value,
      });
    },
  },

  reducers: {
    save(state, action) {
      return {
        ...state,
        page: {
          ...action.payload,
        },
      };
    },
    saveCurrentUser(state, action) {
      return {
        ...state,
        currentUser: {
          ...state?.currentUser,
          admin: action.payload,
        },
      };
    },
    changeNotifyCount(
      state = {
        currentUser: {},
      },
      action,
    ) {
      return {
        ...state,
        currentUser: {
          ...state.currentUser,
        },
      };
    },
    saveToken(state, action) {
      return {
        ...state,
        currentUser: {
          ...state?.currentUser,
          token: action.payload,
        },
      };
    },
  },
};

export default UserModel;
