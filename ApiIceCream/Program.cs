using ApiIceCream.Data;
using ApiIceCream.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<dbcontext>(options => options.UseNpgsql(ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/IceCream/", async (IceCream ic, dbcontext db) =>
{
    db.Helados.Add(ic);
    await db.SaveChangesAsync();
    return Results.Created($"/IceCream/{ic.id}", ic);
});

app.MapGet("/IceCream/{id:int}", async (int id, dbcontext db) =>
{
    return await db.Helados.FindAsync(id)
        is IceCream ic ? Results.Ok(ic) : Results.NotFound();
});

app.MapGet("/IceCream", async (dbcontext db) => await db.Helados.ToListAsync());

app.MapPut("/IceCream/{id:int}", async (int id, IceCream ic, dbcontext db) =>
{
    if(ic.id != id) return Results.BadRequest();
    var helado = await db.Helados.FindAsync(id);
    if (helado == null) return Results.NotFound();
    helado.flavor = ic.flavor;
    helado.existence = ic.existence;
    await db.SaveChangesAsync();
    return Results.Ok(helado); 
});

app.MapDelete("/IceCream/{id:int}", async (int id, dbcontext db) =>
{
    var helado = await db.Helados.FindAsync(id);
    if(helado is null) return Results.NotFound();
    db.Helados.Remove(helado);
    await db.SaveChangesAsync();
    return Results.NoContent(); 
});

app.UseAuthorization();

app.MapControllers();


app.Run();
