/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Thursday March 5th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Thursday, 5th March 2020 5:34:09 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、头像上传组件
 */
import { beforeUpload, getBase64 } from '@/utils/upload';
import { LoadingOutlined, PlusOutlined } from '@ant-design/icons';
import { Upload } from 'antd';
import React, { useEffect, useState } from 'react';
import './style.less';
import { UploadProps } from 'antd/lib/upload';

/** 接收类型：图片、视频、文档 */
export type AcceptType = 'image' | 'video' | 'doc' | string;

export interface AvatarUploadProps {
  value?: string;
  onChange?: (url: string) => void;
  upload: string | HandleUpload;
  accept?: AcceptType;
  fileName?: string;
}

const AvatarUpload: React.FC<AvatarUploadProps> = ({
  value = '',
  onChange,
  upload,
  accept: fileType,
  fileName,
}) => {
  const [loading, handleLoading] = useState<boolean>(false);
  const [url, handleChangeUrl] = useState<string>(value);
  const [element, setElement] = useState<React.ReactElement>();

  useEffect(() => {
    handleChangeUrl(value);
  }, [value]);

  useEffect(() => {
    if (fileType === 'image') {
      setElement(<img src={value} alt="avatar" style={{ width: '100%' }} />);
    }

    if (fileType === 'video') {
      setElement(<video src={value} style={{ width: 300, height: 120 }} controls />);
    }

    if (fileType === 'doc') {
      setElement(<a onClick={() => window.open(value)}>{fileName || '文档'}</a>);
    }
  }, [url]);

  const triggerChange = (changedValue: string) => {
    onChange && onChange(changedValue);
  };
  const handleChange = (info: any) => {
    if (info.file.status === 'uploading') {
      handleLoading(true);
      return;
    }
    if (info.file.status === 'done') {
      triggerChange!(info.file.response.url);
      // Get this url from response in real world.
      getBase64(info.file.originFileObj, (imageUrl: string) => {
        handleLoading(false);
        handleChangeUrl(imageUrl);
      });
    }
  };

  const uploadButton = (
    <div>
      {loading ? <LoadingOutlined /> : <PlusOutlined />}
      <div className="ant-upload-text">上传</div>
    </div>
  );

  const config: UploadProps = {};

  if (typeof upload === 'string') {
    config.action = upload;
  } else {
    config.customRequest = ({ onProgress, onError, onSuccess, file }) =>
      upload(file, onSuccess, onProgress, onError);
  }

  switch (fileType) {
    case 'image':
      config.accept = 'image/*';
      config.beforeUpload = file => beforeUpload(file);
      break;
    case 'vedio':
      config.accept = 'video/*';
      break;
    case 'doc':
      config.accept = 'doc/*';
      break;
    default:
      config.accept = fileType;
      break;
  }

  return (
    <Upload
      name="avatar"
      listType="picture-card"
      className="avatar-uploader"
      showUploadList={false}
      onChange={handleChange}
      {...config}
    >
      {loading ? uploadButton : url ? element : uploadButton}
    </Upload>
  );
};

export default AvatarUpload;
