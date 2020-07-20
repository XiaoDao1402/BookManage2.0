using System;
using System.Collections.Concurrent;

namespace JW.Base.Qiniuyun.Config {
    public  class QiniuConfigManager {
        /// <summary>
        /// 
        /// </summary>
        const string QiniuConfigKey = "Qiniu.Config.Key";
        /// <summary>
        /// 懒加载器
        /// </summary>
        private static readonly Lazy<QiniuConfigManager> Lazy = new Lazy<QiniuConfigManager>(() => new QiniuConfigManager());

        internal ConcurrentDictionary<string, QiniuConfig> ConfigConcurrentDictionary = new ConcurrentDictionary<string, QiniuConfig>();
        /// <summary>
        /// 当前管理器
        /// </summary>
        public static QiniuConfigManager Current => Lazy.Value;
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public QiniuConfig GetConfig() {
            if (ConfigConcurrentDictionary[QiniuConfigKey] != null) {
                return ConfigConcurrentDictionary[QiniuConfigKey];
            }
            return null;
        }
        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public QiniuConfigManager SetConfig(QiniuConfig config) {
            ConfigConcurrentDictionary.AddOrUpdate(QiniuConfigKey, config, (tKey, existingVal) => config);
            return this;
        }
    }
}
