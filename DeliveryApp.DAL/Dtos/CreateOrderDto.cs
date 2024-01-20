using DeliveryApp.DAL;

namespace DeliveryApp.API.Dtos;

public class CreateOrderDto
{
    public List<ItemDto> Items { get; set; } = new ();
    
    public string Requests { get; set; }
    
    public DateTime DeliveryDate { get; set; }
}