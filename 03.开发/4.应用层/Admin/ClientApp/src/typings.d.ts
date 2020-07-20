declare module 'slash2';
declare module '*.css';
declare module '*.less';
declare module '*.scss';
declare module '*.sass';
declare module '*.svg';
declare module '*.png';
declare module '*.jpg';
declare module '*.jpeg';
declare module '*.gif';
declare module '*.bmp';
declare module '*.tiff';
declare module 'omit.js';
declare module '*.json';

// google analytics interface
interface GAFieldsObject {
  eventCategory: string;
  eventAction: string;
  eventLabel?: string;
  eventValue?: number;
  nonInteraction?: boolean;
}
interface Window {
  ga: (
    command: 'send',
    hitType: 'event' | 'pageview',
    fieldsObject: GAFieldsObject | string,
  ) => void;
  reloadAuthorized: () => void;
}

declare let ga: Function;

// preview.pro.ant.design only do not use in your production ;
// preview.pro.ant.design 专用环境变量，请不要在你的项目中使用它。
declare let ANT_DESIGN_PRO_ONLY_DO_NOT_USE_IN_YOUR_PRODUCTION: 'site' | undefined;

declare const REACT_APP_ENV: 'test' | 'dev' | 'pre' | false;

declare type SizeType = 'small' | 'middle' | 'large' | undefined;

declare type CustomTagProps = {
  label: DefaultValueType;
  value: DefaultValueType;
  disabled: boolean;
  onClose: (event?: React.MouseEvent<HTMLElement, MouseEvent>) => void;
  closable: boolean;
};

/** Api结果 */
interface ApiResult {
  /** 状态 */
  status: 'Error' | 'OK';
  /** 是否成功 */
  success: boolean;
  /** 信息 */
  message: string;
  /** 错误详细信息 */
  error: { [key: string]: string } | {};
  /** 状态码 */
  code?: number;
}

declare type PanelRender<RecordType> = (data: RecordType[]) => React.ReactNode;

declare type GetRowKey<RecordType> = (record: RecordType, index?: number) => Key;

declare type RangeValue<DateType> = [EventValue<DateType>, EventValue<DateType>] | null;

declare type Key = React.Key;

declare type AnyType = any;

interface PropsWithDispatch {
  dispatch: Dispatch<AnyAction>;
}

/** Api结果 */
interface ApiModel<TEntity> {
  /** 结果 */
  result: ApiResult;
  /** 返回值 */
  value: TEntity;
}

/** 异步Api结果 */
type ApiModelPromise<TEntity> = Promise<ApiModel<TEntity>>;

interface RequestData<T> {
  data: T[];
  success?: boolean;
  total?: number;
}

type TableModelPromise<TEntity> = Promise<RequestData<TEntity>>;

/** 表格列表分页参数 */
interface TableListPagination {
  total: number;
  pageSize: number;
  current: number;
}

/** 表格列表数据 */
interface TableListData {
  list: TableListItem[];
  pagination: Partial<TableListPagination>;
}

/** 表格列表参数 */
interface TableListParams {
  sorter?: string;
  status?: string;
  name?: string;
  desc?: string;
  key?: number;
  pageSize?: number;
  currentPage?: number;
  current?: number;
  [key: string]: any;
}
interface FileModel {
  fileName: string;
  url: string;
  thumbUrl?: string;
}

declare type HandleUpload = (
  file: File,
  onSuccess: (response: object, file: File) => void,
  onProgress: (
    event: {
      percent: number;
    },
    file: File,
  ) => void,
  onError: (error: Error) => void,
) => void;
