﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthEndpoints.Services;

/// <summary>
/// A default service that provide claims for access Token
/// </summary>
/// <typeparam name="TUserKey"></typeparam>
/// <typeparam name="TUser"></typeparam>
public class AccessClaimsProvider<TUserKey, TUser> : IAccessClaimsProvider<TUser>
    where TUserKey : IEquatable<TUserKey>
    where TUser : IdentityUser<TUserKey>
{
    /// <summary>
    /// Use this method to get list of claims for an access Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns>A list of claims</returns>
    public IList<Claim> provideClaims(TUser user)
    {
        return new List<Claim>()
        {
            new Claim("id", user.Id.ToString()!),
            new Claim(ClaimTypes.Name, user.UserName),
        };
    }
}
