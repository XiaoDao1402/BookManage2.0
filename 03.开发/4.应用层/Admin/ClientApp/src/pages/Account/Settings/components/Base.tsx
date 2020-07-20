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
import { LoginModel } from '@/models/user';
import { Button, Form, Input, message } from 'antd';
import { connect } from 'dva';
import React, { Dispatch, useEffect } from 'react';
import styles from './base.less';
import AvatarUpload from '@/components/AvatarUpload';
import { handleUpload } from '@/services/upload';
import { AdminEntity } from '@/models/admin';
import { moodifyAdminInfo } from '@/services/admin';
import { router } from 'umi';
import { AnyAction } from 'redux';

export interface BaseProps {
  currentUser?: LoginModel;
  admin?: AdminEntity;
  dispatch: Dispatch<AnyAction>;
}

const BaseView: React.FC<BaseProps> = ({ currentUser, admin, dispatch }) => {
  const [form] = Form.useForm();
  let view: HTMLDivElement | undefined = undefined;
  const getAvatarURL = () => {
    if (currentUser && currentUser.admin) {
      if (currentUser.admin.avatar) {
        return currentUser.admin.avatar;
      }
      const url = 'https://gw.alipayobjects.com/zos/rmsportal/BiazfanxmamNRoxxVxka.png';
      return url;
    }
    return '';
  };
  useEffect(() => {
    form.resetFields();
  }, [admin]);

  const handleFinish = async (values: any) => {
    let response: ApiModel<AdminEntity>;
    response = await moodifyAdminInfo(admin!.adminId, values);
    if (response.result && response.result.success) {
      message.success('基本信息更新成功');
      await dispatch({
        type: 'user/fetchCurrent',
      });
      router.push('/account/settings');
    } else {
      response.result && message.error(response.result.message);
    }
  };

  return (
    <div
      className={styles.baseView}
      ref={ref => {
        if (ref) {
          view = ref;
        }
      }}
    >
      <div className={styles.left}>
        <Form
          layout="vertical"
          onFinish={handleFinish}
          initialValues={admin}
          hideRequiredMark
          form={form}
        >
          <Form.Item
            name="name"
            label="名称"
            rules={[
              {
                required: true,
                message: '名称为必填项',
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item name="account" label="账号">
            <Input readOnly disabled />
          </Form.Item>
          <Form.Item
            name="avatar"
            label="头像"
            rules={[{ required: true, message: '头像必须上传' }]}
          >
            <AvatarUpload accept="image" upload={handleUpload} />
          </Form.Item>
          <Form.Item>
            <Button htmlType="submit" type="primary">
              更新基本信息
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
};

export default connect(({ user }: ConnectState) => ({ admin: user.currentUser!.admin || {} }))(
  BaseView,
);
