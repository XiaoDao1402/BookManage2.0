using JW.Base.Qiniuyun.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace JW.Base.Qiniuyun.Apis.Security {
    /// <summary>
    /// 七牛内容检查类
    /// </summary>
    public class CheckApi {
        private const string get = "GET";
        private const string post = "POST";
        private const string rawQuery = "";
        private const string host = "ai.qiniuapi.com";
        private const string contentType = "application/json";
        private const string finish = "FINISHED";
        private const string pass = "pass";

        /// <summary>
        /// 检查图片合法性
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool ImageCheck(string image) {
            string url = "http://ai.qiniuapi.com/v3/image/censor";


            const string path = "/v3/image/censor";
            string bodyStr = "{ \"data\": { \"uri\": \"" + image + "\" } ,\"params\": {\"scenes\": [\"pulp\", \"terror\", \"politician\"]} }";

            string data = QiniuTokenUtility.GetData(post, path, rawQuery, host, contentType, bodyStr);
            string token = QiniuTokenUtility.GetQiniuToken(data);

            HttpContent httpContent = new StringContent(bodyStr, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage respon = client.PostAsync(url, httpContent).Result;
                var rs = respon.Content.ReadAsStringAsync().Result;
                CheckResponse imageCheckRespon = JsonConvert.DeserializeObject<CheckResponse>(rs);

                if (imageCheckRespon.Result.Suggestion != "pass") {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 视屏合法性检查
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public bool VideoCheck(string video) {
            string url = "http://ai.qiniuapi.com/v3/video/censor";

            const string path = "/v3/video/censor";
            string bodyStr = "{ \"data\": { \"uri\": \"" + video + "\" } ,\"params\": {\"scenes\": [\"pulp\", \"terror\", \"politician\"]} }";

            string data = QiniuTokenUtility.GetData(post, path, rawQuery, host, contentType, bodyStr);
            string token = QiniuTokenUtility.GetQiniuToken(data);

            HttpContent httpContent = new StringContent(bodyStr, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
                var rs = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<JObject>(rs);
                if (result.ContainsKey("job")) {
                    using (HttpClient httpClient = new HttpClient()) {
                        var job_id = result["job"].ToString();
                        // 循环获取状态
                        bool finish = false;
                        VideoCheckJobResponse videoCheckRespon = new VideoCheckJobResponse();
                        do {
                            if (IsCheckFinish(job_id, ref videoCheckRespon)) finish = true;
                        } while (!finish);

                        if (videoCheckRespon.Result.Result.Suggestion != pass) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 检查结束
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="videoCheckRespon"></param>
        /// <returns></returns>
        private bool IsCheckFinish(string jobId, ref VideoCheckJobResponse videoCheckRespon) {
            var data = QiniuTokenUtility.GetData(get, "/v3/jobs/video/" + jobId, rawQuery, host, null, null);
            var token = QiniuTokenUtility.GetQiniuToken(data);
            string jobUrl = "http://ai.qiniuapi.com/v3/jobs/video/" + jobId;

            using (HttpClient httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                var response = httpClient.GetAsync(jobUrl).Result;
                var rs = response.Content.ReadAsStringAsync().Result;
                videoCheckRespon = JsonConvert.DeserializeObject<VideoCheckJobResponse>(rs);

                if (videoCheckRespon.Status != finish) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查结果
        /// </summary>
        private class CheckResult {
            public dynamic Scenes { set; get; }

            public string Suggestion { set; get; }
        }

        /// <summary>
        /// 图片检查响应
        /// </summary>
        private class CheckResponse {
            public string Message { set; get; }

            public string Code { set; get; }

            public CheckResult Result { set; get; }

        };

        /// <summary>
        /// 视屏任务检查响应
        /// </summary>
        private class VideoCheckJobResponse {
            /// <summary>
            /// 唯一标识该视频任务的ID
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 视频审核请求中的body
            /// </summary>
            public dynamic Request { get; set; }
            /// <summary>
            /// 任务状态
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 检查响应
            /// </summary>
            public CheckResponse Result { get; set; }
        }
    }
}
