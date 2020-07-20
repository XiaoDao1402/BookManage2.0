/*
 * 版权：Copyright (c) 2020 中国
 *
 * 创建日期：Tuesday April 7th 2020
 * 创建者：胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 修改日期: Tuesday, 7th April 2020 1:55:44 pm
 * 修改者: 胡燕龙(Xiodra) - y.dragon.hu@hotmail.com
 *
 * 说明
 *    1、
 */
import { GridContent } from '@ant-design/pro-layout';
import { Menu, message } from 'antd';
import { MenuMode } from 'antd/lib/menu';
import { connect } from 'dva';
import React, { useState, useEffect, Dispatch } from 'react';
import BaseView from './components/Base';
import SecrityView from './components/Secrity';
import styles from './style.less';
import { fakeChangePassword } from '@/services/login';
import { AnyAction } from 'redux';
import Bind from './components/Bind';

type SelectKeyType = 'base' | 'security' | 'bind';

interface SettingsProps {
  dispatch: Dispatch<AnyAction>;
}

const { Item } = Menu;

const Settings: React.FC<SettingsProps> = ({ dispatch }) => {
  const [selectKey, setSelectKey] = useState<SelectKeyType>('base');
  const [mode, setMode] = useState<MenuMode>('inline');
  const menuMap = {
    base: '基本设置',
    security: '安全设置',
    bind: '绑定用户',
  };

  let main: HTMLDivElement | undefined = undefined;

  useEffect(() => {
    window.addEventListener('resize', resize);
    return () => {
      window.removeEventListener('resize', resize);
    };
  }, []);

  const resize = () => {
    if (!main) {
      return;
    }
    requestAnimationFrame(() => {
      if (!main) {
        return;
      }
      let mode: 'inline' | 'horizontal' = 'inline';
      const { offsetWidth } = main;
      if (main.offsetWidth < 641 && offsetWidth > 400) {
        mode = 'horizontal';
      }
      if (window.innerWidth < 768 && offsetWidth > 400) {
        mode = 'horizontal';
      }
      setMode(mode);
    });
  };

  const getMenu = () => {
    return Object.keys(menuMap).map(item => <Item key={item}>{menuMap[item]}</Item>);
  };

  const handleChangePassword = async (oldPassword: string, password: string) => {
    const response: ApiModel<string> = await fakeChangePassword(oldPassword, password);
    if (response.result && response.result.success) {
      message.success('密码修改成功，请重新登陆');
      await dispatch({
        type: 'login/logout',
      });
    }
  };

  const renderChildren = () => {
    switch (selectKey) {
      case 'base':
        return <BaseView />;
      case 'security':
        return (
          <SecrityView
            onChangePassword={(oldPassword, password) =>
              handleChangePassword(oldPassword, password)
            }
          />
        );
      case 'bind':
        return <Bind />;
      default:
        break;
    }
    return null;
  };

  return (
    <GridContent>
      <div
        className={styles.main}
        ref={ref => {
          if (ref) {
            main = ref;
          }
        }}
      >
        <div className={styles.leftMenu}>
          <Menu
            mode={mode}
            selectedKeys={[selectKey]}
            onClick={({ key }) => setSelectKey(key as SelectKeyType)}
          >
            {getMenu()}
          </Menu>
        </div>
        <div className={styles.right}>
          <div className={styles.title}>{menuMap[selectKey]}</div>
          {renderChildren()}
        </div>
      </div>
    </GridContent>
  );
};

export default connect()(Settings);
