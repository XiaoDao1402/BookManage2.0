using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JW.Base.Http.Model {
    /// <summary>
    /// Api结果
    /// </summary>
    [JsonObject]
    public class ApiResult {
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get { return Errors.Keys.Any() ? "Error" : "OK"; } }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get { return !Errors.Keys.Any(); } }
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }
        /// <summary>
        /// Dictionary key is the field having the error
        /// Value is a list of errors. We don't support errors caused by a combination of fields like the Nancy ModelResult
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, List<string>> Errors { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Code { get; set; }
        /// <summary>
        /// Api结果
        /// </summary>
        /// <param name="errorField">错误字段</param>
        /// <param name="errorMessage">错误消息</param>
        public ApiResult(string errorField, string errorMessage) : this() {
            this.Errors.Add(errorField, new List<string>(new[] { errorMessage }));
        }
        /// <summary>
        /// Api结果
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorMessage">错误消息</param>
        public ApiResult(int errorCode, string errorMessage) : this() {
            this.Code = errorCode;
            this.Message = errorMessage;
            this.Errors.Add("", new List<string>(new[] { errorMessage }));
        }
        /// <summary>
        /// Api结果
        /// </summary>
        public ApiResult() {
            this.Errors = new Dictionary<string, List<string>>();
            this.Code = 200;
        }
        /// <summary>
        /// Api结果
        /// </summary>
        /// <param name="errors">错误</param>
        public ApiResult(Dictionary<string, List<string>> errors) {
            this.Errors = errors;
            this.Code = 500;
        }
        /// <summary>
        /// 扩展：错误
        /// </summary>
        /// <param name="errorField"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ApiResult AsError(string errorField, string errorMessage) {
            return new ApiResult(errorField, errorMessage);
        }
        /// <summary>
        /// 扩展：错误
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static ApiResult AsError(string errorMessage, int errorCode = 400) {
            return new ApiResult(errorCode, errorMessage);
        }
        /// <summary>
        /// 扩展：成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult AsSuccess(string message = null) {
            return new ApiResult { Message = message };
        }
        /// <summary>
        /// 成功结果
        /// </summary>
        public static ApiResult SuccessResult = ApiResult.AsSuccess();
        /// <summary>
        /// 扩展：异常结果
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="includeExceptions"></param>
        /// <returns></returns>
        public static ApiResult FromException(Exception exception, bool includeExceptions = false) {
            ApiResult result;
            if (includeExceptions) {
                result = new ApiResult { Message = "Exception(s) occurred", Code = 500 };
                result.Errors.Add("Exceptions", new List<string>(new[] { exception.Message }));
            } else {
                result = ApiResult.AsError("服务发生异常，请联系管理员", 500);
            }
            return result;
        }
    }
}
