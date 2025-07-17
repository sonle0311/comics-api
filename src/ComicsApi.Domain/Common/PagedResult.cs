using System;
using System.Collections.Generic;

namespace ComicsApi.Domain.Common
{
    /// <summary>
    /// Lớp đại diện cho kết quả phân trang
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của các item trong trang</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Dữ liệu của trang hiện tại
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Tổng số lượng bản ghi
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Kích thước trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Constructor để tạo PagedResult
        /// </summary>
        public PagedResult(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}