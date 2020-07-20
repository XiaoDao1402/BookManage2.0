/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday March 11th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Wednesday, 11th March 2020 11:54:00 am
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、弹窗编辑表单
 */
import { Button, Divider, Form, Modal } from 'antd';
import React, { PropsWithChildren, ReactElement, useEffect, useRef } from 'react';
import { FormInstance } from 'antd/lib/form';

export interface EditFormProps<T> {
  editModalVisible: boolean;
  values?: T;
  title?: string;
  onCancel: () => void;
  onSubmit: (values: Partial<T>) => void;
  top?: string | number;
  width?: string | number;
  formItemLayout?: FormItemLayoutType;
  form?: FormInstance;
}

export type FormItemLayoutType = typeof formItemLayout;

const formItemLayout = {
  labelCol: { span: 6 },
  wrapperCol: { span: 14 },
};

const EditForm: <TEntity>(props: PropsWithChildren<EditFormProps<TEntity>>) => ReactElement = ({
  onCancel: handleEditModalVisible,
  onSubmit: handeSubmit,
  editModalVisible,
  values,
  children,
  title,
  top,
  width,
  form: defaultForm,
  formItemLayout: defaultFormItemLayout,
}) => {
  const [form] = Form.useForm(defaultForm);
  const prevVisibleRef = useRef<boolean>();
  useEffect(() => {
    prevVisibleRef.current = editModalVisible;
  }, [editModalVisible]);

  const prevVisible = prevVisibleRef.current;

  useEffect(() => {
    if (!editModalVisible && prevVisible) {
      form.resetFields();
    }
  }, [editModalVisible]);

  const onFinish = (fieldsValue: Partial<any>) => {
    // Should format date value before submit.
    handeSubmit(fieldsValue);
  };

  return (
    <Modal
      destroyOnClose
      title={title || '编辑'}
      visible={editModalVisible}
      onCancel={() => {
        handleEditModalVisible();
      }}
      footer={null}
      width={width}
      style={top ? { top: top } : {}}
    >
      <Form
        {...(defaultFormItemLayout || formItemLayout)}
        initialValues={values}
        form={form}
        onFinish={onFinish}
      >
        {children}
        <Form.Item
          wrapperCol={{
            xs: { span: 24, offset: 0 },
            sm: { span: 16, offset: 8 },
          }}
        >
          <Button type="default" onClick={() => handleEditModalVisible()}>
            返回
          </Button>
          <Divider type="vertical" />
          <Button type="primary" htmlType="submit">
            提交
          </Button>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default EditForm;
