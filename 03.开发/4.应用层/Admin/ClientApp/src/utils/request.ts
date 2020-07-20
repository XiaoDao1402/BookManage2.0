/* eslint-disable no-param-reassign */
/* eslint-disable @typescript-eslint/no-unused-vars */
/**
 * request 网络请求工具
 * 更详细的 api 文档: https://github.com/umijs/umi-request
 */
import { extend } from 'umi-request';
import { notification, message } from 'antd';
import { getToken, clearToken } from './authority';

export const codeMessage = {
  200: '服务器成功返回请求的数据。',
  201: '新建或修改数据成功。',
  202: '一个请求已经进入后台排队（异步任务）。',
  204: '删除数据成功。',
  400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
  401: '用户没有权限（令牌、用户名、密码错误）。',
  403: '用户得到授权，但是访问是被禁止的。',
  404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
  405: '用户没有权限',
  406: '请求的格式不可得。',
  410: '请求的资源被永久删除，且不会再得到的。',
  422: '当创建一个对象时，发生一个验证错误。',
  500: '服务器发生错误，请检查服务器。',
  502: '网关错误。',
  503: '服务不可用，服务器暂时过载或维护。',
  504: '网关超时。',
  601: '登陆过期，请重新登陆',
};

/** 开发环境 */
const HOST_DEV = 'https://localhost:44352';
/** 测试环境 */
// eslint-disable-next-line @typescript-eslint/no-unused-vars
const HOST_TEST = '';
/** 生成环境 */
const HOST_PRO = '';

/**
 * 异常处理程序
 */
const errorHandler = (error: { response: Response; data: ApiModel<any> }): Response => {
  const { response, data } = error;
  if (response && response.status) {
    switch (response.status) {
      case 403:
        notification.error({
          message: `请求错误 403`,
          description: '用户没有授权，请联系管理员',
        });
        break;
      case 405:
        clearToken();
        notification.error({
          message: `请求错误 405`,
          description: '用户没有权限',
        });
        break;
      default:
        // eslint-disable-next-line no-case-declarations
        const errorText =
          (data.result && data.result.message) ||
          codeMessage[response.status] ||
          response.statusText;

        // eslint-disable-next-line no-case-declarations
        const { status } = response;

        notification.error({
          message: `请求错误 ${(data.result && data.result.code) || status}`,
          description: errorText,
        });
        break;
    }
  } else if (!response) {
    clearToken();
    notification.error({
      description: '您的网络发生异常，无法连接服务器',
      message: '网络异常',
    });
  }
  return response;
};

/**
 * 配置request请求时的默认参数
 */
const request = extend({
  errorHandler, // 默认错误处理
  credentials: 'same-origin', // 默认请求是否带上cookie
});

/** 拦截器 */
/**
 * 根据环境变量使用请求的HOST
 */
request.interceptors.request.use((url, options) => {
  const { NODE_ENV } = process.env;
  const token = getToken();

  if (!url.startsWith('/mock-api/')) {
    // 开发环境
    if (NODE_ENV === 'development') {
      url = `${HOST_DEV}${url}`;
    }
  }

  let headers: any = {
    accept: '*/*',
  };

  if (token) {
    headers = { ...headers, Authorization: `Bearer ${token}` };
  }

  // 测试环境
  // 生成环境

  return { url, options: { ...options, headers } };
});

export default request;

/**
 *
 * @param response Api响应
 * @param successText
 * @param errorText
 */
export function apiMessage<T>(response: ApiModel<T>, successText: string, errorText: string) {
  if (response && response.result && response.result.success) {
    message.success(successText);
  } else {
    message.error(
      response && response.result && response.result.message ? response.result.message : errorText,
    );
  }
}
