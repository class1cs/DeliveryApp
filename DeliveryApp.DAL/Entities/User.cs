﻿namespace DeliveryApp.DAL;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string SecondName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string PasswordHash { get; set; }

    public List<Order> Orders { get; set; } = new();
    
    public string Login { get; set; }
    
    public Role Role { get; set; }
}