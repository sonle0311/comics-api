using System;
using System.Collections.Generic;

namespace ComicsApi.Application.Common
{
    /// <summary>
    /// Base class cho tất cả API responses để đảm bảo format thống nhất
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của response data</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Trạng thái thành công của request
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thông điệp mô tả kết quả
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Danh sách lỗi (nếu có)
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Thông tin phân trang (nếu có)
        /// </summary>
        public PaginationInfo? Pagination { get; set; }

        /// <summary>
        /// Timestamp của response
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Constructor cho response thành công với data
        /// </summary>
        /// <param name="data">Dữ liệu trả về</param>
        /// <param name="message">Thông điệp</param>
        public ApiResponse(T data, string message = "Thành công")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        /// <summary>
        /// Constructor cho response lỗi
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="errors">Danh sách lỗi chi tiết</param>
        public ApiResponse(string message, List<string>? errors = null)
        {
            Success = false;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        /// <summary>
        /// Tạo response thành công
        /// </summary>
        /// <param name="data">Dữ liệu trả về</param>
        /// <param name="message">Thông điệp</param>
        /// <returns>ApiResponse thành công</returns>
        public static ApiResponse<T> SuccessResult(T data, string message = "Thành công")
        {
            return new ApiResponse<T>(data, message);
        }

        /// <summary>
        /// Tạo response thành công với phân trang
        /// </summary>
        /// <param name="data">Dữ liệu trả về</param>
        /// <param name="pagination">Thông tin phân trang</param>
        /// <param name="message">Thông điệp</param>
        /// <returns>ApiResponse thành công với phân trang</returns>
        public static ApiResponse<T> SuccessResult(T data, PaginationInfo pagination, string message = "Thành công")
        {
            return new ApiResponse<T>(data, message)
            {
                Pagination = pagination
            };
        }

        /// <summary>
        /// Tạo response lỗi
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="errors">Danh sách lỗi chi tiết</param>
        /// <returns>ApiResponse lỗi</returns>
        public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>(message, errors);
        }

        /// <summary>
        /// Tạo response lỗi với một lỗi duy nhất
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="error">Lỗi chi tiết</param>
        /// <returns>ApiResponse lỗi</returns>
        public static ApiResponse<T> ErrorResult(string message, string error)
        {
            return new ApiResponse<T>(message, new List<string> { error });
        }
    }

    /// <summary>
    /// ApiResponse không có data (chỉ có status và message)
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ApiResponse() : base()
        {
        }

        /// <summary>
        /// Constructor cho response thành công
        /// </summary>
        /// <param name="message">Thông điệp</param>
        public ApiResponse(string message) : base(null, message)
        {
        }

        /// <summary>
        /// Constructor cho response lỗi
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="errors">Danh sách lỗi chi tiết</param>
        public ApiResponse(string message, List<string>? errors) : base(message, errors)
        {
        }

        /// <summary>
        /// Tạo response thành công
        /// </summary>
        /// <param name="message">Thông điệp</param>
        /// <returns>ApiResponse thành công</returns>
        public static new ApiResponse SuccessResult(string message = "Thành công")
        {
            return new ApiResponse(message);
        }

        /// <summary>
        /// Tạo response lỗi
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="errors">Danh sách lỗi chi tiết</param>
        /// <returns>ApiResponse lỗi</returns>
        public static new ApiResponse ErrorResult(string message, List<string>? errors = null)
        {
            return new ApiResponse(message, errors);
        }

        /// <summary>
        /// Tạo response lỗi với một lỗi duy nhất
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="error">Lỗi chi tiết</param>
        /// <returns>ApiResponse lỗi</returns>
        public static new ApiResponse ErrorResult(string message, string error)
        {
            return new ApiResponse(message, new List<string> { error });
        }
    }
}