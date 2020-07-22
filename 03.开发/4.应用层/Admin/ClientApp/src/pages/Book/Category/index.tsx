import { PageHeaderWrapper } from '@ant-design/pro-layout';
import React from 'react';
import { connect } from 'dva';
import ProTable from '@ant-design/pro-table';
import { BookCategoryEntity } from '@/models/category';
import { ProColumns } from '@ant-design/pro-table/lib/Table';
import { queryCategory } from '@/services/category';
import Authorized from '@/utils/Authorized';

export interface BookProps {}

const BookList: React.FC<BookProps> = () => {
  const columns: ProColumns<BookCategoryEntity>[] = [
    {
      title: '分类编号',
      dataIndex: 'bookCategoryId',
      hideInSearch: true,
      hideInTable: true,
    },
    {
      title: '分类名称',
      dataIndex: 'name',
    },
    {
      title: '上级分类',
      dataIndex: 'parentId',
      hideInSearch: true,
    },
    {
      title: '创建时间',
      dataIndex: 'createDate',
      valueType: 'dateTime',
      hideInSearch: true,
    },
    {
      title: '修改时间',
      dataIndex: 'modifyDate',
      valueType: 'dateTime',
      hideInSearch: true,
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      render: (_, record) => (
        <>
          <Authorized authority="" noMatch={null}>
            <a>修改</a>
          </Authorized>
        </>
      ),
    },
  ];

  return (
    <PageHeaderWrapper>
      <ProTable<BookCategoryEntity>
        headerTitle="图书分类"
        rowKey="bookCategoryId"
        request={params => queryCategory(params)}
        columns={columns}
        rowSelection={{}}
      />
    </PageHeaderWrapper>
  );
};

export default connect()(BookList);
