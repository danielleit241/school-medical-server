using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AuthService(SchoolMedicalManagementContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<TokensResponse?> LoginAsync(UserLoginRequest request)
        {
            var user = await context.Users.Include("Role").Where(u => u.Status == true)
                                        .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (user == null)
            {
                return null;
            }

            if (request.Password == configuration["DefaultAccountCreate:Password"])
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<TokensResponse?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            User? user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;
            return await CreateTokenResponse(user);
        }

        private async Task<TokensResponse> CreateTokenResponse(User user)
        {
            var response = new TokensResponse
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
            return response;
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.Include("Role").FirstOrDefaultAsync(u => userId == u.UserId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.Role, user.Role!.RoleName),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Email, user.EmailAddress ?? ""),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<User?> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (user is null)
            {
                return null!;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.OldPassword) == PasswordVerificationResult.Failed)
            {
                return null;
            }
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.NewPassword);
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<string> GetOtpAsync(SendOtpRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && u.EmailAddress == request.EmailAddress);
            if (user == null)
            {
                return null!;
            }

            var otp = GenerateOtp();
            user.Otp = otp;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(1);
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user.Otp;
        }

        private string GenerateOtp()
        {
            var otp = new Random().Next(100000, 999999).ToString();
            return otp;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && u.Otp == request.Otp);
            if (user == null)
            {
                return false;
            }
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.NewPassword);
            user.Otp = null;
            user.OtpExpiryTime = null;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyOtpAsync(string otp)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Otp == otp && u.OtpExpiryTime > DateTime.UtcNow);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckLoginAsync(UserLoginRequest request)
        {
            var user = await context.Users.Include("Role").Where(u => u.Status == true)
                                        .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (user == null)
            {
                return false;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }
    }
}
