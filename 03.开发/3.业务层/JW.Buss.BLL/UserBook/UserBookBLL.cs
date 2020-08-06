using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Data.Entity.Models;
using JW.Data.Entity.User;
using JW.Data.Entity.Book;
using JW.Data.Entity.UserBook;
using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace JW.Buss.BLL.UserBook
{
    /// <summary>
    /// 用户借书记录业务层
    /// </summary>
    public class UserBookBLL : BaseBLL
    {
        #region 查询

        /// <summary>
        /// 查询用户借书记录
        /// </summary>
        /// <param name="entity">查询参数</param>
        /// <param name="total">总数据条数</param>
        /// <returns></returns>
        public List<UserBookEntity> QueryUserBook(PageQueryUserBookEntity entity, ref int total)
        {
            try
            {
                var list = dal.SqlSugarDb.Queryable<UserBookEntity, UserEntity, BookEntity>((ube, ue, be) => new object[] {
                    JoinType.Left,ube.UserId==ue.UserId,
                    JoinType.Left,ube.BookId==be.BookId
                })
                .WhereIF(entity.UserBookId > 0, (ube, ue, be) => ube.UserBookId == entity.UserBookId)
                .WhereIF(entity.UserName.IsNotNullOrEmpty(), (ube, ue, be) => ue.Name.Contains(entity.UserName))
                .WhereIF(entity.BookName.IsNotNullOrEmpty(), (ube, ue, be) => be.Name.Contains(entity.BookName))
                .WhereIF(entity.CreateDateStart != null, (ube, ue, be) => ube.CreateDate >= entity.CreateDateStart)
                .WhereIF(entity.CreateDateEnd != null, (ube, ue, be) => ube.CreateDate <= entity.CreateDateEnd)
                .Mapper(it => it.User, it => it.UserId)
                .Mapper(it => it.Book, it => it.BookId);

                return list.ToPageList(entity.Current, entity.PageSize, ref total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据用户id查询该用户的借书记录(小程序API)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserBookEntity> GetUserBook(int userId) {
            try {
                List<UserBookEntity> list = dal.TEntity<UserBookEntity>()
                    .AsQueryable()
                    .Where(it => it.UserId == userId)
                    .Where(it=>it.State==1)
                    .Mapper(it => it.User, it => it.UserId)
                    .Mapper(it => it.Book, it => it.BookId)
                    .ToList();
                return list;
            } catch (Exception ex) {
                throw ex;
            }
        }


        #endregion 
    }
}
