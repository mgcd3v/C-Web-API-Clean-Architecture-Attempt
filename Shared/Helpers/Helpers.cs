using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Helpers
{
    public static class Helpers
    {
        private static readonly IMapper _mapper;

        static Helpers()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                // Define your mappings here
                //cfg.CreateMap<SourceClass, DestinationClass>();
                // Add more mappings as needed
            });

            _mapper = configuration.CreateMapper();
        }

        public static void CreateToken(List<Claim> claims, string tokenKey, DateTime? ExpiresAt, out string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: ExpiresAt,
                signingCredentials: creds);

            token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
       
        public static int GetRandomDigits(int minValue, int maxValue)
        {
            var random = new Random();

            return random.Next(minValue, maxValue);
        }

        public static string GetSPCommand(string sp, List<Parameter> parameters)
        {
            string spCommand = $"EXEC {sp}";
            string paramString = "";

            foreach (var parameter in parameters)
            {
                paramString += $"{parameter.Name} = {parameter.Value}, ";
            }

            paramString = paramString != "" ? paramString.Substring(0, paramString.Length - 2) : paramString;

            return $"{spCommand} {paramString}";
        }

        public static string GetWrapped(string value, string wrapper)
        {
            return $"{wrapper}{value}{wrapper}";
        }

        public static string GetFinalParameterValue(object? value, string valueIfNull, string wrapper)
        {
            return (value == null)? valueIfNull : Helpers.GetWrapped(value?.ToString() ?? "", wrapper);
        }

        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static User? GetUser(HttpRequest? request)
        {
            var token = GetToken(request);

            if (token.IsNullOrEmpty())
            {
                return null;
            }

            var id = GetClaim(token, ClaimTypes.NameIdentifier);
            var userId = int.Parse(id);

            return new User() { Id = userId, Name = "" };
        }

        public static T GetFirstOrDefault<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                // Return the default value for the type T
                return default;
            }

            // Return the first element in the list
            return list.FirstOrDefault();
        }

        public static TDestination GetResponse<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public static string GetToken(HttpRequest? request)
        {
            var authHeader = request?.Headers["Authorization"];
            var token = authHeader?.ToString().Replace("Bearer ", "");
            token = token?.ToString().Replace("bearer ", "");

            return token ?? "";
        }

        public static string GetClaim(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var securityToken = jsonToken as JwtSecurityToken;

            var stringClaimValue = securityToken?.Claims.First(claim => claim.Type == claimType).Value;

            return stringClaimValue ?? "";
        }

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public static bool IsValidToken(string? token, string tokenKey)
        {
            if (token == null || token.IsNullOrEmpty())
            {
                return false;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    LifetimeValidator = _CustomLifetimeValidator,
                    RequireExpirationTime = true,
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        private static bool _CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
    }
}
