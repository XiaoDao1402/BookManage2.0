using Admin.Model;
using JW.Base.Http.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using JW.Base.Qiniuyun;
using JW.Base.Qiniuyun.Config;
using JW.Base.Configuration;

namespace Admin.Controllers.Upload {
    /// <summary>
    /// 文件上传
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UploadController : ControllerBase {

        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadController(){
            QiniuConfig config = new QiniuConfig() {
                AK = ConfigurationManager.Current.Configuration["Qiniu:AK"],
                SK = ConfigurationManager.Current.Configuration["Qiniu:SK"],
                Bucket = ConfigurationManager.Current.Configuration["Qiniu:Bucket"],
                Domain = ConfigurationManager.Current.Configuration["Qiniu:Domain"],
            };
            QiniuConfigManager.Current.SetConfig(config);
        }

        /// <summary>
        /// 上传文件接口
        /// </summary>
        /// <param name="fileType">文件类型 image 或者 video</param>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [SwaggerResponse(200, "文档注释", typeof(ApiModel<List<FileModel>>))]
        [HttpPost]
        public Task<IActionResult> Post(string fileType, IFormFile file) {
            try {
                FileModel result = new FileModel();

                if (file.Length == 0) {
                    return ApiModel.AsErrorResult<FileModel>(null, "上传文件错误请新选择");
                }

                Stream stream = file.OpenReadStream();
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                string suffix = string.Empty;
                suffix = Path.GetExtension(file.FileName);
                stream.Seek(0, SeekOrigin.Begin);

                result.FileName = file.FileName;

                switch (fileType) {
                    case "image":
                        Image image = Image.FromStream(stream);
                        result.Url =  QiniuApiContext.Current.UploadApi.UploadByByte(bytes, suffix) + $"?{image.Width}x{image.Height}";
                        break;
                    case "video":
                        result.Url = QiniuApiContext.Current.UploadApi.UploadByByte(bytes, suffix);
                        break;
                    default:
                        return ApiModel.AsErrorResult(result, "上传文件类型未知");
                }

                return ApiModel.AsSuccessResult(result, "上传成功");
            } catch (Exception ex) {
                return ApiModel.AsExceptionResult(ex);
            }
        }

    }
}