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
    public class CategoryBLL:BaseBLL
    {
        #region 查询

        /// <summary>
        /// 获取图书分类
        /// </summary>
        /// <param name="name">分类名</param>
        /// <param name="total">总数据条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns></returns>
        public List<CategoryEntity> QueryCategory(string name,ref int total,int pageIndex, int pageSize) {
            try
            {
                var list = dal.TEntity<CategoryEntity>()
                    .AsQueryable()
                    .WhereIF(name.IsNotNullOrEmpty(), it => it.Name.Contains(name))
                    .Mapper(it=>{ 
                            it.Parent = dal.TEntity<CategoryEntity>().AsQueryable()
                                .Where(e=>e.BookCategoryId==it.ParentId).First(); 
                        });
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
        public List<CategoryEntity> QuerySubclass(int bookCategoryId) {
            try
            {
                var list = dal.TEntity<CategoryEntity>()
                    .AsQueryable()
                    .WhereIF(bookCategoryId.IsNotNullOrEmpty(), it => it.ParentId == bookCategoryId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
