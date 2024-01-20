namespace DeliveryApp.DAL;

public class Order
{
    public Guid Id { get; set; }
    
    public virtual List<Item> Items { get; set; } 
    
    public DateTime DeliveryDate { get; set; }
    
    public string Requests { get; set; }
    
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    Accepted,
    Free,
    Done,
    Canceled
}