using DeliveryApp.DAL;

namespace DeliveryApp.API.Dtos;

public class OrderResponseDto
{
    public Guid Id { get; set; }

    public List<Item> Items { get; set; } = new List<Item>();
    
    public DateTime DeliveryDate { get; set; }
    
    public string Requests { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public List<Guid> UserIds { get; set; }
}