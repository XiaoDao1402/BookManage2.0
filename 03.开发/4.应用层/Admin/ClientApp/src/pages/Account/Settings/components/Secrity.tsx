/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Tuesday April 7th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Tuesday, 7th April 2020 4:44:52 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、
 */
import React, { useState } from 'react';
import { List, Form, Input } from 'antd';
import { connect } from 'dva';
import EditForm from '@/components/EditForm';

type Unpacked<T> = T extends (infer U)[] ? U : T;

export interface SecrityViewProps {
  onChangePassword: (oldPassword: string, password: string) => void;
}

const FormItem = Form.Item;

const SecrityView: React.FC<SecrityViewProps> = ({ onChangePassword }) => {
  const [modalVisible, setModalVisible] = useState<boolean>(false);
  const data = [
    {
      title: '账号密码',
      description: null,
      actions: [
        <a key="Modify" onClick={_ => setModalVisible(true)}>
          修改
        </a>,
      ],
    },
  ];

  return (
    <>
      <List<Unpacked<typeof data>>
        itemLayout="horizontal"
        dataSource={data}
        renderItem={item => (
          <List.Item actions={item.actions}>
            <List.Item.Meta title={item.title} description={item.description} />
          </List.Item>
        )}
      />
      {modalVisible && (
        <EditForm
          editModalVisible={modalVisible}
          values={{ oldPassword: '', password: '' }}
          onCancel={() => setModalVisible(false)}
          onSubmit={value => {
            const { oldPassword, password } = value;
            onChangePassword(oldPassword!, password!);
          }}
        >
          <FormItem
            name="oldPassword"
            label="旧密码"
            rules={[{ required: true, message: '旧密码为必填项' }]}
          >
            <Input.Password placeholder="旧密码" />
          </FormItem>
          <FormItem
            name="password"
            label="新密码"
            rules={[{ required: true, message: '新密码为必填项' }]}
          >
            <Input.Password placeholder="新密码" />
          </FormItem>
          <FormItem
            name="confirmPassword"
            label="确认密码"
            dependencies={['password']}
            hasFeedback
            rules={[
              {
                required: true,
                message: '确认密码为必填项',
              },
              ({ getFieldValue }) => ({
                validator(rule, value) {
                  if (!value || getFieldValue('password') === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject('必须与新密码相同');
                },
              }),
            ]}
          >
            <Input.Password placeholder="确认密码" />
          </FormItem>
        </EditForm>
      )}
    </>
  );
};

export default connect()(SecrityView);
