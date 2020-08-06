import { UserEntity } from './user';
import { BookEntity } from './book';

export interface UserBookEntity {
  userBookId?: number;
  userId?: number;
  bookId?: number;
  state: number;
  adminId?: number;
  createDate?: Date;
  modifyDate?: Date;
  returnDate?: string;
  user?: UserEntity;
  book?: BookEntity;
}

export interface UserBookState {}

export interface UserBookProps {
  namespace: 'userbook';
  state: UserBookState;
  effects: {};
  reducers: {};
}

const UserBookModel: UserBookProps = {
  namespace: 'userbook',
  state: {},
  effects: {},
  reducers: {},
};

export default UserBookModel;
