using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Base.Lang.Encrypt;
using JW.Buss.BLL.User;
using JW.Data.Entity.Book;
using JW.Data.Entity.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController {

        #region 字段

        private const string DEFAULT_PASSWORD = "000000";

        #endregion

        #region 新增

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        public Task<IActionResult> AddUser(UserEntity entity) {
            try {
                if (entity.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<UserEntity>(null, $"参数有误");
                }
                if (entity.Name.IsNotNullOrEmpty()) {
                    UserEntity user = BLL<UserBLL>().QueryUserByName(entity.Name);
                    if (user.IsNotNullOrEmpty()) {
                        return ApiModel.AsErrorResult<UserEntity>(null, $"该用户名已存在");
                    }
                }
                if (entity.Account.IsNotNullOrEmpty()) {
                    UserEntity user = BLL<UserBLL>().QueryUserByAccount(entity.Account);
                    if (user.IsNotNullOrEmpty()) {
                        return ApiModel.AsErrorResult<UserEntity>(null, $"该账号已存在");
                    }
                }

                UserEntity uEntity = new UserEntity() {
                    Name = entity.Name ?? string.Empty,
                    Account = entity.Account ?? string.Empty,
                    Password = DataEncrypt.DataMd5(entity.Password ?? DEFAULT_PASSWORD),
                    TotalCount = entity.TotalCount > 0 ? entity.TotalCount : 0,
                    CreateDate = DateTime.Now,
                    ModifyDate=DateTime.Now,
                };
                _ = BLL<UserBLL>().Create(uEntity);
                return ApiModel.AsSuccessResult<UserEntity>(entity, $"注册成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion 
    }
}
