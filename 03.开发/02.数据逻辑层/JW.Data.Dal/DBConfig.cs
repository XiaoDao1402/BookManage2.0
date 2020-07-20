using JW.Base.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Data.Dal {
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DBConfig {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString {
            get {
                return ConfigurationManager.Current.Configuration["ConnectionStrings:ConnString"];
            }
        }
    }
}
