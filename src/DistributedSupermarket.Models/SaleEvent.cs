namespace DistributedSupermarket.Models;

public record SaleEvent(
    int SaleId,
    string StoreId,
    List<SaleItem> Items,
    DateTime Timestamp,
    decimal TotalAmount
    );

public record SaleItem(string Sku, int Quantity);
