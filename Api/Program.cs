
using System;
using InteractivePresentation.Client;
using InteractivePresentation.Client.Client;
using InteractivePresentation.Client.Client.Abstract;
using InteractivePresentation.Client.Service;
using InteractivePresentation.Client.Service.Abstract;
using InteractivePresentation.Domain.Entity;
using InteractivePresentation.Domain.Repository.Abstract;
using InteractivePresentation.Domain.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InteractivePresentation.Domain.Service.Abstract;
using InteractivePresentation.Domain.Service;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
            builder.Services.AddScoped<IVoteRepository, VoteRepository>();
            builder.Services.AddScoped<IPollRepository, PollRepository>();
            builder.Services.AddScoped<IPollService, PollService>();
            builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("Application"));
            builder.Services.AddHttpClient<IApiClient, ApiClient>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));
            builder.Services.AddTransient<IPresentationClientService, PresentationClientService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=presentations.db"));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
