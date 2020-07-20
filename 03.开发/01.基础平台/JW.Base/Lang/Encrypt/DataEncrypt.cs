using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JW.Base.Lang.Encrypt {
    /// <summary>
    /// 数据加密
    /// </summary>
    public class DataEncrypt {

        #region Base64
        /// <summary>
        /// 加密（默认UTF8编码方式）
        /// </summary>
        /// <returns></returns>
        public static string Encode(string source) {
            return Encode(source, Encoding.UTF8);
        }

        /// <summary>
        /// 将字节数组为Base64编码
        /// </summary>
        /// <param name="bytebase">要编码的byte[]</param>
        /// <returns></returns>
        public static string Encode(byte[] bytebase) {
            return Convert.ToBase64String(bytebase);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <returns></returns>
        public static string Encode(string source, Encoding encoding) {
            byte[] bytes = encoding.GetBytes(source);
            string result;
            try {
                result = Convert.ToBase64String(bytes);
            } catch {
                result = source;
            }
            return result;
        }

        /// <summary>
        /// 解密（默认UTF8编码方式）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decode(string source) {
            return Decode(source, Encoding.UTF8);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decode(string source, Encoding encoding) {
            byte[] bytes = Convert.FromBase64String(source);
            string result;
            try {
                result = encoding.GetString(bytes);
            } catch {
                result = source;
            }
            return result;
        }
        #endregion

        #region MD5

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr">需加密的字符串</param>
        /// <returns></returns>
        public static string DataMd5(string encypStr) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] inputBye = Encoding.ASCII.GetBytes(encypStr);
            byte[] outputBye = md5.ComputeHash(inputBye);

            string retStr = Convert.ToBase64String(outputBye);

            return retStr;
        }

        /// <summary>
        /// MD5 Hash 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DataMd5Hash(string str) {
            var output = new byte[128];
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
            var buffer = System.Text.Encoding.Default.GetBytes(str);
            hashAlgorithm.TransformBlock(buffer, 0, str.Length, output, 0);
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
            var md5 = BitConverter.ToString(hashAlgorithm.Hash).ToLower();
            md5 = md5.Replace("-", "");
            return md5;
        }

        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="data"></param>
        /// <returns>MD5值</returns>
        public static string GetMD5HashByByte(byte[] data) {
            try {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(data);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            } catch (Exception ex) {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        #endregion

        #region 对称加密

        /// <summary>
        /// 用于对称算法的密钥
        /// </summary>
        private static byte[] _arrDESKey = new byte[] { 42, 16, 93, 156, 78, 4, 218, 108 };

        /// <summary>
        /// 用于对称算法的初始化向量
        /// </summary>
        private static byte[] _arrDESIV = new byte[] { 55, 103, 246, 79, 36, 99, 167, 99 };

        /// <summary>
        /// 对数据进行编码
        /// </summary>
        /// <param name="encodeStr">需要加密的字符串</param>
        /// <param name="arrDESKey">用于对称算法的密钥</param>
        /// <param name="arrDESIV">用于对称算法的初始化向量</param>
        /// <returns></returns>
        public static string Encode(string encodeStr, byte[] arrDESKey = null, byte[] arrDESIV = null) {
            if (string.IsNullOrEmpty(encodeStr)) {
                throw new Exception("Error: 源字符串为空！！");
            }

            // 如果没传的话，使用默认
            if (arrDESIV == null || arrDESKey == null) {
                arrDESIV = _arrDESIV;
                arrDESKey = _arrDESKey;
            }

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV), CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.Write(encodeStr);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();
            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int)objMemoryStream.Length);
        }

        /// <summary>
        /// 对数据解码
        /// </summary>
        /// <param name="decodeStr">需要解码的字符串</param>
        /// <param name="arrDESKey">用于对称算法的密钥</param>
        /// <param name="arrDESIV">用于对称算法的初始化向量</param>
        /// <returns></returns>
        public static string Decode(string decodeStr, byte[] arrDESKey = null, byte[] arrDESIV = null) {
            if (decodeStr == null) {
                throw new Exception("Error: 源字符串为空！！");
            }

            // 如果没传的话，使用默认
            if (arrDESIV == null || arrDESKey == null) {
                arrDESIV = _arrDESIV;
                arrDESKey = _arrDESKey;
            }

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            byte[] arrInput = Convert.FromBase64String(decodeStr);
            MemoryStream objMemoryStream = new MemoryStream(arrInput);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV), CryptoStreamMode.Read);
            StreamReader objStreamReader = new StreamReader(objCryptoStream);
            return objStreamReader.ReadToEnd();
        }
        #endregion
        
        /// <summary>
        /// 将字节数组转为字符串
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <param name="e">编码，默认为Default</param>
        /// <returns></returns>
        public static string ByteToString(byte[] b, Encoding e = null) {
            if (e == null) {
                e = Encoding.Default;
            }
            string result = e.GetString(b);
            return result;
        }

        /// <summary>
        /// 将字符串转为字节数组
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="e">编码，默认为Default</param>
        /// <returns></returns>
        public static byte[] StringToByte(string s, Encoding e = null) {
            if (e == null) {
                e = Encoding.Default;
            }
            byte[] b = e.GetBytes(s);
            return b;
        }
    }
}
