/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday March 11th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Wednesday, 11th March 2020 10:10:01 am
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、应用设置
 */
import Authorized from '@/components/Authorized/Authorized';
import EditForm from '@/components/EditForm';
import { AppSettingEntity } from '@/models/appsetting';
import { handleUpload } from '@/services/upload';
import { PlusOutlined } from '@ant-design/icons';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import { Button, Divider, Form, Input, message, Popconfirm, Typography } from 'antd';
import { SorterResult } from 'antd/lib/table/interface';
import { connect } from 'dva';
import React, { useRef, useState } from 'react';
import { AnyAction, Dispatch } from 'redux';
import TypeInput from './components/TypeInput';
import { apiMessage } from '@/utils/request';
import { ActionType, ProColumns } from '@ant-design/pro-table/lib/Table';
import {
  deleteAppSetting,
  queryAppSetting,
  modifyAppSetting,
  createAppSetting,
} from '@/services/appsetting';
const { Paragraph } = Typography;
export interface AppSettingProps {
  dispatch: Dispatch<AnyAction>;
}

const FormItem = Form.Item;

const AppSetting: React.FC<AppSettingProps> = ({ dispatch }) => {
  const [sorter, setSorter] = useState<string>('');
  const [editModalVisible, setEditModalVisible] = useState<boolean>(false);
  const [formValues, setEditFormValues] = useState<AppSettingEntity>({});
  const actionRef = useRef<ActionType>();
  const [size, setSize] = useState<SizeType>('small');

  const handleDelete: (bases: number | number[]) => void = async bases => {
    let id: number[] = [];

    if (!Array.isArray(bases)) {
      id = [bases];
    }

    if (bases && Array.isArray(bases)) {
      id = [...bases];
    }
    const response = await deleteAppSetting(id);

    if (response.result && response.result.success) {
      message.success(response.result.message || '删除成功');
      actionRef.current?.reload();
    }
  };

  const columns: ProColumns<AppSettingEntity>[] = [
    {
      title: '名称',
      dataIndex: 'title',
    },
    {
      title: '键标识',
      dataIndex: 'key',
      hideInSearch: true,
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
                setEditModalVisible(true);
                setEditFormValues(record);
              }}
            >
              修改
            </a>
          </Authorized>
          <Divider type="vertical" />
          <Authorized authority="" noMatch={null}>
            <Popconfirm
              title={`是否删除该记录？`}
              okText="是"
              cancelText="否"
              onConfirm={() => handleDelete(record.baseId!)}
            >
              <a>删除</a>
            </Popconfirm>
          </Authorized>
        </>
      ),
    },
  ];
  return (
    <PageHeaderWrapper>
      <ProTable<AppSettingEntity>
        headerTitle="应用设置"
        actionRef={actionRef}
        rowKey="baseId"
        size={size}
        onSizeChange={size => setSize(size)}
        onChange={(_, _filter, _sorter) => {
          const sorterResult = _sorter as SorterResult<AppSettingEntity>;
          if (sorterResult.field) {
            setSorter(`${sorterResult.field}_${sorterResult.order}`);
          }
        }}
        params={{
          sorter,
        }}
        toolBarRender={(action, { selectedRows, selectedRowKeys }) => [
          <Authorized authority="" noMatch={null}>
            <Button type="primary" onClick={() => setEditModalVisible(true)}>
              <PlusOutlined /> 新建
            </Button>
          </Authorized>,
          selectedRows && selectedRows.length > 0 && (
            <>
              <Authorized authority="" noMatch={null}>
                <Popconfirm
                  title={`是否删除${selectedRows.length}项记录？`}
                  okText="是"
                  cancelText="否"
                  onConfirm={() => handleDelete(selectedRowKeys! as number[])}
                >
                  <Button type="primary" danger>
                    批量删除
                  </Button>
                </Popconfirm>
              </Authorized>
            </>
          ),
        ]}
        tableAlertRender={({ selectedRowKeys }) => (
          <div>
            已选择 <a style={{ fontWeight: 600 }}>{selectedRowKeys.length}</a> 项&nbsp;&nbsp;
          </div>
        )}
        request={params => queryAppSetting(params)}
        columns={columns}
        rowSelection={{}}
      />
      {editModalVisible && (
        <EditForm
          editModalVisible={editModalVisible}
          values={formValues}
          onCancel={() => {
            setEditModalVisible(false);
            setEditFormValues({});
            if (actionRef.current) {
              actionRef.current.reload();
            }
          }}
          onSubmit={async value => {
            let response: ApiModel<AppSettingEntity>;
            if (value.baseId && value.baseId >= 10000) {
              response = await modifyAppSetting(value.baseId, value);
            } else {
              response = await createAppSetting(value);
            }
            apiMessage(response, '编辑成功', '编辑失败');
            setEditModalVisible(false);
            setEditFormValues({});
            if (actionRef.current) {
              actionRef.current.reload();
            }
          }}
          width="80%"
        >
          {formValues && formValues.baseId && (
            <FormItem name="baseId" label="编号">
              <span className="ant-form-text">{formValues.baseId}</span>
            </FormItem>
          )}
          <FormItem name="title" label="名称" rules={[{ required: true, message: '名称为必填项' }]}>
            <Input placeholder="请输入设置名称" />
          </FormItem>
          <FormItem
            name="key"
            label="键标识"
            rules={[{ required: true, message: '键标识为必填项' }]}
          >
            <Input placeholder="请输入设置键" />
          </FormItem>
          <FormItem name="value" label="键值" rules={[{ required: true, message: '键值为必填项' }]}>
            <TypeInput
              placeholder="请输入设置键值"
              accept=".doc,.docx,.pdf"
              upload={handleUpload}
            />
          </FormItem>
        </EditForm>
      )}
    </PageHeaderWrapper>
  );
};

export default connect()(AppSetting);
