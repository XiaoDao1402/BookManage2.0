using JW.Base.Lang;
using JW.Data.Dal;
using JW.Data.Entity.User;
using JW.Data.Entity.UserBook;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JW.Buss.BLL.User
{
    /// <summary>
    /// 用户业务层
    /// </summary>
    public class UserBLL : BaseBLL
    {
        #region 查询

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="name">用户名</param>
        /// <param name="total">总数据条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns></returns>
        public List<UserEntity> QueryUser(int userId, string name, ref int total, int pageIndex, int pageSize)
        {
            try
            {
                // var list = dal.SqlSugarDb.Queryable<UserEntity, UserBookEntity>((ue, ube) => new object[] {
                //     JoinType.Left,ue.UserId==ube.UserId,
                // })
                // .WhereIF(userId > 0, (ue, ube) => ue.UserId==userId)
                // .WhereIF(name.IsNotNullOrEmpty(), (ue, ube) => ue.Name.Contains(name))
                // .Mapper((ue, ube) =>
                // {
                //     ue.TotalCount = dal.TEntity<UserBookEntity>()
                //                     .AsQueryable()
                //                     .Where(it => it.UserId == ue.UserId).Count();
                // }).PartitionBy(ue => ue.UserId);

                //return list.ToPageList(pageIndex, pageSize, ref total);


                var list = dal.TEntity<UserEntity>()
                    .AsQueryable()
                    .WhereIF(userId > 0, it => it.UserId == userId)
                    .WhereIF(name.IsNotNullOrEmpty(), it => it.Name.Contains(name))
                    .Mapper((ue, cach) =>
                    {
                        List<UserBookEntity> userBookList = cach.Get(saList =>
                        {
                            List<int> userIds = saList.Select(item => item.UserId).ToList();
                            return dal.TEntity<UserBookEntity>()
                                .AsQueryable()
                                .Where(ga => userIds.Contains(ga.UserId))
                                .ToList();
                        });
                        ue.TotalCount = userBookList.Where(item => item.UserId == ue.UserId).Count();
                    }).IgnoreColumns(it=>it.Password);
                return list.ToPageList(pageIndex, pageSize, ref total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询用户当前注册的用户名是否存在(小程序API)
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        public UserEntity QueryUserByName(string name) {
            try {
                UserEntity user = dal.TEntity<UserEntity>()
                    .AsQueryable()
                    .WhereIF(name.IsNotNullOrEmpty(), it => it.Name == name)
                    .First();
                return user;
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 查询用户当前注册的账号是否存在(小程序API)
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        public UserEntity QueryUserByAccount(string account) {
            try {
                UserEntity user = dal.TEntity<UserEntity>()
                   .AsQueryable()
                   .WhereIF(account.IsNotNullOrEmpty(), it => it.Account == account)
                   .First();
                return user;
            } catch (Exception ex) {
                throw ex;
            }
        }

        #endregion
    }
}
