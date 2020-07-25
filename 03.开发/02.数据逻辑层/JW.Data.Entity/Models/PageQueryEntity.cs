using JW.Base.Lang.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Entity.Models
{
    /// <summary>
    /// 条件实体
    /// </summary>
    [Name("条件实体")]
    public class PageQueryEntity
    {
        #region 查询条件

        /// <summary>
        /// 图书id
        /// </summary>
        public int? BookId { get; set; }

        /// <summary>
        /// 图书名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图书分类id
        /// </summary>
        public int? BookCategoryId { get; set; }

        #endregion

        #region 分页

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Current { get; set; }
        
        /// <summary>
        /// 每页数据条数
        /// </summary>
        public int PageSize { get; set; }

        #endregion
    }
}
