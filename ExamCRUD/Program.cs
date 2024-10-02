using Azure.Data.Tables;
using Azure.Identity;
using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration["VaultUri"]);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new VisualStudioCredential());

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["Storage"]!, preferMsi: true);
    clientBuilder.AddTableServiceClient(builder.Configuration["Storage"]!, preferMsi: true);
});
builder.Services.AddTransient<IBlobService, BlobStorageService>();
builder.Services.AddScoped<ITableStorageService<Report>>(provider =>
{
    var tableServiceClient = provider.GetRequiredService<TableServiceClient>();
    return new TableStorageService<Report>(tableServiceClient, "Reports");
});

builder.Services.AddScoped<ITableStorageService<Author>>(provider =>
{
    var tableServiceClient = provider.GetRequiredService<TableServiceClient>();
    return new TableStorageService<Author>(tableServiceClient, "Authors");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
