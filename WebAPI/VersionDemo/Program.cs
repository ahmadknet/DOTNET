using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure API Versioning + OpenAPI integration
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
})
.AddOpenApi();

var app = builder.Build();

// 2. Map OpenAPI JSON endpoints & Swagger UI with version dropdown
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().WithDocumentPerVersion();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Version 1.0 (Deprecated)");
        options.SwaggerEndpoint("/openapi/v2.json", "Version 2.0 (Current)");
    });
}

// 3. Define Versioned Endpoint Groups
var versionSet = app.NewApiVersionSet()
    .HasDeprecatedApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .ReportApiVersions()
    .Build();

var apiGroup = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(versionSet);

apiGroup.MapGet("/products", () => new[]
{
    new ProductV1(1, "Mechanical Keyboard"),
    new ProductV1(2, "Wireless Mouse")
})
.MapToApiVersion(1, 0)
.WithSummary("Get products (Legacy v1)");

apiGroup.MapGet("/products", () => new[]
{
    new ProductV2(1, "Mechanical Keyboard", 129.99m, "Peripherals"),
    new ProductV2(2, "Wireless Mouse", 59.99m, "Peripherals")
})
.MapToApiVersion(2, 0)
.WithSummary("Get products with pricing (v2)");

app.Run();

record ProductV1(int Id, string Name);
record ProductV2(int Id, string Name, decimal Price, string Category);
