import { AnyAction } from 'redux';
import { MenuDataItem } from '@ant-design/pro-layout';
import { RouterTypes } from 'umi';
import { GlobalModelState } from './global';
import { DefaultSettings as SettingModelState } from '../../config/defaultSettings';
import { UserModelState } from './user';
import { StateType } from './login';
import { AdminModelState } from './admin';
import { RoleModelState } from './role';
import { ResourceModelState } from './resource';
import { AppSettingState } from './appsetting';
import { ShopModelState } from './shop';
import { ShopTypeModelState } from './shopType';
import { FoodModelState } from './food';
import { formatCountdown } from 'antd/lib/statistic/utils';
export { GlobalModelState, SettingModelState, UserModelState };

export interface Loading {
  global: boolean;
  effects: { [key: string]: boolean | undefined };
  models: {
    global?: boolean;
    menu?: boolean;
    setting?: boolean;
    user?: boolean;
    login?: boolean;
    shop: boolean;
  };
}

export interface ConnectState {
  global: GlobalModelState;
  loading: Loading;
  settings: SettingModelState;
  user: UserModelState;
  login: StateType;
  admin: AdminModelState;
  role: RoleModelState;
  resource: ResourceModelState;
  appsetting: AppSettingState;
  shop: ShopModelState;
  shopType: ShopTypeModelState;
  food: FoodModelState;
}

export interface Route extends MenuDataItem {
  routes?: Route[];
}

/**
 * @type T: Params matched in dynamic routing
 */
export interface ConnectProps<T = {}> extends Partial<RouterTypes<Route, T>> {
  dispatch?: Dispatch<AnyAction>;
}
