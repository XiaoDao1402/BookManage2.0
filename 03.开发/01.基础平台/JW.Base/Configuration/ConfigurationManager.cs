using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Base.Configuration {
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigurationManager {
        /// <summary>
        /// 懒加载器
        /// </summary>
        private static readonly Lazy<ConfigurationManager> Lazy
            = new Lazy<ConfigurationManager>(() => new ConfigurationManager());
        /// <summary>
        /// 当前管理器
        /// </summary>
        public static ConfigurationManager Current => Lazy.Value;
        /// <summary>
        /// 配置
        /// </summary>
        public IConfigurationRoot Configuration;
    }
}
