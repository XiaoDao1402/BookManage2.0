using JW.Base.Lang.Attributes;
using JW.Data.Entity.UserBook;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Entity.User
{
    /// <summary>
    /// 用户实体
    /// </summary>
    [Name("用户实体")]
    [SugarTable("t_user")]
    public class UserEntity {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password {get;set;} 

        /// <summary>
        /// 总借书记录
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 用户借书记录
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public UserBookEntity UserBook { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public int Current { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int PageSize { get; set; }

    }
}
