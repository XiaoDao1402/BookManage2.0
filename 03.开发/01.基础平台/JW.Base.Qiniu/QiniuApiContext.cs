using JW.Base.Qiniuyun.Apis.Security;
using JW.Base.Qiniuyun.Apis.Upload;
using System;

namespace JW.Base.Qiniuyun {
    /// <summary>
    /// 七牛接口上下文
    /// </summary>
    public class QiniuApiContext {
        private static readonly Lazy<QiniuApiContext> Lazy = new Lazy<QiniuApiContext>(() => new QiniuApiContext());
        /// <summary>
        /// 当前上下文接口
        /// </summary>
        public static QiniuApiContext Current => Lazy.Value;

        /// <summary>
        /// 文件上传API
        /// </summary>
        public UploadApi UploadApi = new UploadApi();

        /// <summary>
        /// 七牛内容检查类
        /// </summary>
        public CheckApi CheckApi = new CheckApi();
    }
}
