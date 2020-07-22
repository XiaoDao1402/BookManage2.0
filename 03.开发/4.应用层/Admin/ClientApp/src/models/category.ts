export interface BookCategoryEntity {
  bookCategoryId?: number;
  name?: string;
  parentId?: number;
  createDate?: Date;
  modifyDate?: Date;
  parent?: BookCategoryEntity;
}

export interface BookCategoryState {}

export interface BookCategoryProps {
  namespace: 'category';
  state: BookCategoryState;
  effects: {};
  reducers: {};
}

const BookCategoryModel: BookCategoryProps = {
  namespace: 'category',
  state: {},
  effects: {},
  reducers: {},
};

export default BookCategoryModel;
