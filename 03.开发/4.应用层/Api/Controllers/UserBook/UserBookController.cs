using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Buss.BLL.Book;
using JW.Buss.BLL.UserBook;
using JW.Data.Entity.Book;
using JW.Data.Entity.UserBook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;

namespace Api.Controllers.UserBook {

    /// <summary>
    /// 借书记录控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookController : BaseController {

        #region 查询

        /// <summary>
        /// 根据用户id查询该用户的借书记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getUserBook/{userId}")]
        public Task<IActionResult> getUserBook(int userId) {
            try {
                List<UserBookEntity> list = BLL<UserBookBLL>().GetUserBook(userId);
                return ApiModel.AsSuccessResult(list);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 根据用户id查询该用户的还书记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetReturnUserBook/{userId}")]
        public Task<IActionResult> GetReturnUserBook(int userId) {
            try {
                List<UserBookEntity> list = BLL<UserBookBLL>().GetReturnUserBook(userId);
                return ApiModel.AsSuccessResult(list);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 根据图书id查询当前用户是否有借此书
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("GetBorrowBook/{bookId}&{userId}")]
        public Task<IActionResult> GetBorrowBook(int bookId, int userId) {
            try {
                if (bookId.IsNullOrEmpty() && userId.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<UserBookEntity>(null, $"参数有误");
                }
                List<UserBookEntity> list = BLL<UserBookBLL>().GetBorrowBook(bookId, userId);
                return ApiModel.AsSuccessResult(list);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        #endregion

        #region 修改

        /// <summary>
        /// 还书
        /// </summary>
        /// <param name="userBookId"></param>
        /// <param name="bookId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("UpdateBook/{userBookId}&{bookId}")]
        public Task<IActionResult> UpdateUserBook(int userBookId,int bookId, UserBookEntity entity) {
            try {
                UserBookEntity userBook = BLL<UserBookBLL>().Get<UserBookEntity>(userBookId);
                if (userBook.IsNullOrEmpty()) {
                    return ApiModel.AsSuccessResult<UserBookEntity>(null, $"借书记录不存在");
                }

                userBook.State = entity.State > 0 ? entity.State : 0;
                userBook.ReturnDate = DateTime.Now.ToString();

                BookEntity book = BLL<BookBLL>().Get<BookEntity>(bookId);
                if (book.IsNotNullOrEmpty()) {
                    book.SurplusStockCount = book.SurplusStockCount + 1;
                    _ = BLL<BookBLL>().Update<BookEntity>(book);
                }

                _ = BLL<UserBookBLL>().Update<UserBookEntity>(userBook);
                return ApiModel.AsSuccessResult<UserBookEntity>(entity, $"还书成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }
    #endregion

    #region 新增

    /// <summary>
    /// 借书
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost("AddUserBook")]
    public Task<IActionResult> AddUserBook(UserBookEntity entity) {
        try {
            if (entity.IsNullOrEmpty()) {
                return ApiModel.AsErrorResult<UserBookEntity>(null, $"参数有误");
            }
            UserBookEntity userBook = new UserBookEntity() {
                UserId = entity.UserId > 10000 ? entity.UserId : 0,
                BookId = entity.BookId > 10000 ? entity.BookId : 0,
                State = 1,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                ReturnDate = string.Empty
            };
            BookEntity book = BLL<BookBLL>().Get<BookEntity>(entity.BookId);
            if (book.IsNotNullOrEmpty()) {
                book.BorrowNum = book.BorrowNum + 1;
                book.SurplusStockCount = book.SurplusStockCount - 1;
                _ = BLL<BookBLL>().Update<BookEntity>(book);
            }

            _ = BLL<UserBookBLL>().Create(userBook);
            return ApiModel.AsSuccessResult<UserBookEntity>(entity, "借书成功");

        } catch (Exception ex) {
            return ApiModel.AsExceptionResult(ex);
        }
    }

    #endregion
}


}
