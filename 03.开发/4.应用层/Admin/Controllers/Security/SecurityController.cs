using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Model;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Base.Lang.Encrypt;
using JW.Base.Security;
using JW.Buss.BLL.Admin;
using JW.Data.Entity.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Admin.Controllers.Security {
    /// <summary>
    /// 安全控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : BaseController {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Task<IActionResult> Login(string account, string password) {
            try {
                if (account.IsNullOrEmpty() || password.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<LoginModel>(null, "账户或密码不能为空");
                }

                AdminEntity admin = BLL<AdminBLL>().QueryAdminByAccount(account);

                if (admin.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<LoginModel>(null, $"账户不存在");
                }

                password = DataEncrypt.DataMd5(password);

                if (admin.Password != password) {
                    return ApiModel.AsErrorResult<LoginModel>(null, $"密码错误");
                }

                LoginModel loginModel = new LoginModel() {
                    // 把密码字段过滤后返回
                    Admin = new AdminEntity {
                        Name = admin.Name,
                        Account = admin.Account,
                        AdminId = admin.AdminId,
                        CreateDate = admin.CreateDate,
                        ModifyDate = admin.ModifyDate
                    },
                    Token = GetJwtToken(new SessionUser() { Id = admin.AdminId, Name = admin.Name })
                };
                return ApiModel.AsSuccessResult(loginModel, "登陆成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IActionResult> Logout() {
            try {
                AdminEntity current = CurrentAdmin;
                DisponseJwtToken();
                return ApiModel.AsSuccessResult<LoginModel>(null, "退出成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }
        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public new Task<IActionResult> CurrentUser() {
            try {

                AdminEntity currentAdmin = CurrentAdmin;

                if (currentAdmin.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<string>(null, "登录过期，请重新登录");
                }

                currentAdmin.Password = null;

                return ApiModel.AsSuccessResult(currentAdmin);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

    }
}