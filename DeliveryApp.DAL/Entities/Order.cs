namespace DeliveryApp.DAL;

public class Order
{
    public Guid Id { get; set; }
    
    public long Number { get; set; }
    
    public User User { get; set; }
    
    public DateTime DeliveryDate { get; set; }
    
    public List<Item> Items { get; set; }
    
}