import { ProColumns } from '@ant-design/pro-table/lib/Table';
import { BookEntity } from '@/models/book';
import { connect } from 'dva';
import React, { useState, useEffect } from 'react';
import Authorized from '@/utils/Authorized';
import { Divider, TreeSelect } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import { queryBook } from '@/services/book';
import FormItem from 'antd/lib/form/FormItem';
import { queryBookCategoryTree } from '@/services/bookcategory';

export interface BookProps {}

const BookList: React.FC<BookProps> = () => {
  const [treeData, setTreeDate] = useState();

  const handleQueryCategoryTree = async () => {
    const response = await queryBookCategoryTree();
    if (response && response.result.success) {
      setTreeDate(response.value);
    }
  };

  useEffect(() => {
    handleQueryCategoryTree();
  }, []);

  const columns: ProColumns<BookEntity>[] = [
    {
      title: '编号',
      dataIndex: 'bookId',
      align: 'center',
      valueType: 'digit',
    },
    {
      title: '名称',
      dataIndex: 'name',
      align: 'center',
    },
    {
      title: '类别',
      dataIndex: 'bookCategoryId',
      render: (_, record) => <span>{record.bookCategory && record.bookCategory.name}</span>,
      renderFormItem: (item, props: { value?: any }) => (
        <>
          <FormItem name="bookCategoryId">
            <TreeSelect treeData={treeData} placeholder="选择类别" allowClear={true}></TreeSelect>
          </FormItem>
        </>
      ),
      align: 'center',
      valueType: 'digit',
    },
    {
      title: '封面',
      dataIndex: 'coverImage',
      hideInSearch: true,
      align: 'center',
    },
    {
      title: '价格',
      dataIndex: 'price',
      hideInSearch: true,
      align: 'center',
    },
    {
      title: '借书次数',
      dataIndex: 'borrowNum',
      hideInSearch: true,
      align: 'center',
    },
    {
      title: '总库存',
      dataIndex: 'totalStockCount',
      hideInSearch: true,
      align: 'center',
    },
    {
      title: '剩余库存',
      dataIndex: 'surplusStockCount',
      hideInSearch: true,
      align: 'center',
    },
    {
      title: '管理员',
      dataIndex: 'adminId',
      render: (_, record) => <span>{record.admin && record.admin.name}</span>,
      hideInSearch: true,
      align: 'center',
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
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
      render: (_, react) => (
        <>
          <>
            <Authorized authority="" noMatch={null}>
              <a>修改</a>
            </Authorized>
          </>
          <>
            <Authorized authority="" noMatch={null}>
              <Divider />
              <a>删除</a>
            </Authorized>
          </>
        </>
      ),
    },
  ];

  return (
    <PageHeaderWrapper>
      <ProTable<BookEntity>
        headerTitle="图书列表"
        rowKey="bookId"
        request={params => queryBook(params)}
        columns={columns}
      ></ProTable>
    </PageHeaderWrapper>
  );
};

export default connect()(BookList);
