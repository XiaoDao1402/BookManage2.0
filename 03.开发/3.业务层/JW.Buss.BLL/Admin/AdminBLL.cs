using JW.Base.Lang;
using JW.Data.Entity.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Buss.BLL.Admin {
    /// <summary>
    /// 管理员业务层
    /// </summary>
    public class AdminBLL :BaseBLL{

        /// <summary>
        /// 根据账号查找管理员
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AdminEntity QueryAdminByAccount(string account){
            try {
                AdminEntity single = dal.TEntity<AdminEntity>().AsQueryable()
                    .Where(it=>it.Account==account)
                    .First();
                return single;
            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// 查找管理员
        /// </summary>
        /// <param name="total">总数</param>
        /// <param name="pi">页码</param>
        /// <param name="ps">页条数</param>
        /// <returns></returns>
        public List<AdminEntity> QueryAdmin(string name, ref int total, int pi, int ps) {
            try {
                var queryable = dal.TEntity<AdminEntity>()
                    .AsQueryable()
                    .WhereIF(name.IsNotNullOrEmpty(), it => it.Name.Contains(name))
                    .IgnoreColumns(it => it.Password);

                return queryable.ToPageList(pi, ps, ref total);

            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public AdminEntity QueryAdminByPrimaryKey(int adminId) {
            try {
                AdminEntity admin = dal.TEntity<AdminEntity>()
                    .AsQueryable()
                    .Where(it => it.AdminId == adminId)
                    .First();
                return admin;
            } catch (Exception) {

                throw;
            }
        }

    }
}
