using Microsoft.EntityFrameworkCore;
using RatingMusciAPI.Context;
using Microsoft.EntityFrameworkCore.Design;
using RatingMusciAPI.Interfaces;
using RatingMusciAPI.Repositories;
using RatingMusciAPI.DTO.Mapping;
using RatingMusciAPI.FIlters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(option =>
{
    option.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mySqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
                                            options.UseMySql(mySqlConnectionStr,
                                            ServerVersion.AutoDetect(mySqlConnectionStr)));

builder.Services.AddScoped<IArtistsRepository,ArtistRepository>();
builder.Services.AddScoped<IAlbumsRepository, AlbumRepository>();
builder.Services.AddScoped<ISongsRepository, SongRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(DTOMappingExtension));





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
