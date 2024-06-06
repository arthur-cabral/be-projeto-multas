﻿using Application.DTO.JWT;
using Application.Interfaces;
using Application.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AccessTokenResponseDTO> Login(LoginModelDTO loginModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName!);

                if (user is not null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                    await _userManager.UpdateAsync(user);

                    return new AccessTokenResponseDTO
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        Expiration = token.ValidTo
                    };
                }
                else
                {
                    throw new Exception("User or password invalid");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Register(RegisterModelDTO registerModel)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(registerModel.UserName!);
                if (userExists != null)
                {
                    throw new Exception("User already exists!");
                }

                ApplicationUser user = new()
                {
                    Email = registerModel.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerModel.UserName,
                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (!result.Succeeded)
                {
                    throw new Exception("User creation failed!");
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AccessTokenResponseDTO> RefreshToken(TokenModelDTO tokenModel)
        {
            try
            {
                if (tokenModel is null)
                {
                    throw new Exception("Invalid client request");
                }

                string? accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
                string? refreshToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));

                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);
                if (principal == null)
                {
                    throw new Exception("Invalid access token/refresh token");
                }

                string username = principal.Identity.Name;
                var user = await _userManager.FindByNameAsync(username!);
                if (user == null || user.RefreshToken != refreshToken
                    || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    throw new Exception("Invalid access token/refresh token");
                }

                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = newRefreshToken;

                await _userManager.UpdateAsync(user);

                return new AccessTokenResponseDTO
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    RefreshToken = newRefreshToken,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Revoke(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) throw new Exception("Invalid userName");
                user.RefreshToken = null;

                await _userManager.UpdateAsync(user);
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}