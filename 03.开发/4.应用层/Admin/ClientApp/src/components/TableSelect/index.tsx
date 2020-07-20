/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Thursday April 23rd 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Thursday, 23rd April 2020 11:04:00 am
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、表格选择器
 */
import React, { PropsWithChildren, ReactElement, useRef, useState, useEffect } from 'react';
import ProTable, { ProColumns, ActionType } from '@ant-design/pro-table';
import { SorterResult } from 'antd/lib/table/interface';
import { UseFetchDataAction } from '@ant-design/pro-table/lib/useFetchData';
import { Button, Input, Modal } from 'antd';

export interface ValueType {
  label?: string;
  value?: string | number;
  [key: string]: any;
}

export interface TableSelectProps<T, V extends ValueType = AnyType> {
  value?: V | string | number;
  columns: ProColumns<T>[];
  headerTitle: string;
  rowKey?: string | GetRowKey<T>;
  onChange?: (value?: V | number | string | V[] | number[] | string[]) => void;
  request: (params?: TableListParams) => Promise<RequestData<T>>;
  onSubmit?: (value?: V | number | string) => void;
  toolBarRender?:
    | false
    | ((
        action: UseFetchDataAction<RequestData<T>>,
        rows: {
          selectedRowKeys?: (string | number)[];
          selectedRows?: T[];
        },
      ) => React.ReactNode[]);
  buttonText?: string;
  showLabel?: boolean;
  showLabelWithValue?: boolean;
  valueWithLabel?: boolean;
  handleSelectChange?: (selectedRowKeys: Key[], selectedRows: T[]) => V;
  disabled?: boolean;
}

const TableSelect: <TEntity, TValue extends ValueType = AnyType>(
  props: PropsWithChildren<TableSelectProps<TEntity, TValue>>,
) => ReactElement = <TEntity, TValue extends ValueType = AnyType>({
  columns,
  request,
  headerTitle,
  rowKey,
  toolBarRender,
  buttonText,
  handleSelectChange,
  onChange,
  onSubmit,
  showLabel,
  valueWithLabel,
  value: values,
  disabled,
  showLabelWithValue,
}: PropsWithChildren<TableSelectProps<TEntity, TValue>>) => {
  const actionRef = useRef<ActionType>();
  const [size, setSize] = useState<SizeType>('small');
  const [sorter, setSorter] = useState<string>('');
  const [visible, setVisible] = useState<boolean>(false);
  const [value, setValue] = useState<ValueType>();

  useEffect(() => {
    if (typeof values === 'object') {
      setValue(values);
    }

    if (typeof values === 'string') {
      const temp: any = values;
      setValue({ value: temp, label: `${value}` });
    }
  }, [values]);

  const triggerChange = (change?: ValueType) => {
    setValue(change);
    if (valueWithLabel) {
      onChange && onChange(change as TValue);
    } else {
      onChange && onChange(change?.value);
    }
  };

  return (
    <>
      <Input
        placeholder="请选择"
        readOnly
        value={
          showLabelWithValue
            ? value
              ? `${value?.label} (${value?.value})`
              : ''
            : showLabel
            ? value?.label
            : value?.value
        }
      />
      <Button
        type="primary"
        style={{ marginTop: 5 }}
        onClick={() => setVisible(true)}
        disabled={disabled}
      >
        {buttonText || '请选择'}
      </Button>
      {visible && (
        <Modal
          width={780}
          visible={visible}
          onOk={() => {
            triggerChange(value);
            onSubmit && onSubmit(value as TValue);
            setVisible(false);
          }}
          onCancel={() => setVisible(false)}
        >
          <ProTable<TEntity>
            headerTitle={headerTitle}
            actionRef={actionRef}
            rowKey={rowKey}
            columns={columns}
            size={size}
            params={{ sorter }}
            onSizeChange={s => setSize(s)}
            request={request}
            toolBarRender={toolBarRender}
            onChange={(_, _filter, _sorter) => {
              const sorterResult = _sorter as SorterResult<TEntity>;
              if (sorterResult.field) {
                setSorter(`${sorterResult.field}_${sorterResult.order}`);
              } else {
                setSorter('');
              }
            }}
            tableAlertRender={(selectedRowKeys, _) => (
              <div>
                已选择 <a style={{ fontWeight: 600 }}>{selectedRowKeys.length}</a> 项&nbsp;&nbsp;
              </div>
            )}
            rowSelection={{
              type: 'radio',
              onChange: (selectedRowKeys: Key[], selectedRows: TEntity[]) => {
                const select = handleSelectChange!(selectedRowKeys, selectedRows);
                triggerChange(select);
              },
            }}
          />
        </Modal>
      )}
    </>
  );
};

export default TableSelect;
