using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Model;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Base.Lang.Encrypt;
using JW.Base.Security;
using JW.Buss.BLL.User;
using JW.Data.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Security {
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : BaseController {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Task<IActionResult> Login(string account, string password) {
            try {
                if (account.IsNullOrEmpty() || password.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<LoginModel>(null, $"账户或密码不能为空");
                }

                UserEntity user = BLL<UserBLL>().QueryUserByAccount(account);

                if (user.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<LoginModel>(null, $"账户不存在");
                }

                password = DataEncrypt.DataMd5(password);

                if (user.Password != password) {
                    return ApiModel.AsErrorResult<LoginModel>(null, $"密码错误");
                }
                LoginModel loginModel = new LoginModel() {
                    // 把密码字段过滤后返回
                    User = new UserEntity {
                        UserId=user.UserId,
                        Name = user.Name,
                        Account = user.Account,
                        TotalCount = user.TotalCount,
                        CreateDate = user.CreateDate,
                        ModifyDate = user.ModifyDate
                    },
                    Token = GetJwtToken(new SessionUser() { Id = user.UserId, Name = user.Name })
                };
                return ApiModel.AsSuccessResult(loginModel, "登录成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("CurrenUser")]
        public Task<IActionResult> CurrenUser() {
            try {
                UserEntity user = CurrentUser;
                return ApiModel.AsSuccessResult(user);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }
    }
}
