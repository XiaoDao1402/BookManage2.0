import { ProColumns, ActionType } from '@ant-design/pro-table/lib/Table';
import { BookEntity } from '@/models/book';
import { connect } from 'dva';
import React, { useState, useEffect, useRef } from 'react';
import Authorized from '@/utils/Authorized';
import { Divider, TreeSelect, message, Button, Input, Select, InputNumber } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import { queryBook, deleteBook, updateBook, addBook } from '@/services/book';
import FormItem from 'antd/lib/form/FormItem';
import { queryBookCategoryTree } from '@/services/bookcategory';
import { PlusOutlined } from '@ant-design/icons';
import EditForm from '@/components/EditForm';
import { queryAdmins } from '@/services/admin';
import AvatarUpload from '@/components/AvatarUpload';
import { handleUpload } from '@/services/upload';

export interface BookProps {}

const BookList: React.FC<BookProps> = () => {
  const { Option } = Select;
  const [treeData, setTreeDate] = useState();
  const actionRef = useRef<ActionType>();
  const [createModalVisible, handleModalVisible] = useState<boolean>(false);
  const [editFormValues, setEditFormValues] = useState<BookEntity>({});
  const [adminValues, setAdminValues] = useState();

  useEffect(() => {
    handleQueryCategoryTree();
    handleQueryAdmin();
  }, []);

  const handleQueryCategoryTree = async () => {
    const response = await queryBookCategoryTree();
    if (response && response.result.success) {
      setTreeDate(response.value);
    }
  };

  const handleDeleteBook = async (bookId: number | number[]) => {
    let id: number[] = [];
    if (bookId && typeof bookId === 'number') {
      id = [bookId];
    }
    if (bookId && Array.isArray(bookId)) {
      id = [...bookId];
    }
    const response = await deleteBook(id);
    if (response.result && response.result.success) {
      message.success(response.result.message || '删除成功');
      if (actionRef.current) {
        actionRef.current.reload();
      }
    }
  };

  const handleQueryAdmin = async () => {
    const response = await queryAdmins();
    if (response && response.success) {
      setAdminValues(response.data);
    }
  };

  const columns: ProColumns<BookEntity>[] = [
    {
      title: '图书编号',
      dataIndex: 'bookId',
      align: 'center',
      valueType: 'digit',
    },
    {
      title: '图书名称',
      dataIndex: 'name',
      align: 'center',
    },
    {
      title: '图书类别',
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
      render: (_, record) => <img src={record.coverImage} width={100} height={100} />,
      align: 'center',
    },
    {
      title: '价格',
      dataIndex: 'price',
      hideInSearch: true,
      valueType: 'money',
      align: 'center',
    },
    {
      title: '借书次数',
      dataIndex: 'borrowNum',
      hideInSearch: true,
      valueType: 'digit',
      align: 'center',
    },
    {
      title: '总库存',
      dataIndex: 'totalStockCount',
      hideInSearch: true,
      valueType: 'digit',
      align: 'center',
    },
    {
      title: '剩余库存',
      dataIndex: 'surplusStockCount',
      hideInSearch: true,
      valueType: 'digit',
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
          <Authorized authority="" noMatch={null}>
            <Divider type="vertical" />
            <a onClick={() => handleDeleteBook(record.bookId!)}>删除</a>
          </Authorized>
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
        actionRef={actionRef}
        rowSelection={{}}
        toolBarRender={(action, { selectedRows, selectedRowKeys }) => [
          <Authorized authority="" noMatch={{}}>
            <Button
              type="primary"
              onClick={() => {
                handleModalVisible(true);
              }}
            >
              <PlusOutlined />
              新增
            </Button>
          </Authorized>,
          selectedRows && selectedRows.length > 0 && (
            <>
              <Authorized authority="" noMatch={{}}>
                <Button
                  type="primary"
                  danger
                  onClick={() => handleDeleteBook(selectedRowKeys as number[])}
                >
                  批量删除
                </Button>
              </Authorized>
              ,
            </>
          ),
        ]}
      ></ProTable>
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
          onSubmit={async (value: BookEntity) => {
            let response: ApiModel<BookEntity>;
            if (value.bookId && value.bookId >= 10000) {
              response = await updateBook(value.bookId, value);
            } else {
              response = await addBook(value);
            }
            if (response.result && response.result.success) {
              handleModalVisible(false);
              setEditFormValues({});
              if (actionRef.current) {
                actionRef.current.reload();
              }
            }
          }}
        >
          {editFormValues && editFormValues!.bookId && (
            <FormItem name="bookId" label="图书编号">
              <span className="ant-form-text">{editFormValues!.bookId}</span>
            </FormItem>
          )}
          <FormItem
            name="name"
            label="图书名称"
            rules={[{ required: true, message: '图书名称为必填项' }]}
          >
            <Input placeholder="输入图书名称"></Input>
          </FormItem>
          <FormItem
            name="bookCategoryId"
            label="图书分类"
            rules={[{ required: true, message: '图书分类为必填项' }]}
          >
            <TreeSelect
              treeData={treeData}
              placeholder="选择图书分类"
              allowClear={true}
            ></TreeSelect>
          </FormItem>
          <FormItem
            name="coverImage"
            label="图书封面"
            rules={[{ required: true, message: '请上传图片' }]}
          >
            <AvatarUpload accept="image" upload={handleUpload} />
          </FormItem>
          <FormItem
            name="price"
            label="图书价格"
            rules={[{ required: true, message: '图书价格为必填项' }]}
          >
            <InputNumber style={{ width: 350 }} type="number" placeholder="输入图书价格" />
          </FormItem>
          <FormItem name="borrowNum" label="借书次数">
            <InputNumber style={{ width: 350 }} type="number" placeholder="输入借书次数" />
          </FormItem>
          <FormItem
            name="totalStockCount"
            label="总库存"
            rules={[{ required: true, message: '总库存为必填项' }]}
          >
            <InputNumber style={{ width: 350 }} type="number" placeholder="输入总库存" />
          </FormItem>
          <FormItem
            name="surplusStockCount"
            label="剩余库存"
            rules={[{ required: true, message: '剩余库存为必填项' }]}
          >
            <InputNumber style={{ width: 350 }} type="number" placeholder="输入剩余库存" />
          </FormItem>
          <FormItem
            name="adminId"
            label="管理员"
            rules={[{ required: true, message: '管理员为必填项' }]}
          >
            <Select placeholder="选择管理员" allowClear={true}>
              {adminValues &&
                adminValues.map((item: any, index: number) => (
                  <Option key={index} value={item.adminId}>
                    {item.name}
                  </Option>
                ))}
            </Select>
          </FormItem>
        </EditForm>
      )}
    </PageHeaderWrapper>
  );
};

export default connect()(BookList);
