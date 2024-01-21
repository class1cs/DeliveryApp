namespace DeliveryApp.DAL;

public class Item
{
    public Guid Id { get; set; }
    
    public Product Product { get; set; }
    
    public long Count { get; set; }
    
    public decimal TotalSum { get; set; }
}