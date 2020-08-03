using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Data.Entity.Category;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JW.Buss.BLL.Category
{
    /// <summary>
    /// 图书分类业务层
    /// </summary>
    public class BookCategoryBLL : BaseBLL
    {
        #region 查询

        /// <summary>
        /// 查询图书分类
        /// </summary>
        /// <param name="name">分类名</param>
        /// <param name="total">总数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns></returns>
        public List<BookCategoryEntity> QueryBookCategory(string name, ref int total, int pageIndex, int pageSize)
        {
            try
            {
                var list = dal.TEntity<BookCategoryEntity>()
                    .AsQueryable()
                    .WhereIF(name.IsNotNullOrEmpty(), it => it.Name.Contains(name))
                    .Mapper(it =>
                    {
                        it.Parent = dal.TEntity<BookCategoryEntity>().AsQueryable()
                            .Where(e => e.BookCategoryId == it.ParentId).First();
                    })
                    .OrderBy(it => it.BookCategoryId, OrderByType.Desc);
                return list.ToPageList(pageIndex, pageSize, ref total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据父类id查询子类
        /// </summary>
        /// <param name="bookCategoryId"></param>
        /// <returns></returns>
        public List<BookCategoryEntity> QuerySubclass(int bookCategoryId)
        {
            try
            {
                var list = dal.TEntity<BookCategoryEntity>()
                    .AsQueryable()
                    .WhereIF(bookCategoryId.IsNotNullOrEmpty(), it => it.ParentId == bookCategoryId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询用户新增的图书分类是否已存在
        /// </summary>
        /// <param name="name">分类名</param>
        /// <returns></returns>
        public BookCategoryEntity QueryBookCategoryByName(string name)
        {
            try
            {
                BookCategoryEntity category = dal.TEntity<BookCategoryEntity>()
                    .AsQueryable()
                    .Where(it => it.Name == name)
                    .First();
                return category;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询所有图书分类（用于树形下拉列表）
        /// </summary>
        /// <returns></returns>
        public List<BookCategoryEntity> QueryBookCategoryTree()
        {
            try
            {
                List<BookCategoryEntity> list = dal.SqlSugarDb.Ado.SqlQuery<BookCategoryEntity>(
                    @"WITH child AS(
    	                SELECT res.BookCategoryId,res.Name,res.ParentId
    	                FROM t_book_category res
    	                WHERE res.ParentId = 0
    	                UNION ALL
    	                SELECT res.BookCategoryId,res.Name,res.ParentId
    	                FROM t_book_category res
    	                INNER JOIN
    	                child b
    	                ON(res.ParentId = b.BookCategoryId)
                    )
                    SELECT * FROM child ORDER BY(BookCategoryId)");
                List<BookCategoryEntity> result = (from item in list
                                                   where item.ParentId == 0
                                                   select item).ToList();
                result.ForEach(item =>
                {
                    item.Title = item.Label =  item.Name;
                    item.Value = item.Key = item.BookCategoryId;
                    item.Children = QueryCategoryChildren(list, item.BookCategoryId);
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理子类节点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<BookCategoryEntity> QueryCategoryChildren(List<BookCategoryEntity> list, int parentId)
        {
            try
            {
                List<BookCategoryEntity> children = (from item in list
                                                     where item.ParentId == parentId
                                                     select item).ToList();
                children.ForEach(item =>
                {
                    item.Title = item.Label= item.Name;
                    item.Value = item.Key = item.BookCategoryId;
                    item.Children = QueryCategoryChildren(list, item.BookCategoryId);
                });

                return children;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询所有父级分类
        /// </summary>
        /// <returns></returns>
        public List<BookCategoryEntity> QueryBookCategoryParent() {
            try {
                List<BookCategoryEntity> list = dal.TEntity<BookCategoryEntity>()
                    .AsQueryable()
                    .Where(it => it.ParentId == 0)
                    .ToList();
                return list;
            } catch (Exception ex) {
                throw ex;
            }
        }


        #endregion
    }
}
