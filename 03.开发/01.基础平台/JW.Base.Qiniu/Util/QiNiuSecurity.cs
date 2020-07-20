using Qiniu.IO.Model;
using Qiniu.Util;

namespace JW.Base.Qiniuyun.Util {
    /// <summary>
    /// 七牛安全
    /// </summary>
    public class QiNiuSecurity {
        /// <summary>
        /// 公钥
        /// </summary>
        public string PK {
            get;
            private set;
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SK {
            get;
            private set;
        }

        /// <summary>
        /// 七牛安全
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="securityKey">密钥</param>
        public QiNiuSecurity(string publicKey, string securityKey) {
            PK = publicKey;
            SK = securityKey;
        }

        /// <summary>
        /// 鉴权对象
        /// </summary>
        public Mac Mac {
            get {
                return new Mac(PK, SK);
            }
        }

        /// <summary>
        /// 创建上传凭证
        /// </summary>
        /// <param name="bucket">空间名</param>
        /// <param name="saveKey">文件键值</param>
        /// <param name="expires">凭证有效期</param>
        /// <returns></returns>
        public PutPolicy CreatePutPolicy(string bucket, string saveKey, int expires = 3600) {
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy {
                // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
                Scope = bucket + ":" + saveKey
            };
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(expires);
            return putPolicy;
        }
    }
}
