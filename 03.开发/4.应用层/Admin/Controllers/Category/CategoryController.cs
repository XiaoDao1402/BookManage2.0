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

namespace Admin.Controllers.Category
{
    /// <summary>
    /// 图书分类控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        #region 查询

        /// <summary>
        /// 查询图书分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="current"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("QueryCategory")]
        public Task<IActionResult> QueryCategory(string name,int current = 1,int pageSize=10) {
            try
            {
                int total = 0;
                List<CategoryEntity> list = BLL<CategoryBLL>().QueryCategory(name,ref total, current, pageSize);
                PageModel<CategoryEntity> result = new PageModel<CategoryEntity>(list, current, pageSize, total);
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
