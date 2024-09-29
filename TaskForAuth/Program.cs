
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TaskForAuth.Data.Context;
using TaskForAuth.Data.Models;

namespace TaskForAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // bonus part
            var keyString = builder.Configuration.GetValue<string>("JWT:SecretKey");
            var keyInBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInBytes);
            builder.Services.AddSingleton(key);

            builder.Services.AddDbContext<SchoolContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectionstring"));
            });
            builder.Services.AddIdentity<SchoolUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;

            }).AddEntityFrameworkStores<SchoolContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Default";
                options.DefaultChallengeScheme = "Default";
            })
            .AddJwtBearer("Default", options =>
                {
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Student",
                    p => p.RequireClaim(ClaimTypes.Role, "Student")
                    );
                options.AddPolicy("Teacher",
                    p => p.RequireClaim(ClaimTypes.Role, "Teacher")
                    );
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
