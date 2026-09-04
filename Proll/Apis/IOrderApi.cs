using Proll.Shared.Dtos;
using Refit;

[Headers("Authorization: Bearer ")]
public interface IOrderApi
{
    [Post("/api/orders/place-order")]
    Task<ApiResult> PlaceOrderAsync(PlaceOrderDto dto);
    [Get("/api/orders/user/{userId}")]
    Task<OrderDto[]> GetUserOrdersAsync(int userId, int startIndex, int pageSize);
    [Get("/api/orders/users/{userId:int}/orders/{orderId}/items")]
    Task<ApiResult<OrderItemDto[]>> GetUserOrderItemsAsync(int orderId, int userId);
}
