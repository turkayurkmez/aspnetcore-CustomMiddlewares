using customMiddlewares.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// gelen tüm http talepleri üzerinde çalışması şart olan eylemler middleware gerektirir.
// Configure the HTTP request pipeline.


//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("<<<<");
//    await next();
//    await context.Response.WriteAsync(">>>>");


//});



//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("------");
//    await next();
//    await context.Response.WriteAsync("------");
//});



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseBadwordHandler();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.UseWelcomePage();

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Deneme");


//});

app.Run();