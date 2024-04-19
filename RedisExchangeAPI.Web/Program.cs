using RedisExchangeAPI.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Uygulama ayaða kalktýðý an redisservice'den 1 adet örnek alýr.
builder.Services.AddSingleton<RedisService>();

var app = builder.Build();

var redisService = app.Services.GetRequiredService<RedisService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

redisService.Connect();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


