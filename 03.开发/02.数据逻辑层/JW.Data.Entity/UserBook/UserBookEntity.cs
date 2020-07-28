using JW.Base.Lang.Attributes;
using JW.Data.Entity.Book;
using JW.Data.Entity.User;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Entity.UserBook
{
    /// <summary>
    /// 借书记录实体
    /// </summary>
    [Name("借书记录实体")]
    [SugarTable("t_user_book")]
    public class UserBookEntity
    {
        /// <summary>
        /// 用户借书id
        /// </summary>
        public int UserBookId { get; set; }

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

        /// <summary>
        /// 用户
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public UserEntity User { get; set; }

        /// <summary>
        /// 图书
        /// </summary>
        [SugarColumn(IsIgnore =true)]
        public BookEntity Book { get; set; }
        
    }
}
