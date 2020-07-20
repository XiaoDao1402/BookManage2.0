/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday March 4th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Wednesday, 4th March 2020 4:17:29 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、管理员账户管理
 */
import EditForm from '@/components/EditForm';
import { AdminEntity, AdminModelState } from '@/models/admin';
import { createAdmin, deleteAdmin, moodifyAdmin, queryAdmins, resetAdmin } from '@/services/admin';
import Authorized from '@/utils/Authorized';
import { PlusOutlined } from '@ant-design/icons';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import { ActionType, ProColumns } from '@ant-design/pro-table/lib/Table';
import { Avatar, Button, Divider, Form, Input, message, Tag } from 'antd';
import { SorterResult } from 'antd/lib/table/interface';
import { connect } from 'dva';
import React, { useRef, useState } from 'react';
import { AnyAction, Dispatch } from 'redux';

export interface AdminProps {
  admin: AdminModelState;
  dispatch: Dispatch<AnyAction>;
}

const FormItem = Form.Item;

const AdminList: React.FC<AdminProps> = () => {
  const [sorter, setSorter] = useState<string>('');
  const [createModalVisible, handleModalVisible] = useState<boolean>(false);
  const [editFormValues, setEditFormValues] = useState<AdminEntity>({});
  const actionRef = useRef<ActionType>();
  const [size, setSize] = useState<SizeType>('small');

  const handleDelete = async (admin: number | number[]) => {
    let id: number[] = [];
    if (admin && typeof admin === 'number') {
      id = [admin];
    }

    if (admin && Array.isArray(admin)) {
      id = [...admin];
    }

    const response = await deleteAdmin(id);

    if (response.result && response.result.success) {
      message.success(response.result.message || '删除成功');
      actionRef.current!.reload();
    }
  };

  const handleReset = async (adminId: number) => {
    const response = await resetAdmin(adminId);

    if (response.result && response.result.success) {
      message.success(response.result.message || '重置密码成功');
      actionRef.current!.reload();
    }
  };

  const columns: ProColumns<AdminEntity>[] = [
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '账号',
      hideInSearch: true,
      dataIndex: 'account',
    },
    {
      title: '操作',
      dataIndex: 'option',
      valueType: 'option',
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
          {record.isAdministrator && (
            <>
              <Authorized authority="" noMatch={null}>
                <Divider type="vertical" />
                <a onClick={() => handleDelete(record.adminId!)}>删除</a>
              </Authorized>
            </>
          )}

          <Authorized authority="" noMatch={null}>
            <Divider type="vertical" />
            <a onClick={() => handleReset(record.adminId!)}>重置密码</a>
          </Authorized>
        </>
      ),
    },
  ];

  return (
    <PageHeaderWrapper>
      <ProTable<AdminEntity>
        headerTitle="管理员"
        actionRef={actionRef}
        rowKey="adminId"
        columns={columns}
        rowSelection={{}}
        size={size}
        onSizeChange={value => setSize(value)}
        request={params => queryAdmins(params)}
        onChange={(_, _filter, _sorter) => {
          const sorterResult = _sorter as SorterResult<AdminEntity>;
          if (sorterResult.field) {
            setSorter(`${sorterResult.field}_${sorterResult.order}`);
          } else {
            setSorter('');
          }
        }}
        params={{
          sorter,
        }}
        toolBarRender={(action, { selectedRows, selectedRowKeys }) => [
          <Authorized authority="" noMatch={null}>
            <Button type="primary" onClick={() => handleModalVisible(true)}>
              <PlusOutlined /> 新建
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
        tableAlertRender={({ selectedRowKeys }) => (
          <div>
            已选择 <a style={{ fontWeight: 600 }}>{selectedRowKeys.length}</a> 项&nbsp;&nbsp;
          </div>
        )}
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
          onSubmit={async value => {
            let response: ApiModel<AdminEntity>;
            if (value.adminId && value.adminId >= 10000) {
              response = await moodifyAdmin(value.adminId, value);
            } else {
              response = await createAdmin(value);
            }

            if (response.result && response.result.success) {
              handleModalVisible(false);
              setEditFormValues({});
              if (actionRef.current) {
                actionRef.current.reload();
              }
            } else {
              // eslint-disable-next-line no-unused-expressions
              response.result && message.error(response.result.message);
            }
          }}
        >
          {editFormValues && editFormValues!.adminId && (
            <FormItem name="adminId" label="管理员编号">
              <span className="ant-form-text">{editFormValues!.adminId}</span>
            </FormItem>
          )}
          <FormItem
            name="name"
            label="管理员名称"
            rules={[{ required: true, message: '管理员名称为必填项' }]}
          >
            <Input placeholder="请输入管理员名称" />
          </FormItem>
          <FormItem
            name="account"
            label="账号"
            rules={[{ required: true, message: '账号为必填项' }]}
          >
            <Input placeholder="请输入账号" />
          </FormItem>
          {editFormValues && editFormValues!.adminId! < 10000 && (
            <FormItem
              name="password"
              label="密码"
              rules={[{ required: true, message: '密码为必填项' }]}
            >
              <Input.Password placeholder="请输入密码" />
            </FormItem>
          )}
          {/* <FormItem name="avatar" label="头像" rules={[{ required: true, message: '请上传头像' }]}>
            <AvatarUpload accept="image" upload={handleUpload} />
          </FormItem> */}
        </EditForm>
      )}
    </PageHeaderWrapper>
  );
};

export default connect()(AdminList);
