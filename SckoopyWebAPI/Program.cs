using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SckoopyWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<PostService>();
            builder.Services.AddScoped<CommentService>(); 
            builder.Services.AddScoped<FileUploadService>();

            builder.Services.AddSingleton<AppSettingsService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));

            //builder.Services.AddSwaggerGen(options =>
            //{
               
            //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header, 
            //        Description = "Please enter your Bearer token", 
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey
            //    });

               
            //    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] {}
            //        }
            //    });
            //});

            builder.Services.AddSingleton(c =>
            {
                var cloudinary = new Cloudinary(new Account(
                    Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
                    Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
                    Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
                ));
                return cloudinary;
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
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

            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }
    }
}
