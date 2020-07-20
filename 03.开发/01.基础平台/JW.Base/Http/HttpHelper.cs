using JW.Base.File;
using JW.Base.Http.BaseBll;
using JW.Base.Http.Enum;
using JW.Base.Http.Helper;
using JW.Base.Http.Item;
using JW.Base.Lang;
using JW.Base.Lang.Encrypt;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JW.Base.Http {
    /// <summary>
    /// gethtml方法异步调用的委托
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public delegate void ResultHandler(HttpResult item);
    /// <summary>
    /// HTTP工具类
    /// </summary>
    public class HttpHelper {

        #region POST

        /// <summary>
        /// POST请求
        /// </summary>
        public static async Task<string> PostAsync(string url, HttpContent content, Dictionary<string, string> headers) {
            try {
                using (HttpClient client = new HttpClient()) {
                    if (headers != null) {
                        if (content != null) {
                            foreach (var headerItem in headers) {
                                content.Headers.Add(headerItem.Key, headerItem.Value);
                            }
                        } else {
                            foreach (var headerItem in headers) {
                                client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                            }
                        }
                    }

                    var response = await client.PostAsync(url, content);

                    var result = await response.Content.ReadAsStringAsync();

                    return result;
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// POST请求（返回字符串）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string postdata) {
            try {
                StringContent httpContent = null;
                using (HttpClient client = new HttpClient()) {
                    if (!string.IsNullOrWhiteSpace(postdata)) {
                        httpContent = new StringContent(postdata, Encoding.UTF8, "application/json");
                        return await PostAsync(url, httpContent, null);
                    }
                    return null;
                }
            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// POST请求（返回字符串）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <param name="cer"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string postdata, X509Certificate2 cer) {
            try {
                StringContent httpContent = null;
                HttpClientHandler handler = new HttpClientHandler() {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    SslProtocols = SslProtocols.Tls12,
                    ServerCertificateCustomValidationCallback = (x, y, z, m) => true,
                };

                handler.ClientCertificates.Add(cer);

                using (HttpClient client = new HttpClient(handler)) {
                    if (!string.IsNullOrWhiteSpace(postdata)) {
                        httpContent = new StringContent(postdata, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(url, httpContent);

                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    return null;
                }
            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// POST请求（返回具体类型）
        /// </summary>
        /// <typeparam name="T">返回数据对象</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求内容</param>
        /// <param name="headers">请求头部</param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(string url, HttpContent content, Dictionary<string, string> headers) where T : class {
            try {
                var result = await PostAsync(url, content, headers);

                var obj = JsonConvert.DeserializeObject<T>(result);

                return obj;
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// POST请求（返回字符串）
        /// </summary>
        /// <typeparam name="T">数据对象</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">数据</param>
        /// <param name="headers">请求头部</param>
        /// <returns></returns>
        public static async Task<string> PostAsync<T>(string url, T data, Dictionary<string, string> headers) where T : class, new() {
            try {
                StringContent httpContent = null;
                if (data != null) {
                    var json = JsonConvert.SerializeObject(data);
                    httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
                }

                return await PostAsync(url, httpContent, headers);

            } catch (Exception ex) {

                throw ex;
            }
        }

        /// <summary>
        /// POST请求（返回具体类型）
        /// </summary>
        /// <typeparam name="T">数据对象</typeparam>
        /// <typeparam name="V">返回对象</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers">请求头部</param>
        /// <returns></returns>
        public static async Task<V> PostAsync<T, V>(string url, T data, Dictionary<string, string> headers) where T : class, new() where V : class, new() {
            try {

                StringContent httpContent = null;
                if (data != null) {
                    var json = JsonConvert.SerializeObject(data);
                    httpContent = new StringContent(json, new UTF8Encoding(), "application/json");
                }

                return await PostAsync<V>(url, httpContent, headers);
            } catch (Exception) {

                throw;
            }
        }

        /// <summary>
        /// POST请求（支持IFormFile数据）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers">请求头部</param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, Dictionary<string, object> data, Dictionary<string, string> headers) {
            try {
                List<ByteArrayContent> byteList = InitPostParamters(data);

                using (var content = new MultipartFormDataContent()) {
                    //声明一个委托，该委托的作用就是将ByteArrayContent集合加入到MultipartFormDataContent中
                    Action<List<ByteArrayContent>> action = (dataContents) => {
                        foreach (var byteArrayContent in dataContents) {
                            content.Add(byteArrayContent);
                        }
                    };

                    action(byteList);

                    return await PostAsync(url, content, headers);
                }

            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// POST请求（支持IFormFile数据，返回具体类型）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="headers">请求头部</param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(string url, Dictionary<string, object> data, Dictionary<string, string> headers) where T : class {
            try {
                List<ByteArrayContent> byteList = InitPostParamters(data);

                using (var content = new MultipartFormDataContent()) {
                    //声明一个委托，该委托的作用就是将ByteArrayContent集合加入到MultipartFormDataContent中
                    Action<List<ByteArrayContent>> action = (dataContents) => {
                        foreach (var byteArrayContent in dataContents) {
                            content.Add(byteArrayContent);
                        }
                    };

                    action(byteList);

                    return await PostAsync<T>(url, content, headers);
                }

            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="serializeStrFunc"></param>
        /// <param name="inputDataType"></param>
        /// <param name="outDataType"></param>
        /// <param name="cer"></param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(string url, object obj, Func<string, string> serializeStrFunc = null,
          WebRequestDataTypes inputDataType = WebRequestDataTypes.JSON,
          WebRequestDataTypes outDataType = WebRequestDataTypes.JSON,
          X509Certificate2 cer = null) where T : class {
            string postStr;
            switch (inputDataType) {
                case WebRequestDataTypes.XML:
                    postStr = Xml.SerializeObjectWithoutNamespace(obj);
                    break;
                default:
                    postStr = JsonConvert.SerializeObject(obj);
                    break;
            }
            if (serializeStrFunc != null)
                postStr = serializeStrFunc(postStr);

            string result;

            if (cer.IsNullOrEmpty()) {
                result = await PostAsync(url, postStr);
            } else {
                result = await PostAsync(url, postStr, cer);
            }

            switch (outDataType) {
                case WebRequestDataTypes.XML:
                    return Xml.DeserializeObject<T>(result);
                default:
                    return JsonConvert.DeserializeObject<T>(result);
            }
        }

        #endregion

        #region GET

        /// <summary>
        /// GET请求
        /// </summary>
        public static async Task<string> GetAsync(string url) {
            try {
                using (HttpClient client = new HttpClient()) {
                    var response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();

                    return result;
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url) where T : class {
            try {
                using (HttpClient client = new HttpClient()) {
                    var response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();

                    var obj = JsonConvert.DeserializeObject<T>(result);

                    return obj;
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// GET 请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, object> param) {
            try {
                url = InitGetParamters(url, param);

                return await GetAsync(url);

            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, object> param) where T : class {
            try {
                url = InitGetParamters(url, param);

                return await GetAsync<T>(url);

            } catch (Exception ex) {
                throw ex;
            }
        }

        #endregion

        #region HttpHelper
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        public HttpResult GetHtml(HttpItem item) {
            return bll.GetHtml(item);
        }
        /// <summary>
        /// 快速请求方法FastRequest（极速请求不接收数据,只做提交）不返回Header、Cookie、Html
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        public HttpResult FastRequest(HttpItem item) {

            return bll.FastRequest(item);
        }
        #endregion

        #region Cookie
        /// <summary>
        /// 根据字符生成Cookie和精简串，将排除path,expires,domain以及重复项
        /// </summary>
        /// <param name="strcookie">Cookie字符串</param>
        /// <returns>精简串</returns>
        public static string GetSmallCookie(string strcookie) {
            return HttpCookieHelper.GetSmallCookie(strcookie);
        }
        /// <summary>
        /// 将字符串Cookie转为CookieCollection
        /// </summary>
        /// <param name="strcookie">Cookie字符串</param>
        /// <returns>List-CookieItem</returns>
        public static CookieCollection StrCookieToCookieCollection(string strcookie) {
            return HttpCookieHelper.StrCookieToCookieCollection(strcookie);
        }
        /// <summary>
        /// 将CookieCollection转为字符串Cookie
        /// </summary>
        /// <param name="cookie">Cookie字符串</param>
        /// <returns>strcookie</returns>
        public static string CookieCollectionToStrCookie(CookieCollection cookie) {
            return HttpCookieHelper.CookieCollectionToStrCookie(cookie);
        }
        /// <summary>
        /// 自动合并两个Cookie的值返回更新后结果 
        /// </summary>
        /// <param name="cookie1">Cookie1</param>
        /// <param name="cookie2">Cookie2</param>
        /// <returns>返回更新后的Cookie</returns>
        public static string GetMergeCookie(string cookie1, string cookie2) {
            return HttpCookieHelper.GetMergeCookie(cookie1, cookie2);
        }
        #endregion

        #region URL

        /// <summary>
        /// 使用指定的编码对象将 URL 编码的字符串转换为已解码的字符串。
        /// </summary>
        /// <param name="text">指定的字符串</param>
        /// <param name="encoding">指定编码默认为Default</param>
        /// <returns>解码后字符串</returns>
        public static string URLDecode(string text, Encoding encoding = null) {
            return HttpUrlHelper.URLDecode(text, encoding);
        }
        /// <summary>
        /// 使用指定的编码对象对 URL 字符串进行编码。
        /// </summary>
        /// <param name="text">指定的字符串</param>
        /// <param name="encoding">指定编码默认为Default</param>
        /// <returns>转码后字符串</returns>
        public static string URLEncode(string text, Encoding encoding = null) {
            return HttpUrlHelper.URLEncode(text, encoding);
        }
        /// <summary>
        /// 将Url参数字符串转为一个Key和Value的集合
        /// </summary>
        /// <param name="str">要转为集合的字符串</param>
        /// <returns>NameValueCollection</returns>
        public static NameValueCollection GetNameValueCollection(string str) {
            return HttpUrlHelper.GetNameValueCollection(str);
        }
        /// <summary>
        /// 提取网站主机部分就是host
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>host</returns>
        public static string GetUrlHost(string url) {
            return HttpUrlHelper.GetUrlHost(url);
        }
        /// <summary>
        /// 提取网址对应的IP地址
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>返回Url对应的IP地址</returns>
        public static string GetUrlIp(string url) {
            return HttpUrlHelper.GetUrlIp(url);
        }
        #endregion

        #region HTML
        /// <summary>
        /// 获取所有的A链接
        /// </summary>
        /// <param name="html">要分析的Html代码</param>
        /// <returns>返回一个List存储所有的A标签</returns>
        public static List<AItem> GetAList(string html) {
            return HtmlHelper.GetAList(html);
        }
        /// <summary>
        /// 获取所有的Img标签
        /// </summary>
        /// <param name="html">要分析的Html代码</param>
        /// <returns>返回一个List存储所有的Img标签</returns>
        public static List<ImgItem> GetImgList(string html) {
            return HtmlHelper.GetImgList(html);
        }
        /// <summary>
        /// 过滤html标签
        /// </summary>
        /// <param name="html">html的内容</param>
        /// <returns>处理后的文本</returns>
        public static string StripHTML(string html) {
            return HtmlHelper.StripHTML(html);
        }
        /// <summary>
        /// 过滤html中所有的换行符号
        /// </summary>
        /// <param name="html">html的内容</param>
        /// <returns>处理后的文本</returns>
        public static string ReplaceNewLine(string html) {
            return HtmlHelper.ReplaceNewLine(html);
        }

        /// <summary>
        /// 提取Html字符串中两字符之间的数据
        /// </summary>
        /// <param name="html">源Html</param>
        /// <param name="s">开始字符串</param>
        /// <param name="e">结束字符串</param>
        /// <returns></returns>
        public static string GetBetweenHtml(string html, string s, string e) {
            return HtmlHelper.GetBetweenHtml(html, s, e);
        }
        /// <summary>
        /// 提取网页Title
        /// </summary>
        /// <param name="html">Html</param>
        /// <returns>返回Title</returns>
        public static string GetHtmlTitle(string html) {
            return HtmlHelper.GetHtmlTitle(html);
        }
        #endregion

        #region JavaScript
        /// <summary>
        /// 直接调用JS方法并获取返回的值
        /// </summary>
        /// <param name="strJs">要执行的JS代码</param>
        /// <param name="main">要调用的方法名</param>
        /// <returns>执行结果</returns>
        public static string JavaScriptEval(string strJs, string main) {
            return ExecJsHelper.JavaScriptEval(strJs, main);
        }

        #endregion

        #region Encoding
        /// <summary>
        /// 将字节数组转为字符串
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <param name="e">编码，默认为Default</param>
        /// <returns>字符串</returns>
        public static string ByteToString(byte[] b, Encoding e = null) {
            return DataEncrypt.ByteToString(b, e);
        }
        /// <summary>
        /// 将字符串转为字节数组
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="e">编码，默认为Default</param>
        /// <returns>字节数组</returns>
        public static byte[] StringToByte(string s, Encoding e = null) {
            return DataEncrypt.StringToByte(s, e);
        }
        #endregion

        #region Base64

        /// <summary>
        /// 将Base64编码解析成字符串
        /// </summary>
        /// <param name="strbase">要解码的string字符</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>字符串</returns>
        public static string Base64ToString(string strbase, Encoding encoding) {
            return DataEncrypt.Decode(strbase, encoding);
        }
        /// <summary>
        /// 将字节数组为Base64编码
        /// </summary>
        /// <param name="bytebase">要编码的byte[]</param>
        /// <returns>base字符串</returns>
        public static string ByteToBase64(byte[] bytebase) {
            return DataEncrypt.Encode(bytebase);
        }
        /// <summary>
        /// 将字符串转为Base64编码
        /// </summary>
        /// <param name="str">要编码的string字符</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>base字符串</returns>
        public static string StringToBase64(string str, Encoding encoding) {
            return DataEncrypt.Encode(str, encoding);
        }
        #endregion

        #region PRIVATE

        /// <summary>
        /// 初始化GET请求参数
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        private static string InitGetParamters(string url, Dictionary<string, object> param) {
            StringBuilder builder = null;

            foreach (var keyValue in param) {
                (var key, var value) = keyValue;
                // builder 为空时第一次进入遍历，初始化builder后跳出并继续
                if (builder.IsNull()) {
                    if (UrlHelper.IsNormalUrl(url)) {
                        builder = new StringBuilder()
                            .AppendFormat("{0}?{1}={2}", url, key, value);
                    } else {
                        if (url.EndsWith("&")) {
                            builder = new StringBuilder()
                                .AppendFormat("{0}{1}={2}", url, key, value);
                        } else {
                            builder = new StringBuilder()
                                .AppendFormat("{0}&{1}={2}", url, key, value);
                        }
                    }
                    continue;
                }
                // 拼接后续的参数
                builder.AppendFormat("&{0}={1}", keyValue.Key, keyValue.Value);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 初始化POST请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static List<ByteArrayContent> InitPostParamters(Dictionary<string, object> param) {
            List<ByteArrayContent> byteList = new List<ByteArrayContent>();
            foreach (var keyValue in param) {
                (var key, var value) = keyValue;
                Type type = value.GetType();
                ByteArrayContent content = null;
                if (typeof(IFormFile).IsAssignableFrom(type)) {
                    // 文件表单类型
                    var file = (IFormFile)value;
                    Stream stream = file.OpenReadStream();
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    content = new ByteArrayContent(bytes);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
                        Name = key,
                        FileName = file.FileName
                    };
                } else {
                    content = new ByteArrayContent(Encoding.UTF8.GetBytes(value.ToString()));
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
                        Name = key
                    };
                }
                byteList.Add(content);
            }
            return byteList;
        }
        /// <summary>
        /// HttpHelperBLL
        /// </summary>
        private HttpHelperBll bll = new HttpHelperBll();
        /// <summary>
        /// gethtml方法异步调用的委托
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private delegate HttpResult GethtmlHandler(HttpItem item);
        #endregion
    }
}
