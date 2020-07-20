using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Dal {
    /// <summary>
    /// 数据上下文
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DBContext<T> : SimpleClient<T> where T : class, new() {
        public DBContext(SqlSugarClient context) : base(context) {

        }

        /// <summary>
        /// 根据主键获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetByIds(dynamic[] ids) {
            return Context.Queryable<T>().In(ids).ToList(); ;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetByIdentityId(int id) {
            return Context.Queryable<T>().InSingle(id);
        }
    }
}
