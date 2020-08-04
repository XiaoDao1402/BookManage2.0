using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.Base.Http.Model;
using JW.Buss.BLL.Book;
using JW.Data.Entity.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Book {

    /// <summary>
    /// 图书控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookController : BaseController {

        #region 查询

        /// <summary>
        /// 根据分类id查询图书
        /// </summary>
        /// <param name="bookCategoryId"></param>
        /// <returns></returns>
        [HttpGet("QueryBookByCategoryId/{bookCategoryId}")]
        public Task<IActionResult> QueryBookByCategoryId(int bookCategoryId) {
            try {
                List<BookEntity> list = BLL<BookBLL>().QueryBookByCategoryId(bookCategoryId);
                return ApiModel.AsSuccessResult(list);

            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion
    }
}
