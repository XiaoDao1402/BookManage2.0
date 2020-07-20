using JW.Base.Qiniuyun.Config;
using System;
using System.Security.Cryptography;
using System.Text;

namespace JW.Base.Qiniuyun.Util {
    public static class QiniuTokenUtility {
        /// <summary>
        /// 构造待签名的 Data
        /// </summary>
        /// <param name="method"></param>
        /// <param name="path"></param>
        /// <param name="rawQuery"></param>
        /// <param name="host"></param>
        /// <param name="contentType"></param>
        /// <param name="bodyStr"></param>
        /// <returns></returns>
        public static string GetData(string method, string path, string rawQuery, string host, string contentType, string bodyStr) {
            string data = method + " " + path;
            if (!string.IsNullOrWhiteSpace(rawQuery)) {
                data += "?" + rawQuery;
            }
            data += "\nHost: " + host;
            if (!string.IsNullOrWhiteSpace(contentType)) {
                data += "\nContent-Type: " + contentType;
            }
            data += "\n\n";
            if (!string.IsNullOrWhiteSpace(bodyStr) && !string.IsNullOrWhiteSpace(contentType) && contentType != "application/octet-stream") {
                data += bodyStr;
            }
            return data;
        }

        /// <summary>
        /// 计算 HMAC-SHA1 签名，并对签名结果做 URL 安全的 Base64 编码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSign(string data) {
            QiniuConfig config = QiniuConfigManager.Current.GetConfig();
            //HMACSHA1加密
            HMACSHA1 hmacsha1 = new HMACSHA1 {
                Key = Encoding.UTF8.GetBytes(config.SK)
            };
            byte[] dataBuffer = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return ToBase64StringForUrl(hashBytes);
        }

        /// <summary>
        /// 将 Qiniu 标识与 AccessKey、encodedSign 拼接得到管理凭证
        /// </summary>
        /// <returns></returns>
        public static string GetQiniuToken(string data) {

            QiniuConfig config = QiniuConfigManager.Current.GetConfig();
            string encodedSign = GetSign(data);
            return "Qiniu " + config.AK + ":" + encodedSign;
        }

        /// <summary>
        /// 从二进制字符转换为适用于URL的Base64编码字符串
        /// < /summary>
        public static string ToBase64StringForUrl(byte[] token) {
            return Convert.ToBase64String(token).Replace('+', '-').Replace('/', '_');
        }
    }
}
