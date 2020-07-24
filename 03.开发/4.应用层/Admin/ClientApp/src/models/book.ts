import { BookCategoryEntity } from './bookcategory';
import { AdminEntity } from './admin';

export interface BookEntity {
  bookId?: number;
  name?: string;
  bookCategoryId?: number;
  coverImage?: string;
  price?: number;
  borrowNum?: number;
  totalStockCount?: number;
  surplusStockCount?: number;
  adminId?: number;
  createDate?: Date;
  modifyDate?: Date;
  bookCategory?: BookCategoryEntity;
  admin?: AdminEntity;
}

export interface BookState {}

export interface BookProps {
  namespace: 'book';
  state: BookState;
  effects: {};
  reducers: {};
}

const BookModel: BookProps = {
  namespace: 'book',
  state: {},
  effects: {},
  reducers: {},
};

export default BookModel;
