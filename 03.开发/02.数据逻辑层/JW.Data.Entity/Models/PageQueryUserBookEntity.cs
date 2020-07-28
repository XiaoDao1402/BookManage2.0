using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Entity.Models
{
    /// <summary>
    /// 分页查询用户借书记录 条件实体
    /// </summary>
    public class PageQueryUserBookEntity
    {
        #region 查询条件

        /// <summary>
        /// 用户借书id
        /// </summary>
        public int? UserBookId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 图书名
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string[] CreateDate { get; set; }

        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? CreateDateStart { get; set; }

        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }

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
