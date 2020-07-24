using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Model {
    /// <summary>
    /// 文件model
    /// </summary>
    public class FileModel {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Url { get; set; }
    }
}
