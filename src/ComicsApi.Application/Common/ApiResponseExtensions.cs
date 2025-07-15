using Microsoft.AspNetCore.Mvc;

namespace ComicsApi.Application.Common
{
    /// <summary>
    /// Extension methods để đơn giản hóa việc tạo ApiResponse trong controllers
    /// </summary>
    public static class ApiResponseExtensions
    {
        /// <summary>
        /// Tạo OkObjectResult với ApiResponse thành công
        /// </summary>
        public static OkObjectResult OkApiResponse<T>(this ControllerBase controller, T data, string message = "Thành công")
        {
            var response = ApiResponse<T>.SuccessResult(data, message);
            return controller.Ok(response);
        }

        /// <summary>
        /// Tạo OkObjectResult với ApiResponse thành công có phân trang
        /// </summary>
        public static OkObjectResult OkApiResponse<T>(this ControllerBase controller, T data, PaginationInfo paginationInfo, string message = "Thành công")
        {
            var response = ApiResponse<T>.SuccessResult(data, message, paginationInfo);
            return controller.Ok(response);
        }

        /// <summary>
        /// Tạo BadRequestObjectResult với ApiResponse lỗi
        /// </summary>
        public static BadRequestObjectResult BadRequestApiResponse(this ControllerBase controller, string message, params string[] errors)
        {
            var response = ApiResponse<object>.ErrorResult(message, errors);
            return controller.BadRequest(response);
        }

        /// <summary>
        /// Tạo NotFoundObjectResult với ApiResponse lỗi
        /// </summary>
        public static NotFoundObjectResult NotFoundApiResponse(this ControllerBase controller, string message)
        {
            var response = ApiResponse<object>.ErrorResult(message);
            return controller.NotFound(response);
        }

        /// <summary>
        /// Tạo ObjectResult với status code 500 và ApiResponse lỗi
        /// </summary>
        public static ObjectResult InternalServerErrorApiResponse(this ControllerBase controller, string message, string errorDetail = null)
        {
            var response = ApiResponse<object>.ErrorResult(message, errorDetail);
            return controller.StatusCode(500, response);
        }
    }
}