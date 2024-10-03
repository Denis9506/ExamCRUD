using Azure.Data.Tables;
using Azure.Identity;
using AzureTeacherStudentSystem;
using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.Extensions.Azure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration["VaultUri"]);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new VisualStudioCredential());

builder.Services.AddSingleton(_=> ConnectionMultiplexer.Connect(builder.Configuration["Redis"]).GetDatabase());
builder.Services.AddRazorPages();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["Storage"]!, preferMsi: true);
    clientBuilder.AddTableServiceClient(builder.Configuration["Storage"]!, preferMsi: true);
});
builder.Services.AddTransient<ICacheService,RedisCacheService>();
builder.Services.AddTransient<IBlobService, BlobStorageService>();
builder.Services.AddScoped<ITableStorageService<Report>>(provider =>
{
    var tableServiceClient = provider.GetRequiredService<TableServiceClient>();
    var cacheService = provider.GetRequiredService<ICacheService>(); 
    return new TableStorageService<Report>(tableServiceClient, "Reports",cacheService); 
});

builder.Services.AddScoped<ITableStorageService<Author>>(provider =>
{
    var tableServiceClient = provider.GetRequiredService<TableServiceClient>();
    var cacheService = provider.GetRequiredService<ICacheService>();
    return new TableStorageService<Author>(tableServiceClient, "Authors", cacheService); 
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
