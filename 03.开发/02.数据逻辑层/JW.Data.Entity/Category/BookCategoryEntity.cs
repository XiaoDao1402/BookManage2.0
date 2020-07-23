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
    public class BookCategoryEntity
    {
        /// <summary>
        /// 图书分类id
        /// </summary>
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
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

        #region

        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public string Title { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public int Value { get; set; }

        /// <summary>
        /// 子数据
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public List<BookCategoryEntity> Children { get; set; }

        /// <summary>
        /// 键值
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public int Key { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public string Label { get; set; }

        #endregion

        /// <summary>
        /// 上级分类
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public BookCategoryEntity Parent { get; set; }

    }
}
