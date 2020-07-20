/*
 * @Author: 何煜杰
 * @Date: 2020-06-10 18:19:46
 * @LastEditors: 何煜杰
 * @LastEditTime: 2020-06-16 17:17:23
 * @Description: file content
 */

/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Thursday March 5th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Thursday, 5th March 2020 5:14:40 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、
 */
import { message } from 'antd';

/**
 * 图片转base64字符串
 */
export function getBase64(img: Blob, callback: any) {
  const reader = new FileReader();
  reader.addEventListener('load', () => callback(reader.result));
  reader.readAsDataURL(img);
}

/**
 * 文件上传：图片文件上传类型及大小检查
 */
export function beforeUpload(file: any) {
  const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
  if (!isJpgOrPng) {
    message.error('只能上传 JPG/PNG 文件');
  }
  const isLt2M = file.size / 1024 / 1024 < 5;
  if (!isLt2M) {
    message.error('Image must smaller than 5MB!');
  }
  return isJpgOrPng && isLt2M;
}

/** 判断文件类型 */
export function judgeFile(fileName: string) {
  var first = fileName.lastIndexOf('.'); //取到文件名开始到最后一个点的长度
  var namelength = fileName.length; //取到文件名长度
  var filesuffix = fileName.substring(first + 1, namelength); //截取获得后缀名

  if (
    filesuffix === 'png' ||
    filesuffix === 'jpg' ||
    filesuffix === 'bmp' ||
    filesuffix === 'gif'
  ) {
    return 'image';
  }

  if (filesuffix === 'mp4' || filesuffix === 'avi' || filesuffix === 'flv') {
    return 'video';
  }
  return undefined;
}

/** 文件结果 */
export const normFile = (e: any) => {
  if (Array.isArray(e)) {
    return e;
  }
  return e && e.fileList;
};
