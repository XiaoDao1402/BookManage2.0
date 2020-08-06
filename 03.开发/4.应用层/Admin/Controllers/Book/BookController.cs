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
using JW.Data.Entity.Category;
using JW.Buss.BLL.Category;
using JW.Base.Lang;
using JW.Data.Entity.Book;

namespace Admin.Controllers.Book
{
    /// <summary>
    /// 图书控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : BaseController
    {
        #region 查询

        /// <summary>
        /// 查询图书
        /// </summary>
        /// <param name="entity">查询参数实体</param>
        /// <returns></returns>
        [HttpPost("QueryBook")]
        public Task<IActionResult> QueryBook(PageQueryBookEntity entity)
        {
            try
            {
                int total = 0;
                List<int> categoryIds = new List<int>();
                if (entity.IsNotNullOrEmpty() && entity.BookCategoryId.IsNotNullOrEmpty())
                {
                    categoryIds.Add((int)entity.BookCategoryId);
                    List<BookCategoryEntity> categoryList = BLL<BookCategoryBLL>()
                      .Get<BookCategoryEntity>().Where(it => it.ParentId == entity.BookCategoryId).ToList();
                    categoryList.ForEach(it =>
                    {
                        categoryIds.Add(it.BookCategoryId);
                    });
                }
                List<BookEntity> list = BLL<BookBLL>().QueryBook(categoryIds.ToArray(), entity, ref total);

                PageModel<BookEntity> result = new PageModel<BookEntity>(list, entity.Current, entity.PageSize, total);
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
        /// 批量删除图书
        /// </summary>
        /// <param name="bookIds"></param>
        /// <returns></returns>
        [HttpPost("DeleteBook")]
        public Task<IActionResult> DeleteBook(List<int> bookIds)
        {
            try
            {
                List<int> delete = new List<int>();
                foreach (var bookId in bookIds)
                {
                    BookEntity book = BLL<BookBLL>().Get<BookEntity>(bookId);
                    if (book.IsNullOrEmpty())
                    {
                        return ApiModel.AsErrorResult<BookEntity>(null, $"图书不存在");
                    }
                    delete.Append(bookId);
                }
                if (delete.Count > 0)
                {
                    _ = BLL<BookBLL>().DeleteByIds<BookEntity>(delete.ToArray());
                }
                return ApiModel.AsSuccessResult<BookEntity>(null, $"删除成功");
            }
            catch (Exception ex)
            {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增图书
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("AddBook")]
        public Task<IActionResult> AddBook(BookEntity entity)
        {
            try
            {
                if (entity.IsNullOrEmpty())
                {
                    return ApiModel.AsErrorResult<BookEntity>(null, $"参数有误");
                }
                if (entity.Name.IsNotNullOrEmpty())
                {
                    BookEntity book = BLL<BookBLL>().QueryBookByName(entity.Name);
                    if (book.IsNotNullOrEmpty())
                    {
                        return ApiModel.AsErrorResult<BookEntity>(null, $"图书已存在");
                    }
                }
                BookEntity bEntity = new BookEntity()
                {
                    Name = entity.Name ?? string.Empty,
                    BookCategoryId = entity.BookCategoryId >= 10000 ? entity.BookCategoryId : 0,
                    CoverImage = entity.CoverImage ?? string.Empty,
                    Price = entity.Price >= 0 ? entity.Price : 0,
                    BorrowNum = entity.BorrowNum >= 0 ? entity.BorrowNum : 0,
                    TotalStockCount = entity.TotalStockCount >= 0 ? entity.TotalStockCount : 0,
                    SurplusStockCount = entity.SurplusStockCount >= 0 ? entity.SurplusStockCount : 0,
                    AdminId = entity.AdminId >= 10000 ? entity.AdminId : 0,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,

                };
                _ = BLL<BookBLL>().Create(bEntity);
                return ApiModel.AsSuccessResult<BookEntity>(entity, $"新增成功");
            }
            catch (Exception ex)
            {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改图书
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("UpdateBook/{bookId}")]
        public Task<IActionResult> UpdateBook(int bookId, BookEntity entity)
        {
            try
            {
                BookEntity book = BLL<BookBLL>().Get<BookEntity>(bookId);
                if (book.IsNullOrEmpty())
                {
                    return ApiModel.AsErrorResult<BookEntity>(null, $"图书不存在");
                }
                BookEntity bookName = BLL<BookBLL>().QueryBookByName(entity.Name);
                if (bookName.IsNotNullOrEmpty()) {
                    return ApiModel.AsErrorResult<BookEntity>(null, $"该图书名已存在");
                }
                book.Name = entity.Name ?? string.Empty;
                book.BookCategoryId = entity.BookCategoryId >= 10000 ? entity.BookCategoryId : 0;
                book.CoverImage = entity.CoverImage ?? string.Empty;
                book.Price = entity.Price >= 0 ? entity.Price : 0;
                book.BorrowNum = entity.BorrowNum >= 0 ? entity.BorrowNum : 0;
                book.TotalStockCount = entity.TotalStockCount >= 0 ? entity.TotalStockCount : 0;
                book.SurplusStockCount = entity.SurplusStockCount >= 0 ? entity.SurplusStockCount : 0;
                book.AdminId = entity.AdminId >= 10000 ? entity.AdminId : 0;
                book.ModifyDate = DateTime.Now;

                _ = BLL<BookBLL>().Update<BookEntity>(book);
                return ApiModel.AsSuccessResult<BookEntity>(entity, $"修改成功");
            }

            catch (Exception ex)
            {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion 
    }
}
