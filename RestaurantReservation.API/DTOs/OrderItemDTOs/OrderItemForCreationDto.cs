﻿namespace RestaurantReservation.API.DTOs.OrderItemDTOs;

public class OrderItemForCreationDto
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}