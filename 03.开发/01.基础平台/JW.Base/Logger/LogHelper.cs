using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JW.Base.Logger {
    /// <summary>  
    /// 日志 
    /// </summary>   
    public class LogHelper {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private static readonly ILog logger;
        /// <summary>
        /// 构造函数：日志
        /// </summary>
        static LogHelper() {
            if (logger == null) {
                var repository = LogManager.CreateRepository("NETCoreRepository");
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                logger = LogManager.GetLogger(repository.Name, "InfoLogger");
            }
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception = null) {
            if (exception == null)
                logger.Info(message);
            else
                logger.Info(message, exception);
        }

        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception = null) {
            if (exception == null)
                logger.Warn(message);
            else
                logger.Warn(message, exception);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception = null) {
            if (exception == null)
                logger.Error(message);
            else
                logger.Error(message, exception);
        }

        /// <summary>
        /// 格式化日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="objects"></param>
        public static void InfoFormat(string format, params object[] objects) {
            Info(string.Format(format, objects));
        }
    }
}
