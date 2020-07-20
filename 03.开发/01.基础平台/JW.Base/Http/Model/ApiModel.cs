using JW.Base.Configuration;
using JW.Base.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JW.Base.Http.Model {
    /// <summary>
    /// Api结果
    /// </summary>
    /// <typeparam name="TValue">数据模型</typeparam>
    [JsonObject]
    public class ApiModel<TValue> where TValue : class {
        /// <summary>
        /// 结果
        /// </summary>
        public ApiResult Result { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TValue Value { get; private set; }
        /// <summary>
        /// Api返回的数据模型（成功）
        /// </summary>
        /// <param name="val">值</param>
        public ApiModel(TValue val) {
            this.Value = val;
            this.Result = ApiResult.SuccessResult;
        }
        /// <summary>
        /// Api返回的数据模型
        /// </summary>
        /// <param name="val">值</param>
        /// <param name="result">结果</param>
        public ApiModel(TValue val, ApiResult result) {
            this.Result = result;
            this.Value = val;
        }
        /// <summary>
        /// Api返回的数据模型（异常）
        /// </summary>
        /// <param name="e">异常</param>
        /// <param name="includeExceptions">返回是否包含异常</param>
        public ApiModel(Exception e, bool includeExceptions) {
            this.Result = ApiResult.FromException(e, includeExceptions);
        }
        /// <summary>
        /// Api返回的数据模型（异常）
        /// </summary>
        /// <param name="errorField"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ApiModel<TValue> AsError(string errorField, string errorMessage) {
            return new ApiModel<TValue>(null, ApiResult.AsError(errorField, errorMessage));
        }
        /// <summary>
        /// Api返回的数据模型（异常）
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static ApiModel<TValue> AsError(string errorMessage, int errorCode = 400) {
            return new ApiModel<TValue>(null, ApiResult.AsError(errorMessage, errorCode));
        }
        /// <summary>
        /// Api返回的数据模型（异常）
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ApiModel<TValue> AsError(string errorMessage, TValue val) {
            return new ApiModel<TValue>(val, ApiResult.AsError(errorMessage));
        }
        /// <summary>
        /// Api返回的数据模型（成功）
        /// </summary>
        /// <param name="val"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiModel<TValue> AsSuccess(TValue val, string message = null) {
            return new ApiModel<TValue>(val, ApiResult.AsSuccess(message));
        }
        /// <summary>
        /// Api返回的数据模型（异常）
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="includeExceptions"></param>
        /// <returns></returns>
        public static ApiModel<TValue> FromException(Exception exception, bool includeExceptions = false) {
            return new ApiModel<TValue>(null, ApiResult.FromException(exception, includeExceptions));
        }
    }
    /// <summary>
    /// 扩展：Api结果
    /// </summary>
    public partial class ApiModel {

        /// <summary>
        /// 全局控制，是否包括异常
        /// </summary>
        public static bool IsIncludeException {
            get {
                if (ConfigurationManager.Current.Configuration["ApiModel:IncludeException"] == "True") {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 全局控制，是否打印异常
        /// </summary>
        public static bool IsLogException {
            get {
                if (ConfigurationManager.Current.Configuration["ApiModel:LogException"] == "True") {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 结果
        /// </summary>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static Task<IActionResult> ActionResult(IActionResult actionResult) {
            return Task.FromResult<IActionResult>(actionResult);
        }
        /// <summary>
        /// 成功结果
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public static ApiModel<TValue> AsSuccess<TValue>(TValue val, string message = null) where TValue : class {
            return new ApiModel<TValue>(val, ApiResult.AsSuccess(message));
        }
        /// <summary>
        /// 成功结果
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public static Task<IActionResult> AsSuccessResult<TValue>(TValue val, string message = null) where TValue : class {
            return ActionResult(new OkObjectResult(AsSuccess(val, message)));
        }

        /// <summary>
        /// 异常结果
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public static ApiModel<TValue> AsError<TValue>(TValue val, string message = null) where TValue : class {
            return new ApiModel<TValue>(val, ApiResult.AsError(message));
        }
        /// <summary>
        /// 错误结果
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public static Task<IActionResult> AsErrorResult<TValue>(TValue val, string message = null) where TValue : class {
            return ActionResult(new BadRequestObjectResult(AsError(val, message)));
        }
        /// <summary>
        /// 错误结果
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="message">信息</param>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static Task<IActionResult> AsErrorResult<TValue>(TValue val, string message = null, int errorCode = 400) where TValue : class {
            return ActionResult(new BadRequestObjectResult(ApiModel<TValue>.AsError(message, errorCode)) { StatusCode = errorCode });
        }

        /// <summary>
        /// 异常结果
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="e">异常</param>
        /// <param name="includeExceptions">包含异常信息</param>
        /// <returns></returns>
        public static ApiModel<TValue> FromException<TValue>(TValue val, Exception e, bool includeExceptions = false) where TValue : class {
            return new ApiModel<TValue>(val, ApiResult.FromException(e, includeExceptions));
        }
        /// <summary>
        /// 异常结果
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="e">异常</param>
        /// <param name="includeExceptions">包含异常信息</param>
        /// <returns></returns>
        public static ApiModel<TValue> FromException<TValue>(Exception e, bool includeExceptions = false) where TValue : class {
            return new ApiModel<TValue>(e, includeExceptions);
        }
        /// <summary>
        /// 异常结果
        /// </summary>
        /// <param name="e">异常</param>
        /// <param name="includeExceptions">包含异常信息</param>
        /// <returns></returns>
        public static Task<IActionResult> AsExceptionResult(Exception e, bool includeExceptions = false) {
            if (IsLogException) {
                LogHelper.Error(e.Message, e);
            }
            return ActionResult(new ObjectResult(FromException<string>(e, includeExceptions)) {
                StatusCode = StatusCodes.Status500InternalServerError
            });
        }
    }
}
