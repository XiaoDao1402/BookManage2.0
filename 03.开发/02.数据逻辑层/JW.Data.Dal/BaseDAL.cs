using JW.Base.Lang;
using SqlSugar;
using System;
using System.Linq;

namespace JW.Data.Dal {
    /// <summary>
    /// 基础数据层
    /// </summary>
    public class BaseDAL {

        /// <summary>
        /// 用来处理事务多表查询和复杂的操作
        /// </summary>
        public SqlSugarClient SqlSugarDb;
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public BaseDAL() {
            SqlSugarDb = new SqlSugarClient(new ConnectionConfig() {
                ConnectionString = DBConfig.ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
#if DEBUG
            //调式代码 用来打印SQL 
            SqlSugarDb.Aop.OnLogExecuting = (sql, pars) => {
                Console.WriteLine($"{sql}\r\n{SqlSugarDb.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value))}");
                Console.WriteLine();
            };
#endif
        }

        /// <summary>
        /// 数据实体上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DBContext<T> TEntity<T>() where T : class, new() => new DBContext<T>(SqlSugarDb);

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTran() {
            SqlSugarDb.BeginTran();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran() {
            SqlSugarDb.CommitTran();
        }
        /// <summary>
        /// 回滚
        /// </summary>
        public void RollbackTran() {
            SqlSugarDb.RollbackTran();
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="processAction">数据处理方法</param>
        /// <param name="failAction">失败方法</param>
        public void UseTran(Action processAction, Action<Exception> failAction = null) {
            try {
                BeginTran();
                if (processAction.IsNotNullOrEmpty()) {
                    processAction();
                }
                CommitTran();
            } catch (Exception ex) {
                RollbackTran();
                if (failAction.IsNotNullOrEmpty()) {
                    failAction(ex);
                } else {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="processAction">数据处理方法</param>
        /// <param name="failAction">失败方法</param>
        /// <returns>返回结果</returns>
        public T UseTran<T>(Func<T> processAction, Action<Exception> failAction = null) {
            T value = default(T);
            try {
                BeginTran();
                if (processAction.IsNotNullOrEmpty()) {
                    value = processAction();
                }
                CommitTran();
            } catch (Exception ex) {
                RollbackTran();
                if (failAction.IsNotNullOrEmpty()) {
                    failAction(ex);
                }
            }
            return value;
        }
    }
}
