import { PageHeaderWrapper } from '@ant-design/pro-layout';
import React, { useRef, useState } from 'react';
import { connect } from 'dva';
import ProTable from '@ant-design/pro-table';
import { BookCategoryEntity } from '@/models/category';
import { ProColumns, ActionType } from '@ant-design/pro-table/lib/Table';
import { queryCategory, deleteCategory } from '@/services/category';
import Authorized from '@/utils/Authorized';
import { Button, message } from 'antd';
import { PlusOutlined, EditFilled } from '@ant-design/icons';

export interface BookProps {}

const BookList: React.FC<BookProps> = () => {
  const actionRef = useRef<ActionType>();
  const [createModalVisible, handleModalVisible] = useState<boolean>(false);
  const [editFormValues, setEditFormValues] = useState<BookCategoryEntity>({});

  const handleDelete = async (category: number | number[]) => {
    let id: number[] = [];
    if (category && typeof category === 'number') {
      id = [category];
    }
    if (category && Array.isArray(category)) {
      id = [...category];
    }

    const response = await deleteCategory(id);
    if (response.result && response.result.success) {
      message.success(response.result.message || '删除成功');
      if (actionRef.current) {
        actionRef.current.reload();
      }
    }
  };

  const columns: ProColumns<BookCategoryEntity>[] = [
    {
      title: '分类编号',
      dataIndex: 'bookCategoryId',
      hideInSearch: true,
      hideInTable: true,
      align: 'center',
    },
    {
      title: '分类名称',
      dataIndex: 'name',
      align: 'center',
    },
    {
      title: '上级分类',
      dataIndex: 'parentId',
      render: (_, record) => <span>{record.parent && record.parent.name}</span>,
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
        actionRef={actionRef}
        rowSelection={{}}
        toolBarRender={(action, { selectedRows, selectedRowKeys }) => [
          <Button type="primary">
            <PlusOutlined />
            新增
          </Button>,
          selectedRows && selectedRows.length > 0 && (
            <>
              <Button
                type="primary"
                danger
                onClick={() => handleDelete(selectedRowKeys as number[])}
              >
                批量删除
              </Button>
            </>
          ),
        ]}
      />
      {/* {createModalVisible && (
        <EditForm onCancel={()=>{
          handleModalVisible(false);
          setEditFormValues({});
          if (actionRef.current) {
            actionRef.current.reload();
          }
        }}>

      </EditForm>)} */}
    </PageHeaderWrapper>
  );
};

export default connect()(BookList);
