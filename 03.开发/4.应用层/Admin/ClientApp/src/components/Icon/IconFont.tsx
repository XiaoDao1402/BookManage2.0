/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Monday March 23rd 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Monday, 23rd March 2020 3:37:07 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、图标
 */
import { createFromIconfontCN } from '@ant-design/icons';
import { connect } from 'dva';
import { ConnectState, SettingModelState } from '@/models/connect';
import React from 'react';

export interface IconFontProps {
  settings: SettingModelState;
  type: string;
}

const IconFont: React.FC<IconFontProps> = ({ settings, type }) => {
  const Icon = createFromIconfontCN({
    // scriptUrl: '//at.alicdn.com/t/font_1690057_m4s3kkq9d9.js',
    scriptUrl: settings.iconfontUrl,
  });

  return <Icon type={type} />;
};

export default connect(({ settings }: ConnectState) => ({ settings }))(IconFont);
