﻿using System;
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
using Newtonsoft.Json;
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
    public class BookCategoryController : BaseController
    {
        #region 查询

        /// <summary>
        /// 查询图书分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="current"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("QueryBookCategory")]
        public Task<IActionResult> QueryBookCategory(string name, int current = 1, int pageSize = 10)
        {
            try
            {
                int total = 0;
                List<BookCategoryEntity> list = BLL<BookCategoryBLL>().QueryBookCategory(name, ref total, current, pageSize);
                PageModel<BookCategoryEntity> result = new PageModel<BookCategoryEntity>(list, current, pageSize, total);
                return ApiModel.AsSuccessResult(result);
            }
            catch (Exception ex)
            {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 查询所有图书分类（用于树形下拉列表）
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryBookCategoryTree")]
        [AllowAnonymous]
        public Task<IActionResult> QueryBookCategoryTree() {
            try
            {
                List<BookCategoryEntity> list = BLL<BookCategoryBLL>().QueryBookCategoryTree();
                return ApiModel.AsSuccessResult(list);
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
        [HttpPost("DeleteBookCategory")]
        public Task<IActionResult> DeleteBookCategory(List<int> bookCategoryIds)
        {
            try
            {
                List<int> delete = new List<int>();
                foreach (var bookCategoryId in bookCategoryIds)
                {
                    BookCategoryEntity category = BLL<BookCategoryBLL>().Get<BookCategoryEntity>(bookCategoryId);
                    if (category.IsNullOrEmpty())
                    {
                        return ApiModel.AsErrorResult<BookCategoryEntity>(null, $"图书分类不存在");
                    }
                    delete.Append(bookCategoryId);

                    List<BookCategoryEntity> subclass = BLL<BookCategoryBLL>().QuerySubclass(bookCategoryId);
                    if (subclass.IsNotNullOrEmpty())
                    {
                        return ApiModel.AsErrorResult<BookCategoryEntity>(null, $"图书分类下面有子类，不能删除");
                    }
                }
                if (delete.Count > 0)
                {
                    _ = BLL<BookCategoryBLL>().DeleteByIds<BookCategoryEntity>(delete.ToArray());
                }

                return ApiModel.AsSuccessResult<BookCategoryEntity>(null, "图书分类删除成功");
            }
            catch (Exception ex)
            {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增图书分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("AddBookCategory")]
        public Task<IActionResult> AddBookCategory(BookCategoryEntity entity)
        {
            try
            {
                if (entity.Name.IsNotNullOrEmpty())
                {
                    BookCategoryEntity category = BLL<BookCategoryBLL>().QueryBookCategoryByName(entity.Name);
                    if (category.IsNotNullOrEmpty())
                    {
                        return ApiModel.AsErrorResult<BookCategoryEntity>(null, "该图书分类已存在");
                    }
                }
                //BookCategoryEntity parant = BLL<BookCategoryBLL>().QueryBookCategoryByName(entity.Parent.Name);

                BookCategoryEntity bookCategory = new BookCategoryEntity()
                {
                    Name = entity.Name,
                    ParentId = entity.ParentId,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                };

                _ = BLL<BookCategoryBLL>().Create(bookCategory);
                return ApiModel.AsSuccessResult(entity, "新增成功");
            }
            catch (Exception ex)
            {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion 
    }
}
