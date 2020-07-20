/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Tuesday April 7th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Tuesday, 7th April 2020 3:00:53 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、
 */
import { ConnectState } from '@/models/connect';
import { LoginModel, UserEntity } from '@/models/user';
import { Button, Form, Input, message, Avatar } from 'antd';
import { connect } from 'dva';
import React, { useState, useEffect } from 'react';
import styles from './base.less';
import TableSelect from '@/components/TableSelect';
import { query } from '@/services/user';
import { AdminEntity } from '@/models/admin';
import { bindUser } from '@/services/admin';
import { apiMessage } from '@/utils/request';

export interface BindProps extends PropsWithDispatch {
  currentUser?: LoginModel;
}

const BindView: React.FC<BindProps> = ({ currentUser, dispatch }) => {
  let view: HTMLDivElement | undefined = undefined;

  const [admin, setAdmin] = useState<AdminEntity>();

  useEffect(() => {
    if (currentUser && currentUser!.admin) {
      if (currentUser!.admin.userId && typeof currentUser?.admin.userId === 'number') {
        currentUser!.admin.userId = {
          label: currentUser?.admin.user?.name,
          value: currentUser?.admin.userId as number,
        };
      }
      setAdmin(currentUser?.admin);
    }
  }, [currentUser]);
  return (
    <div
      className={styles.baseView}
      ref={ref => {
        if (ref) {
          view = ref;
        }
      }}
    >
      <Form
        layout="vertical"
        initialValues={{
          name: currentUser?.admin.user?.name != null ? currentUser?.admin.user?.name : '未绑定',
        }}
        hideRequiredMark
      >
        <Form.Item name="name" label="用户">
          <Input readOnly disabled />
        </Form.Item>
      </Form>
      {/* <div className={styles.left}>
        <Form layout="vertical" initialValues={admin} hideRequiredMark>
          <Form.Item
            name="userId"
            label="用户"
            rules={[
              {
                required: true,
                message: '用户为必填项',
              },
            ]}
          >
            <TableSelect<UserEntity, { value?: number | string; label?: string; user?: UserEntity }>
              columns={[
                {
                  title: '用户头像',
                  dataIndex: 'portrait',
                  hideInSearch: true,
                  render: (text, _) => text && <Avatar shape={'square'} src={text! as string} />,
                },
                { title: '名称', dataIndex: 'name' },
                { title: '城市', dataIndex: 'city', hideInSearch: true },
                { title: '手机号码', dataIndex: 'phone', hideInSearch: true },
              ]}
              request={params => query(params)}
              handleSelectChange={(selectedRowKeys, selectedRows) => {
                return {};
              }}
              headerTitle="选择用户"
              showLabel={true}
              rowKey="userId"
              value={{ label: '132', value: '13456' }}
            />
          </Form.Item>
          <Form.Item>
            <Button htmlType="submit" type="primary" disabled>
              绑定用户
            </Button>
          </Form.Item>
        </Form>
      </div> */}
    </div>
  );
};

export default connect(({ user }: ConnectState) => ({ currentUser: user.currentUser }))(BindView);
