using JW.Data.Dal;
using log4net.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace JW.Buss.BLL {
    /// <summary>
    /// 业务层：基础类
    /// </summary>
    public class BaseBLL {
        /// <summary>
        /// 数据层
        /// </summary>
        protected BaseDAL dal = null;
        /// <summary>
        /// 业务层：基础类
        /// </summary>
        public BaseBLL() {
            dal = new BaseDAL();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T BLL<T>() where T : BaseBLL, new() {
            return new T();
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public T Create<T>(T entity) where T : class, new() {
            try {
                var result = dal.TEntity<T>().AsInsertable(entity).ExecuteReturnEntity();

                return result;

            } catch (Exception ex) {

                throw ex;
            }
        }

        /// <summary>
        /// 新增多个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void Create<T>(List<T> entities) where T : class, new() {

            try {
                _ = dal.TEntity<T>().AsInsertable(entities).ExecuteCommand();

            } catch (Exception ex) {
                throw ex;
            }
        }

        public List<T> Get<T>() where T : class, new() {

            try {
                var result = dal.TEntity<T>().GetList();

                return result;
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 根据主键ID获取对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public T Get<T>(int id) where T : class, new() {
            try {

                var result = dal.TEntity<T>().GetByIdentityId(id);

                return result;

            } catch (Exception ex) {

                throw ex;
            }
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool Update<T>(T entity) where T : class, new() {
            try {
                var result = dal.TEntity<T>().Update(entity);

                return result;

            } catch (Exception ex) {

                throw ex;
            }
        }

        /// <summary>
        /// 更新字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="whereExpression"></param>
        /// <param name="locked">是否锁</param>
        /// <returns></returns>
        public bool Update<T>(T t, Expression<Func<T, object>> columns, Expression<Func<T, object>> where, bool locked = false) where T : class, new() {
            try {
                IUpdateable<T> updateable = dal.SqlSugarDb
                    .Updateable<T>(t)
                    .UpdateColumns(columns)
                    .WhereColumns(where);

                if (locked) {
                    updateable = updateable.With(SqlWith.UpdLock);
                }

                bool result = updateable.ExecuteCommandHasChange();

                return result;
            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// 更新字段（主键）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="whereExpression"></param>
        /// <param name="locked">是否锁</param>
        /// <returns></returns>
        public bool Update<T>(T t, Expression<Func<T, object>> columns, bool locked = false) where T : class, new() {
            try {
                IUpdateable<T> updateable = dal.SqlSugarDb
                    .Updateable<T>(t)
                    .UpdateColumns(columns);

                if (locked) {
                    updateable = updateable.With(SqlWith.UpdLock);
                }

                bool result = updateable.ExecuteCommandHasChange();

                return result;
            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="where"></param>
        /// <param name="locked"></param>
        /// <returns></returns>
        public bool Update<T>(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where, bool locked = false) where T : class, new() {
            try {
                IUpdateable<T> updateable = dal.SqlSugarDb
                    .Updateable<T>()
                    .SetColumns(columns)
                    .Where(where);

                if (locked) {
                    updateable = updateable.With(SqlWith.UpdLock);
                }

                bool result = updateable.ExecuteCommandHasChange();

                return result;
            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// 更新多个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool Update<T>(List<T> entities) where T : class, new() {

            try {
                var result = dal.TEntity<T>().UpdateRange(entities);

                return result;

            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 根据ID删除对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public bool Delete<T>(int id) where T : class, new() {
            try {
                var result = dal.TEntity<T>().DeleteById(id);

                return result;

            } catch (Exception ex) {

                throw ex;
            }
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Delete<T>(Expression<Func<T, bool>> where) where T : class, new() {
            try {
                var result = dal.TEntity<T>()
                    .AsDeleteable()
                    .Where(where)
                    .ExecuteCommandHasChange();

                return result;

            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// 根据ID删除对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">主键</param>
        /// <returns></returns>
        public bool DeleteByIds<T>(int[] ids) where T : class, new() {
            try {
                var result = dal.SqlSugarDb.Deleteable<T>().In(ids).ExecuteCommandHasChange();

                return result;
            } catch (Exception) {

                throw;
            }
        }
    }
}
