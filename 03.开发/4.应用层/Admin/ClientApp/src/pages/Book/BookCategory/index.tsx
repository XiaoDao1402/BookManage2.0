import { PageHeaderWrapper } from '@ant-design/pro-layout';
import React, { useRef, useState, useEffect } from 'react';
import { connect } from 'dva';
import ProTable from '@ant-design/pro-table';
import { BookCategoryEntity } from '@/models/bookcategory';
import { ProColumns, ActionType } from '@ant-design/pro-table/lib/Table';
import {
  queryBookCategory,
  deleteBookCategory,
  addBookCategory,
  queryBookCategoryTree,
  updateBookCategory,
} from '@/services/bookcategory';
import Authorized from '@/utils/Authorized';
import { Button, message, TreeSelect, Input, Divider } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import EditForm from '@/components/EditForm';
import FormItem from 'antd/lib/form/FormItem';

export interface BookProps {}

const BookCategoryList: React.FC<BookProps> = () => {
  const actionRef = useRef<ActionType>();
  const [createModalVisible, handleModalVisible] = useState<boolean>(false);
  const [editFormValues, setEditFormValues] = useState<BookCategoryEntity>({});
  const [treeData, setTreeDate] = useState();

  useEffect(() => {
    handleQueryCategoryTree();
  }, []);

  const handleDelete = async (category: number | number[]) => {
    let id: number[] = [];
    if (category && typeof category === 'number') {
      id = [category];
    }
    if (category && Array.isArray(category)) {
      id = [...category];
    }
    const response = await deleteBookCategory(id);
    if (response.result && response.result.success) {
      message.success(response.result.message || '删除成功');
      if (actionRef.current) {
        actionRef.current.reload();
      }
    }
  };

  const handleQueryCategoryTree = async () => {
    const response = await queryBookCategoryTree();
    if (response && response.result.success) {
      setTreeDate(response.value);
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
      align: 'center',
      render: (_, record) => (
        <>
          <Authorized authority="" noMatch={null}>
            <a
              onClick={() => {
                handleModalVisible(true);
                setEditFormValues(record);
              }}
            >
              修改
            </a>
          </Authorized>
          <>
            <Authorized authority="" noMatch={null}>
              <Divider type="vertical" />
              <a onClick={() => handleDelete(record.bookCategoryId!)}>删除</a>
            </Authorized>
          </>
        </>
      ),
    },
  ];

  return (
    <PageHeaderWrapper>
      <ProTable<BookCategoryEntity>
        headerTitle="图书分类"
        rowKey="bookCategoryId"
        request={params => queryBookCategory(params)}
        columns={columns}
        actionRef={actionRef}
        rowSelection={{}}
        toolBarRender={(action, { selectedRows, selectedRowKeys }) => [
          <Authorized authority="" noMatch={null}>
            <Button
              type="primary"
              onClick={() => {
                handleModalVisible(true);
              }}
            >
              <PlusOutlined /> 新增
            </Button>
          </Authorized>,
          selectedRows && selectedRows.length > 0 && (
            <>
              <Authorized authority="" noMatch={null}>
                <Button
                  type="primary"
                  danger
                  onClick={() => handleDelete(selectedRowKeys as number[])}
                >
                  批量删除
                </Button>
              </Authorized>
            </>
          ),
        ]}
      />
      {createModalVisible && (
        <EditForm
          onCancel={() => {
            handleModalVisible(false);
            setEditFormValues({});
            if (actionRef.current) {
              actionRef.current.reload();
            }
          }}
          editModalVisible={createModalVisible}
          values={editFormValues}
          onSubmit={async (value: BookCategoryEntity) => {
            let response: ApiModel<BookCategoryEntity>;
            if (value.bookCategoryId && value.bookCategoryId >= 10000) {
              response = await updateBookCategory(value.bookCategoryId, value);
            } else {
              response = await addBookCategory(value);
            }
            if (response.result && response.result.success) {
              handleModalVisible(false);
              setEditFormValues({});
              if (actionRef.current) {
                actionRef.current.reload();
              }
            } else {
              response.result && message.error(response.result.message);
            }
          }}
        >
          {editFormValues && editFormValues!.bookCategoryId && (
            <FormItem name="bookCategoryId" label="分类编号">
              <span className="ant-form-text">{editFormValues!.bookCategoryId}</span>
            </FormItem>
          )}
          <FormItem
            name="name"
            label="分类名称"
            rules={[{ required: true, message: '分类名称为必填项' }]}
          >
            <Input placeholder="请输入分类名称"></Input>
          </FormItem>
          <FormItem name="parentId" label="上级分类">
            <TreeSelect
              treeData={treeData}
              placeholder="请选择上级类型"
              allowClear={true}
            ></TreeSelect>
          </FormItem>
        </EditForm>
      )}
    </PageHeaderWrapper>
  );
};

export default connect()(BookCategoryList);
