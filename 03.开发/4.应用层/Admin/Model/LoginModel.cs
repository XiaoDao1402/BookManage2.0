using JW.Data.Entity.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Model {
    /// <summary>
    /// 登录model
    /// </summary>
    public class LoginModel {
        /// <summary>
        /// 管理员
        /// </summary>
        public AdminEntity Admin { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
    }
}
