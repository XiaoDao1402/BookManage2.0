using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Buss.BLL.Category;
using JW.Data.Entity.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.POIFS.Properties;
using SqlSugar;

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

        #region 删除

        /// <summary>
        /// 批量删除图书分类
        /// </summary>
        /// <param name="bookCategoryIds">要删除的图书分类的集合</param>
        /// <returns></returns>
        [HttpPost("DeleteCategory")]
        public Task<IActionResult> DeleteCategory(List<int> bookCategoryIds) {
            try{
                List<int> delete = new List<int>();
                //List<int> update = new List<int>();
                foreach (var bookCategoryId in bookCategoryIds)
                {
                    CategoryEntity category = BLL<CategoryBLL>().Get<CategoryEntity>(bookCategoryId);
                    if (category.IsNullOrEmpty())
                    {
                        return ApiModel.AsErrorResult<CategoryEntity>(null, $"图书分类不存在");
                    }
                    delete.Append(bookCategoryId);

                    List<CategoryEntity> subclass = BLL<CategoryBLL>().QuerySubclass(bookCategoryId);
                    if (subclass.IsNotNullOrEmpty()) {
                        return ApiModel.AsErrorResult<CategoryEntity>(null, $"图书分类下面有子类，不能删除");
                    }
                }
                if (delete.Count > 0)
                {
                    _ = BLL<CategoryBLL>().DeleteByIds<CategoryEntity>(delete.ToArray());
                }

                return ApiModel.AsSuccessResult<CategoryEntity>(null, "图书分类删除成功");
            }
            catch (Exception ex){
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion
    }
}
