/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday March 11th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Wednesday, 11th March 2020 10:18:51 am
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、
 */
export interface AppSettingEntity {
  baseId?: number;
  title?: string;
  key?: string;
  value?: string;
  createTime?: Date;
  modifyTime?: Date;
}

export interface AppSettingState {
  page: RequestData<AppSettingEntity>;
}

export interface AppSettingModelType {
  namespace: 'appSetting';
  state: AppSettingState;
  effects: {};
  reducers: {};
}

const AppSettingModel: AppSettingModelType = {
  namespace: 'appSetting',
  state: {
    page: { data: [] },
  },
  effects: {},
  reducers: {},
};

export default AppSettingModel;
