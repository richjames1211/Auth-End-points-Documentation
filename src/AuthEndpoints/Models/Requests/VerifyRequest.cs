﻿namespace AuthEndpoints.Models;

/// <summary>
/// the dto used for verify jwt request
/// </summary>
public class VerifyRequest
{
    public string? Token { get; set; }
}
