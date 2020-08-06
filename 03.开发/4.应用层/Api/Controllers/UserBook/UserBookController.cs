using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Buss.BLL.UserBook;
using JW.Data.Entity.UserBook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserBook {

    /// <summary>
    /// 借书记录控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookController : BaseController {

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
        /// 根据借书记录id修改借书记录信息
        /// </summary>
        /// <param name="userBookId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("UpdateBook/{userBookId}")]
        public Task<IActionResult> UpdateUserBook(int userBookId, UserBookEntity entity) {
            try {
                UserBookEntity userBook = BLL<UserBookBLL>().Get<UserBookEntity>(userBookId);
                if (userBook.IsNullOrEmpty()) {
                    return ApiModel.AsSuccessResult<UserBookEntity>(null, $"借书记录不存在");
                }

                userBook.State = entity.State > 0 ? entity.State : 0;
                userBook.ReturnDate = DateTime.Now.ToString();

                _ = BLL<UserBookBLL>().Update<UserBookEntity>(userBook);
                return ApiModel.AsSuccessResult<UserBookEntity>(entity, $"还书成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }
    }

    
}
