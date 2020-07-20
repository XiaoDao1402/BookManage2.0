using System;
using System.Collections.Generic;
using System.Text;
using JW.Base.Lang.Attributes;
using SqlSugar;

namespace JW.Data.Entity.Admin {
	/// <summary>
	/// 管理员表
	/// </summary>
    [Name()]
	[SugarTable("t_admin")]
	public class AdminEntity {
		/// <summary>
		/// 管理员id
		/// </summary>
		public int AdminId { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 账号
		/// </summary>
		public string Account { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 创建日期
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 修改日期
		/// </summary>
		public DateTime ModifyDate { get; set; }

	}
}
