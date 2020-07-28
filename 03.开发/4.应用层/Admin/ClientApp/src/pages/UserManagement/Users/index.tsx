import { ProColumns } from '@ant-design/pro-table/lib/Table';
import { UsersEntity } from '@/models/users';
import React from 'react';
import ProTable from '@ant-design/pro-table';
import { connect } from 'dva';
import { queryUser } from '@/services/users';
import { PageHeaderWrapper } from '@ant-design/pro-layout';

export interface UserProps {}

const UsersList: React.FC<UserProps> = () => {
  const columns: ProColumns<UsersEntity>[] = [
    {
      title: '用户编号',
      dataIndex: 'userId',
      valueType: 'digit',
      align: 'center',
    },
    {
      title: '用户名',
      dataIndex: 'name',
      align: 'center',
    },
    {
      title: '总借书记录',
      dataIndex: 'totalCount',
      hideInSearch: true,
      align: 'center',
      sorter: {
        compare: (a, b) => a.totalCount! - b.totalCount!,
        multiple: 4,
      },
    },
    {
      title: '创建时间',
      dataIndex: 'createDate',
      valueType: 'dateTime',
      hideInSearch: true,
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
      <ProTable<UsersEntity, TableListParams>
        headerTitle="用户信息"
        rowKey="userId"
        request={params => {
          const { userId, ...rest } = params!;
          let param = { ...rest };
          param.userId = 0;
          if (userId) {
            param.userId = userId;
          }
          return queryUser(param);
        }}
        columns={columns}
      ></ProTable>
    </PageHeaderWrapper>
  );
};

export default connect()(UsersList);
