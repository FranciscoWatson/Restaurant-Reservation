﻿namespace RestaurantReservation.API.DTOs.OrderItemDTOs;

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}