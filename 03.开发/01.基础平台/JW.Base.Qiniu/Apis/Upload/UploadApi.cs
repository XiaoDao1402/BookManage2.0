using JW.Base.Lang.Encrypt;
using JW.Base.Qiniuyun.Config;
using JW.Base.Qiniuyun.Util;
using Qiniu.Common;
using Qiniu.IO;
using Qiniu.Util;

namespace JW.Base.Qiniuyun.Apis.Upload {
    /// <summary>
    /// 七牛上传文件
    /// </summary>
    public class UploadApi {
        /// <summary>
        /// 字节流上传
        /// </summary>
        /// <param name="data">上传文件字节流</param>
        /// <param name="suffix">文件后缀名（.jpg）</param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public string UploadByByte(byte[] data, string suffix) {
            QiniuConfig config = QiniuConfigManager.Current.GetConfig();

            // 七牛安全
            QiNiuSecurity security = new QiNiuSecurity(config.AK, config.SK);
            string MD5 = DataEncrypt.GetMD5HashByByte(data);
            string saveKey = MD5;
            if (!string.IsNullOrWhiteSpace(suffix)) {
                saveKey += suffix;
            }
            // 生成(上传)凭证时需要使用此Mac
            Mac mac = security.Mac;

            ZoneID zoneId = ZoneID.CN_South;
            Qiniu.Common.Config.SetZone(zoneId, false);
            // 上传策略
            var putPolicy = security.CreatePutPolicy(config.Bucket, saveKey);
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            FormUploader fu = new FormUploader();
            var result = fu.UploadData(data, saveKey, token);

            string fileUrl = config.Domain + saveKey;
            return fileUrl;
        }
    }
}
