﻿namespace RestaurantReservation.API.DTOs.EmployeeDTOs;

public class EmployeeForCreationDto
{
    public int RestaurantId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
}