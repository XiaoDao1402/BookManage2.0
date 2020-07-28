import { UserBookEntity } from './userbook';

export interface UsersEntity {
  UserId?: number;
  name?: string;
  totalCount?: number;
  createDate?: Date;
  modifyDate?: Date;
  userbook?: UserBookEntity;
}

export interface UserState {}

export interface UserProps {
  namespace: 'users';
  state: UserState;
  effects: {};
  reducers: {};
}

const UsersModel: UserProps = {
  namespace: 'users',
  state: {},
  effects: {},
  reducers: {},
};

export default UsersModel;
