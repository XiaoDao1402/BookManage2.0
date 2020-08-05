using JW.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model {
    /// <summary>
    /// 登录model
    /// </summary>
    public class LoginModel {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserEntity User { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
    }
}
