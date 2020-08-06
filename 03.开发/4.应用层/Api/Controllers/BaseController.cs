using IdentityModel;
using JW.Base.Configuration;
using JW.Base.Lang;
using JW.Base.Security;
using JW.Buss.BLL;
using JW.Buss.BLL.Admin;
using JW.Buss.BLL.User;
using JW.Data.Entity.Admin;
using JW.Data.Entity.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers {
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase {
        #region BaseController

        /// <summary>
        /// 业务层
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T BLL<T>() where T : BaseBLL, new() {
            return new T();
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseController() { }

        #region Session

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        protected void SetSession(string key, string value) {
            HttpContext.Session.SetString(key, value);
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetSession(string key) {
            var value = HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ClearSession() {
            HttpContext.Session.Clear();
        }

        #endregion

        #region Jwt
        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        protected int GetCurrentUserId() {
            int id = -1;

            try {
                id = Convert.ToInt32(User.Claims.Where(it => it.Type == JwtRegisteredClaimNames.Sid).First().Value);
                return id;
            } catch {
                return id;
            }
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        protected UserEntity CurrentUser => BLL<UserBLL>().QueryUserByPrimaryKey(GetCurrentUserId());

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected string GetJwtToken(SessionUser user) {
            string token = string.Empty;

            var claims = new[] {
                //加入用户的名称
                new Claim(JwtClaimTypes.Id,user.Id.ToString()),
                new Claim(JwtClaimTypes.Name,user.Name),
                new Claim(JwtRegisteredClaimNames.Sid,user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.Current.Configuration["JWT:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);

            var twtToken = new JwtSecurityToken(
                issuer: "bestapi",
                audience: "bestapi",
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            token = new JwtSecurityTokenHandler().WriteToken(twtToken);

            return token;
        }
        /// <summary>
        /// 注销登陆
        /// </summary>
        protected void DisponseJwtToken() {
            ClearSession();
        }
        #endregion

    }
    #endregion
}
