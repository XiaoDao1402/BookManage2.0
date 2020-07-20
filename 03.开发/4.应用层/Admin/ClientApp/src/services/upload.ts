/*
 * @Author: 何煜杰
 * @Date: 2020-06-10 18:19:46
 * @LastEditors: 何煜杰
 * @LastEditTime: 2020-06-12 19:03:11
 * @Description: 1.上传
 */

import request from '@/utils/request';
import { judgeFile } from '@/utils/upload';
const { post } = request;

export async function upload(
  file: FormData,
  fileType: 'image' | 'video' | undefined,
): ApiModelPromise<FileModel> {
  return post('/api/Upload', { data: file, params: { fileType } });
}

export function handleUpload(
  file: File,
  onSuccess: (response: object, file: File) => void,
  onProgress: (
    event: {
      percent: number;
    },
    file: File,
  ) => void,
  onError: (error: Error) => void,
) {
  let formData = new FormData();
  formData.append('file', file);

  upload(formData, judgeFile(file.name)).then(response => {
    if (response && response.result && response.result.success) {
      onSuccess(response.value, file);
    } else {
      onError(new Error('上传错误'));
    }
  });
}

let id = 1;

const getId = () => {
  id += 1;
  return `${id}`;
};

export const handleBraftEditorUpload = (params: {
  file: File;
  progress: (progress: number) => void;
  libraryId: string;
  success: (res: {
    url: string;
    meta: {
      id: string;
      title: string;
      alt: string;
      loop: boolean;
      autoPlay: boolean;
      controls: boolean;
      poster: string;
    };
  }) => void;
  error: (err: { msg: string }) => void;
}) => {
  const formData = new FormData(),
    { file, success, error } = params;
  formData.append('file', file);

  upload(formData, judgeFile(file.name)).then(response => {
    if (response && response.result && response.result.success) {
      success({
        url: response.value.url,
        meta: {
          id: getId(),
          title: file.name,
          alt: file.name,
          loop: false,
          autoPlay: false,
          controls: false,
          poster: '',
        },
      });
    } else {
      error({ msg: '上传错误' });
    }
  });
};
