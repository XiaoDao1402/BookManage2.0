using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Model;
using JW.Base.Http.Model;
using JW.Base.Lang;
using JW.Base.Lang.Encrypt;
using JW.Buss.BLL.Admin;
using JW.Data.Entity.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers.Admin {
    /// <summary>
    /// 管理员控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : BaseController {
        #region 字段

        private const string DEFAULT_PASSWORD = "000000";

        #endregion

        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public Task<IActionResult> CreateAdmin(AdminEntity entity) {
            try {
                if (entity.Account.IsNotNullOrEmpty()) {
                    AdminEntity exist = BLL<AdminBLL>().QueryAdminByAccount(entity.Account);
                    if (exist.IsNotNullOrEmpty()) {
                        return ApiModel.AsErrorResult(entity, $"管理员账户已存在,{entity.Account}");
                    }
                }

                AdminEntity admin = new AdminEntity() {
                    Account = entity.Account,
                    Password = DataEncrypt.DataMd5(entity.Password ?? DEFAULT_PASSWORD),
                    Name = entity.Name ?? string.Empty,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                };

                _ = BLL<AdminBLL>().Create(admin);

                return ApiModel.AsSuccessResult(entity, "添加成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("Modify/{adminId}")]
        public Task<IActionResult> ModifyAdmin(int adminId, AdminEntity entity) {
            try {
                AdminEntity admin = BLL<AdminBLL>().Get<AdminEntity>(adminId);

                if (admin.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult(entity, $"管理员不存在");
                }

                admin.Name = entity.Name ?? string.Empty;
                admin.Account = entity.Account ?? string.Empty;
                admin.ModifyDate = DateTime.Now;

                _ = BLL<AdminBLL>().Update(admin);

                return ApiModel.AsSuccessResult(entity);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }
        /// <summary>
        /// 查询管理员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="current "></param>
        /// <param name="pageSize"></param>
        /// <param name="sorter"></param>
        /// <returns></returns>
        [HttpGet("Query")]
        public Task<IActionResult> QueryAdmins(string name, int current = 1, int pageSize = 10, string sorter = null) {
            try {
                int total = 0;
                List<AdminEntity> list = BLL<AdminBLL>().QueryAdmin(name, ref total, current, pageSize);

                PageModel<AdminEntity> result = new PageModel<AdminEntity>(list, current, pageSize, total);

                return ApiModel.AsSuccessResult(result);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="adminIds"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public Task<IActionResult> DeleteAdmin(List<int> adminIds) {
            try {
                List<int> delete = new List<int>();
                foreach (int adminId in adminIds) {
                    AdminEntity admin = BLL<AdminBLL>().Get<AdminEntity>(adminId);
                    if (admin.IsNullOrEmpty()) {
                        return ApiModel.AsErrorResult<AdminEntity>(null, $"管理员不存在");
                    }

                    if (CurrentAdmin.AdminId == adminId) {
                        return ApiModel.AsErrorResult<AdminEntity>(admin, $"不能删除自己,{admin.Name}");
                    }

                   
                    delete.Append(adminId);
                }
                if (delete.Count > 0) {
                    _ = BLL<AdminBLL>().DeleteByIds<AdminEntity>(delete.ToArray());
                }

                return ApiModel.AsSuccessResult<AdminEntity>(null, "删除管理员成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpGet("Reset/{adminId}")]
        public Task<IActionResult> ResetPassword(int adminId) {
            try {
                AdminEntity admin = BLL<AdminBLL>().Get<AdminEntity>(adminId);

                if (admin.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult<AdminEntity>(null, $"管理员{adminId}不存在");
                }

                admin.Password = DataEncrypt.DataMd5(DEFAULT_PASSWORD);
                admin.ModifyDate = DateTime.Now;

                _ = BLL<AdminBLL>().Update(admin);

                return ApiModel.AsSuccessResult<AdminEntity>(null, "密码已重置");

            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }


        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("ModifyInfo/{adminId}")]
        public Task<IActionResult> ModifyAdminInfo(int adminId, AdminEntity entity) {
            try {
                AdminEntity admin = BLL<AdminBLL>().Get<AdminEntity>(adminId);

                if (admin.IsNullOrEmpty()) {
                    return ApiModel.AsErrorResult(entity, $"管理员不存在");
                }

                admin.Name = entity.Name ?? admin.Name;
                admin.ModifyDate = DateTime.Now;

                _ = BLL<AdminBLL>().Update<AdminEntity>(admin, it => new { it.Name, it.ModifyDate });

                return ApiModel.AsSuccessResult(entity);
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

    }
}