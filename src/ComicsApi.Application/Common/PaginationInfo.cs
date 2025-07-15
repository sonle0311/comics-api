using System;

namespace ComicsApi.Application.Common
{
    /// <summary>
    /// Thông tin phân trang cho API responses
    /// </summary>
    public class PaginationInfo
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Số lượng item trên một trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số item
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Có trang trước hay không
        /// </summary>
        public bool HasPrevious { get; set; }

        /// <summary>
        /// Có trang sau hay không
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public PaginationInfo()
        {
        }

        /// <summary>
        /// Constructor với thông tin phân trang
        /// </summary>
        /// <param name="currentPage">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="totalItems">Tổng số item</param>
        public PaginationInfo(int currentPage, int pageSize, int totalItems)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            HasPrevious = currentPage > 1;
            HasNext = currentPage < TotalPages;
        }

        /// <summary>
        /// Tạo thông tin phân trang
        /// </summary>
        /// <param name="currentPage">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="totalItems">Tổng số item</param>
        /// <returns>Thông tin phân trang</returns>
        public static PaginationInfo Create(int currentPage, int pageSize, int totalItems)
        {
            return new PaginationInfo(currentPage, pageSize, totalItems);
        }
    }
}