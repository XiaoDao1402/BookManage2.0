using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.Base.Http.Model;
using JW.Buss.BLL.Category;
using JW.Data.Entity.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Category {

    /// <summary>
    /// 图书分类控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookCategoryController : BaseController {

        #region 查询

        /// <summary>
        /// 查询所有父级分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryBookCategoryParent")]
        public Task<IActionResult> QueryBookCategoryParent() {
            try {
                List<BookCategoryEntity> list = BLL<BookCategoryBLL>().QueryBookCategoryParent();
                return ApiModel.AsSuccessResult(list);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 根据父类id查询子类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("QuerySubclass/{parentId}")]
        public Task<IActionResult> QuerySubclass(int parentId) {
            try {
                List<BookCategoryEntity> list = BLL<BookCategoryBLL>().QuerySubclass(parentId);
                return ApiModel.AsSuccessResult(list);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion
    }
}
