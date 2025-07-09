
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AutoMapper;

using PersonMicroservice.Models.DTO;
using PersonMicroservice.Repository;
using PersonMicroservice.Data;

namespace PersonMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //--------------------------------------------------------------------------------------------------------------------------------
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<PersonDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("myConn")));
            
            
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<IDonationRepository, DonationRepository>();
            builder.Services.AddScoped<IReceiverRepository, ReceiverRepository>();
            builder.Services.AddScoped<IStockRepo, StockRepository>();


            builder.Services.AddAutoMapper(typeof(MappingProfile));
            //--------------------------------------------------------------------------------------------------------------------------------

            // Code for JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidAudiences = builder.Configuration.GetSection("JWT:ValidAudiences").Get<IEnumerable<string>>(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
                    };
                });
            
            

            //--------------------------------------------------------------------------------------------------------------------------------



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Jwt",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            //// Add CORS policy
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAngular",
            //        policy =>
            //        {
            //            policy.WithOrigins("http://localhost:4200") // Angular app URL
            //                  .AllowAnyMethod()
            //                  .AllowAnyHeader();
            //        });
            //});

            var app = builder.Build();

            app.UseCors("AllowAngular");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(policyConfig =>
            {
                policyConfig.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
            });
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
