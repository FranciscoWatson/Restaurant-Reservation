﻿namespace RestaurantReservation.API.Authentication;

public class AuthenticationRequestBody
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}