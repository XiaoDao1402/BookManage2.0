/*
 * @Author: 曾敏辉
 * @Date: 2020-07-07 15:14:57
 * @LastEditors: 曾敏辉
 * @LastEditTime: 2020-07-07 15:15:04
 * @Description: file content
 */

import { AdminEntity } from './admin';

export interface HistoryLogEntity {
  historyLogId?: number;
  adminId?: number;
  logType?: number;
  ip?: string;
  target?: string;
  description?: string;
  remarks?: string;
  createTime?: Date;
  modifyTime?: Date;
  adminEntity?: AdminEntity;
}

export interface HistoryLogState {}

export interface HistoryLogModel {
  namespace: 'historylog';
  state: {};
  effects: {};
  reducers: {};
}

const HistoryLogModel: HistoryLogModel = {
  namespace: 'historylog',
  state: {},
  effects: {},
  reducers: {},
};

export default HistoryLogModel;
