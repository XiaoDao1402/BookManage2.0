/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Wednesday March 11th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Wednesday, 11th March 2020 2:15:49 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、
 */
import { UploadOutlined } from '@ant-design/icons';
import { Button, Divider, Input, message, Radio, Upload } from 'antd';
import { UploadProps } from 'antd/lib/upload';
import BraftEditor, { ControlType } from 'braft-editor';
import 'braft-editor/dist/index.css';
import React, { useState } from 'react';
import styles from './style.less';
import { handleBraftEditorUpload } from '@/services/upload';

enum InputType {
  Input,
  Upload,
  RichText,
}

export interface TypeInputProps {
  value?: string;
  onChange?: (value: string) => void;
  placeholder?: string;
  upload?: HandleUpload;
  accept?: string;
}

const TypeInput: React.FC<TypeInputProps> = ({ value, placeholder, onChange, upload, accept }) => {
  const [type, setType] = useState<InputType>(InputType.Input);

  const onChangeType = (e: any) => {
    setType(e.target.value);
  };

  const triggerChange = (changedValue: string) => {
    onChange && onChange(changedValue);
  };

  const props: UploadProps = {
    name: 'file',
    accept,
    onChange: (info: any) => {
      if (info.file.status === 'done') {
        triggerChange(info.file.response.url);
      } else if (info.file.status === 'error') {
        message.error(`${info.file.name} file upload failed.`);
      }
    },
    customRequest: ({ file, onSuccess, onProgress, onError }) =>
      upload && upload(file, onSuccess, onProgress, onError),
  };
  return (
    <>
      <Radio.Group onChange={onChangeType} defaultValue={type}>
        <Radio.Button value={InputType.Input}>文本</Radio.Button>
        <Radio.Button value={InputType.Upload}>文件</Radio.Button>
        <Radio.Button value={InputType.RichText}>富文本</Radio.Button>
      </Radio.Group>
      <Divider style={{ margin: '8px 0' }} />
      {(() => {
        switch (type) {
          case InputType.Upload:
            return (
              <Upload {...props}>
                <Button>
                  <UploadOutlined /> 点击上传
                </Button>
              </Upload>
            );
          case InputType.RichText:
            return (
              <BraftEditor
                value={BraftEditor.createEditorState(value)}
                className={styles.editor}
                media={{
                  uploadFn: handleBraftEditorUpload,
                  accepts: {
                    image: 'image/png,image/jpeg,image/gif,image/bmp',
                    video: 'video/mp4,video/flv,video/avi',
                    audio: false,
                  },
                  externals: {
                    audio: false,
                    embed: false,
                  },
                }}
                placeholder="请输入内容"
                onChange={e => {
                  triggerChange(e.toHTML());
                }}
              />
            );
          case InputType.Input:
          default:
            return (
              <Input
                value={value}
                placeholder={placeholder}
                onChange={e => triggerChange(e.target.value)}
              />
            );
        }
      })()}
    </>
  );
};

export default TypeInput;
