using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

builder.Services.AddDbContext<SOCIALDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("stringSQL")));

//remove cycles dependences
builder.Services.AddControllers().AddJsonOptions(opt =>
{

    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


//builder.Services.AddAuthentication(auth => {
//    auth.toke


//})
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
