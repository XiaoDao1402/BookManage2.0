using JW.Base.Lang;
using JW.Data.Entity.Category;
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
        /// <summary>
        /// 获取图书分类
        /// </summary>
        /// <param name="name">分类名</param>
        /// <param name="total">总数据条数</param>
        /// <param name="current">页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns></returns>
        public List<CategoryEntity> QueryCategory(string name,ref int total,int current, int pageSize) {
            try
            {
                var list = dal.TEntity<CategoryEntity>()
                    .AsQueryable()
                    .WhereIF(name.IsNotNullOrEmpty(),it=>it.Name.Contains(name));
                return list.ToPageList(current, pageSize, ref total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
