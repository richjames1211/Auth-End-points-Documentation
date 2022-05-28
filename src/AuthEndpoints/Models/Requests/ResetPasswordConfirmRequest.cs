﻿using System.ComponentModel.DataAnnotations;

namespace AuthEndpoints.Models;
public class ResetPasswordConfirmRequest
{
    [Required]
    public string? Identity { get; set; }
    [Required]
    public string? Token { get; set; }
    [Required]
    public string? NewPassword { get; set; }
    [Required]
    public string? ConfirmNewPassword { get; set; }
}
