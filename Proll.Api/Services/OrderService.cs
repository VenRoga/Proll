using Proll.Api.Models.BaseModelsContext;
using Proll.Api.Models.BaseModels;
using Proll.Shared.Dtos;
using Microsoft.EntityFrameworkCore;


namespace Proll.Api.Services
{
    public class OrderService
    {
        private readonly BaseModelContext _context;
        public OrderService(BaseModelContext context)
        {
            _context = context;
        }
        public async Task<ApiResult> PlaceOrderAsync(PlaceOrderDto dto, int userId)
        {
            if(dto.Items.Length == 0)
            {
                return ApiResult.Fail("Order must contian items");
            }
            var productIds = dto.Items.Select(i => i.Id).ToHashSet();
            var products = await _context.Products
                .Where(p => productIds
                .Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            if(products.Count != dto.Items.Length)
            {
                return ApiResult.Fail("Some product is not avaiable");
            }

            var orderItems = dto.Items
                .Select(i => new OrderItem
            { 
                ProductId = i.Id,
                Quantity = i.Quantity,
                ProductImageUrl = products[i.Id].ImageUrl,
                ProductName = products[i.Id].Name,
                ProductPrice= products[i.Id].Price,
                Unit = products[i.Id].Unit,
            }).ToArray();

            var now = DateTime.UtcNow;
            var order = new Order
            { 
                Date = now,
                UserId = userId,
                UserAddressId = dto.UserAddressId,
                Address = dto.AddressName,
                TotalItems = dto.Items.Length,
                TotalAmount = orderItems.Sum(oi => oi.Quantity * oi.ProductPrice),
                Items = orderItems
            };

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return ApiResult.Success();
            }
            catch(Exception ex) 
            {
                return ApiResult.Fail(ex.Message);
            }

        }
        public async Task<AddressDto[]> GetUserOrdersAsync(int userId, int startIndex, int pageSize) => 
            await _context.UserAddresses
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Id)
                .Skip(startIndex)
                .Take(pageSize)
                .Select(a => new AddressDto
                {
                    Id = a.Id,
                    Address = a.Address,
                    IsDefault = a.IsDefault,
                    Name = a.Name
                })
                .ToArrayAsync();
        public async Task<ApiResult<OrderItemDto[]>> GetUserOrderItemAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if(order == null)
            {
                return ApiResult<OrderItemDto[]>.Fail("Order not found");
            }

            if(order.UserId != userId)
            {
                return ApiResult<OrderItemDto[]>.Fail("Order not your or not found");
            }

            var  items = order.Items
                .Select(oi => new OrderItemDto
                    { 
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductImageUrl = oi.ProductImageUrl,
                        ProductName = oi.ProductName,
                        ProductPrice =  oi.ProductPrice,
                        Quantity = oi.Quantity,
                        Unit = oi.Unit
                    })
                .ToArray();
            return ApiResult<OrderItemDto[]>.Success(items);
        }
    }
}