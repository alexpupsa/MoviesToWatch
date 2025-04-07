using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoviesToWatch.Server.Configuration;
using MoviesToWatch.Server.Data;
using MoviesToWatch.Server.Data.Repositories;
using MoviesToWatch.Server.Profiles;
using MoviesToWatch.Server.Services;
using MoviesToWatch.Server.Services.Interfaces;

namespace MoviesToWatch.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            builder.Services.AddHttpClient<IMovieDBApiService, MovieDBApiService>((sp, client) =>
            {
                var config = sp.GetRequiredService<IOptions<AppSettings>>().Value;

                client.BaseAddress = new Uri(config.MovieDBApiBaseUrl!);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.MovieDBApiKey}");
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            var secret = 

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                var config = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

                                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                                {
                                    ValidateIssuer = false,
                                    ValidateAudience = false,
                                    ValidateLifetime = true,
                                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config!.AuthSecret!))
                                };
                            });


            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IMovieCommentRepository, MovieCommentRepository>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://localhost:56273")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            var app = builder.Build();

            app.UseCors("AllowSpecificOrigin");
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
