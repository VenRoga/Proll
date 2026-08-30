using System.Security.Cryptography.X509Certificates;

public record PlaceOrderDto(int UserAddressId, string AddressName, OrderItemSaveDto[] Items);
