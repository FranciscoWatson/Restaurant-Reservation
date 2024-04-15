﻿namespace RestaurantReservation.API.DTOs.MenuItemDTOs;

public class MenuItemForUpdateDto
{
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}