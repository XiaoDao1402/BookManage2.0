import { UserBookEntity } from '@/models/userbook';
import { ProColumns } from '@ant-design/pro-table/lib/Table';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import React from 'react';
import ProTable from '@ant-design/pro-table';
import { queryUserBook } from '@/services/userbook';
import { connect } from 'dva';

export interface UserBookProps {}

const UserBookList: React.FC<UserBookProps> = () => {
  const columns: ProColumns<UserBookEntity>[] = [
    {
      title: '借书编号',
      dataIndex: 'userBookId',
      align: 'center',
      valueType: 'digit',
    },
    {
      title: '用户名',
      dataIndex: 'userName',
      align: 'center',
      render: (_, record) => <span>{record.user && record.user.name}</span>,
    },
    {
      title: '图书名',
      dataIndex: 'bookName',
      align: 'center',
      render: (_, record) => <span>{record.book && record.book.name}</span>,
    },
    {
      title: '创建时间',
      dataIndex: 'createDate',
      valueType: 'dateTimeRange',
      align: 'center',
    },
    {
      title: '修改时间',
      dataIndex: 'modifyDate',
      valueType: 'dateTime',
      hideInSearch: true,
      align: 'center',
    },
  ];
  return (
    <PageHeaderWrapper>
      <ProTable<UserBookEntity>
        headerTitle="借书记录"
        rowKey="userBookId"
        request={params => queryUserBook(params)}
        columns={columns}
      ></ProTable>
    </PageHeaderWrapper>
  );
};

export default connect()(UserBookList);
