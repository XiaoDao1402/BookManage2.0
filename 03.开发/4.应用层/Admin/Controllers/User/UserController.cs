using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.Base.Http.Model;
using JW.Buss.BLL.User;
using JW.Data.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers.User {
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : BaseController {
        #region  查询

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="name">用户名</param>
        /// <param name="current">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns></returns>
        [HttpPost("QueryUser")]
        public Task<IActionResult> QueryUser(UserEntity entity) {
            try {
                int total = 0;
                List<UserEntity> list = BLL<UserBLL>().QueryUser(entity.UserId, entity.Name, ref total, entity.Current, entity.PageSize);
                PageModel<UserEntity> result = new PageModel<UserEntity>(list, entity.Current, entity.PageSize, total);
                return ApiModel.AsSuccessResult(result);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }


        #endregion 
    }
}
