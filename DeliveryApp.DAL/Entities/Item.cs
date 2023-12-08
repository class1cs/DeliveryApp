namespace DeliveryApp.DAL;

public class Item
{
    public Item(List<Product> products)
    {
        Products = products;
        TotalSum = products.Sum(x => x.Cost);
    }

    public Guid Id { get; set; }
    
    public List<Product> Products { get; set; }

    public decimal TotalSum { get; set; }
    
    
}