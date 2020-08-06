using JW.Base.Lang.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Entity.UserBookReturn {

    /// <summary>
    /// 还书记录实体
    /// </summary>
    [Name("借书记录实体")]
    [SugarTable("t_user_book_return")]
    public class UserBookReturnEntity {
        /// <summary>
        /// 还书id
        /// </summary>
        public int ReturnId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 图书id
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

    }
}
