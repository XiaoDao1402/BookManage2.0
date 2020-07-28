using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Buss.BLL.UserBook;
using JW.Data.Entity.Models;
using JW.Data.Entity.UserBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers.UserBook
{
    /// <summary>
    /// 用户借书记录控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserBookController : BaseController
    {
        #region 查询

        /// <summary>
        /// 查询用户借书记录
        /// </summary>
        /// <param name="entity">查询参数</param>
        /// <returns></returns>
        [HttpPost("QueryUserBook")]
        public Task<IActionResult> QueryUserBook(PageQueryUserBookEntity entity) {
            try
            {
                int total = 0;
                if (entity.IsNotNullOrEmpty() && entity.CreateDate.IsNotNullOrEmpty()) {
                    entity.CreateDateStart = Convert.ToDateTime(entity.CreateDate[0]);
                    entity.CreateDateEnd = Convert.ToDateTime(entity.CreateDate[1]);
                }

                List<UserBookEntity> list = BLL<UserBookBLL>().QueryUserBook(entity, ref total);
                PageModel<UserBookEntity> result = new PageModel<UserBookEntity>(list,entity.Current, entity.PageSize, total);
                return ApiModel.AsSuccessResult(result);
            }
            catch (Exception ex)
            {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion 
    }
}
