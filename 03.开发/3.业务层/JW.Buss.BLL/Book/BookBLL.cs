using JW.Base.Lang;
using JW.Data.Dal;
using JW.Data.Entity.Admin;
using JW.Data.Entity.Category;
using JW.Data.Entity.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JW.Buss.BLL.Book
{
    /// <summary>
    /// 图书业务层
    /// </summary>
    public class BookBLL : BaseBLL
    {
        #region 查询

        /// <summary>
        /// 查询图书
        /// </summary>
        /// <param name="entity">查询参数实体</param>
        /// <param name="total">总数据条数</param>
        /// <returns></returns>
        public List<BookEntity> QueryBook(int[] categories,PageQueryEntity entity, ref int total)
        {
            try
            {
                var list = dal.TEntity<BookEntity>()
                    .AsQueryable()
                    .WhereIF(entity.BookId > 0, it => it.BookId == entity.BookId)
                    .WhereIF(entity.Name.IsNotNullOrEmpty(), it => it.Name.Contains(entity.Name))
                    .WhereIF(entity.BookCategoryId > 0, it => categories.Contains(it.BookCategoryId))
                    .Mapper(it => it.BookCategory, it => it.BookCategoryId)
                    .Mapper(it =>
                    {
                        it.Admin = dal.TEntity<AdminEntity>().AsQueryable()
                        .Where(e => e.AdminId == it.AdminId)
                        .IgnoreColumns(e => e.Password)
                        .First();
                    }).OrderBy(it => it.BookId, OrderByType.Desc);
                return list.ToPageList(entity.Current, entity.PageSize, ref total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询用户新增的图书是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BookEntity QueryBookByName(string name) {
            try
            {
                BookEntity book = dal.TEntity<BookEntity>().AsQueryable().Where(it => it.Name == name).First();
                return book;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
