using JiraAnalyzer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseFirebird(builder.Configuration.GetConnectionString("FirebirdConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add Swagger
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

// Use CORS
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

// Save authors to file on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await SaveAuthorsOnStartup(context);
}

app.Run();

async Task SaveAuthorsOnStartup(DataContext context)
{
    var authors = await context.JiraWorklogs
        .Select(w => w.Author)
        .Distinct()
        .ToListAsync();

    await FileHelper.SaveAuthorsToFile(authors);
}
