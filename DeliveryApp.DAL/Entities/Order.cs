using System.Text.Json.Serialization;

namespace DeliveryApp.DAL;

public class Order
{
    public Guid Id { get; set; }

    public virtual List<Item> Items { get; set; } = new List<Item>();
    
    public DateTime DeliveryDate { get; set; }
    
    public string Requests { get; set; }
    
    public OrderStatus Status { get; set; }

    public List<User> Users { get; set; } = new List<User>();
}

public enum OrderStatus
{
    Accepted,
    Free,
    Done,
    Canceled
}