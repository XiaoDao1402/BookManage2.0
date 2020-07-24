using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JW.Data.Entity.Models;
using JW.Base.Http.Model;
using JW.Buss.BLL.Book;

namespace Admin.Controllers.Book
{
    /// <summary>
    /// 图书控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookController : BaseController
    {
        #region 查询

        /// <summary>
        /// 查询图书
        /// </summary>
        /// <param name="entity">查询参数实体</param>
        /// <returns></returns>
        [HttpPost("QueryBook")]
        public Task<IActionResult> QueryBook(PageQueryEntity entity) {
            try
            {
                int total = 0;
                List<BookEntity> list = BLL<BookBLL>().QueryBook(entity, ref total);
                PageModel<BookEntity> result = new PageModel<BookEntity>(list, entity.Current, entity.PageSize, total);
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
