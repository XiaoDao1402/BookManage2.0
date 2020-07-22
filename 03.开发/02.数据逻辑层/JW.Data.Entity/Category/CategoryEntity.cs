using JW.Base.Lang.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Entity.Category
{
    /// <summary>
    /// 图书分类实体
    /// </summary>
    [Name("图书分类实体")]
    [SugarTable("t_book_category")]
    public class CategoryEntity
    {
        /// <summary>
        /// 图书分类id
        /// </summary>
        public int BookCategoryId { get; set; }

        /// <summary>
        /// 图书分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///  修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

    }
}
