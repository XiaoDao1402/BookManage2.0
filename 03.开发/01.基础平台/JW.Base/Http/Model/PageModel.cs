using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Base.Http.Model {
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageModel<T> {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { set; get; }

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 是否上一页
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// 是否下一页
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// 是否首页
        /// </summary>
        public bool IsFirstPage { get; set; }

        /// <summary>
        /// 是否末页
        /// </summary>
        public bool IsLastPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int FirstItemOnPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int LastItemOnPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Data { set; get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        public PageModel(IEnumerable<T> list, int pageNumber, int pageSize, int total) {
            Data = list;
            Total = total;
            PageNumber = pageNumber;
            PageSize = pageSize;

            if (pageSize > 0 && pageNumber > 0 && total > 0) {
                PageCount = (total + pageSize - 1) / pageSize;

                if (pageNumber > 1 && PageCount > 2) {
                    HasPreviousPage = true;
                }

                if (pageNumber < PageCount) {
                    HasNextPage = true;
                }

                if (pageNumber == 1) {
                    IsFirstPage = true;
                }

                if (pageNumber == PageCount) {
                    IsLastPage = true;
                }

                FirstItemOnPage = ((pageNumber - 1) * pageSize + 1);
                LastItemOnPage = IsLastPage ? total : (pageNumber * pageSize);
            }
        }
    }
}
