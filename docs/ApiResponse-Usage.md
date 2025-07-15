# ApiResponse Usage Guide

## Tổng quan

`ApiResponse<T>` là một lớp cơ sở được thiết kế để chuẩn hóa định dạng phản hồi API trong toàn bộ ứng dụng Comics API. Điều này đảm bảo tính nhất quán và dễ dàng xử lý cho Frontend.

## Cấu trúc ApiResponse

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }
    public PaginationInfo PaginationInfo { get; set; }
    public DateTime Timestamp { get; set; }
}
```

## Cách sử dụng

### 1. Response thành công

```csharp
// Response thành công đơn giản
var response = ApiResponse<MangaDto>.SuccessResult(mangaData, "Lấy thông tin manga thành công");
return Ok(response);

// Response thành công với phân trang
var paginationInfo = PaginationInfo.Create(currentPage, pageSize, totalItems);
var response = ApiResponse<List<MangaDto>>.SuccessResult(mangaList, "Lấy danh sách manga thành công", paginationInfo);
return Ok(response);
```

### 2. Response lỗi

```csharp
// Response lỗi đơn giản
var response = ApiResponse<object>.ErrorResult("Không tìm thấy manga");
return NotFound(response);

// Response lỗi với chi tiết
var response = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
return StatusCode(500, response);

// Response lỗi với nhiều lỗi
var response = ApiResponse<object>.ErrorResult("Dữ liệu không hợp lệ", "Tên không được để trống", "Email không đúng định dạng");
return BadRequest(response);
```

### 3. Sử dụng BaseApiController

```csharp
public class MangaController : BaseApiController
{
    public MangaController(ILogger<MangaController> logger) : base(logger)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetManga(int id)
    {
        try
        {
            var manga = await _mangaService.GetByIdAsync(id);
            
            if (manga == null)
                return NotFoundResponse($"Không tìm thấy manga với ID: {id}");
                
            return SuccessResponse(manga, "Lấy thông tin manga thành công");
        }
        catch (Exception ex)
        {
            return HandleException(ex, $"lấy thông tin manga với ID: {id}");
        }
    }
}
```

### 4. Sử dụng Extension Methods

```csharp
// Thay vì
var response = ApiResponse<MangaDto>.SuccessResult(manga, "Thành công");
return Ok(response);

// Có thể sử dụng
return this.OkApiResponse(manga, "Lấy manga thành công");

// Hoặc với phân trang
return this.OkApiResponse(mangaList, paginationInfo, "Lấy danh sách manga thành công");
```

## Ví dụ Response JSON

### Response thành công

```json
{
  "success": true,
  "message": "Lấy thông tin manga thành công",
  "data": {
    "id": 1,
    "title": "One Piece",
    "author": "Eiichiro Oda"
  },
  "errors": null,
  "paginationInfo": null,
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Response thành công với phân trang

```json
{
  "success": true,
  "message": "Lấy danh sách manga thành công",
  "data": [
    {
      "id": 1,
      "title": "One Piece"
    },
    {
      "id": 2,
      "title": "Naruto"
    }
  ],
  "errors": null,
  "paginationInfo": {
    "currentPage": 1,
    "pageSize": 20,
    "totalItems": 100,
    "totalPages": 5,
    "hasPrevious": false,
    "hasNext": true
  },
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Response lỗi

```json
{
  "success": false,
  "message": "Không tìm thấy manga",
  "data": null,
  "errors": [
    "Manga với ID 999 không tồn tại"
  ],
  "paginationInfo": null,
  "timestamp": "2024-01-15T10:30:00Z"
}
```

## Lợi ích

1. **Tính nhất quán**: Tất cả API endpoints đều trả về cùng một định dạng
2. **Dễ xử lý**: Frontend có thể xử lý response một cách thống nhất
3. **Thông tin phong phú**: Bao gồm timestamp, pagination, và error details
4. **Type-safe**: Sử dụng generics để đảm bảo type safety
5. **Extensible**: Dễ dàng mở rộng thêm các thuộc tính mới

## Best Practices

1. Luôn sử dụng `ApiResponse<T>` cho tất cả API endpoints
2. Cung cấp message có ý nghĩa cho người dùng
3. Sử dụng `BaseApiController` để giảm code duplication
4. Bao gồm thông tin phân trang khi cần thiết
5. Log lỗi chi tiết nhưng chỉ trả về thông tin cần thiết cho client
6. Sử dụng HTTP status codes phù hợp kết hợp với `Success` flag