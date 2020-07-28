using JW.Base.Lang.Attributes;
using JW.Data.Entity.Admin;
using JW.Data.Entity.Category;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Entity.Book
{
    /// <summary>
    /// 图书实体
    /// </summary>
    [Name("图书实体")]
    [SugarTable("t_book")]
    public class BookEntity
    {
		/// <summary>
		/// 图书id
		/// </summary>
		public int BookId { get; set; }

		/// <summary>
		/// 图书名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 图书分类id
		/// </summary>
		public int BookCategoryId { get; set; }

		/// <summary>
		/// 图书封面
		/// </summary>
		public string CoverImage { get; set; }

		/// <summary>
		/// 图书价格
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// 借书次数
		/// </summary>
		public int BorrowNum { get; set; }

		/// <summary>
		/// 总库存
		/// </summary>
		public int TotalStockCount { get; set; }

		/// <summary>
		/// 剩余库存
		/// </summary>
		public int SurplusStockCount { get; set; }

		/// <summary>
		/// 管理员id
		/// </summary>
		public int AdminId { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime ModifyDate { get; set; }

		/// <summary>
		/// 管理员
		/// </summary>
		[SugarColumn(IsIgnore =true)]
		public AdminEntity Admin { get; set;}

		/// <summary>
		/// 图书分类
		/// </summary>
		[SugarColumn(IsIgnore =true)]
		public BookCategoryEntity BookCategory { get; set; }
	}
}
