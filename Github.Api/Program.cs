using Github.Api.Configurations;
using Github.Api.Endpoints;
using Github.Api.Middleware;
using Github.Api.Services;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, logConfig) =>
    logConfig.ReadFrom.Configuration(context.Configuration));

// Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Github settings from configurations
builder.Services.AddOptions<GithubSettings>()
    .BindConfiguration(GithubSettings.Key)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Typed http client
builder.Services.AddHttpClient<GithubService>((sp, client) =>
{
    var githubSettings = sp.GetRequiredService<IOptions<GithubSettings>>().Value;

    client.DefaultRequestHeaders.Add("Authorization", githubSettings.AccessToken);
    client.DefaultRequestHeaders.Add("User-Agent", githubSettings.UserAgent);

    client.BaseAddress = new Uri(githubSettings.BaseAddress);
});

// Exception handler and problem details
builder.Services.AddExceptionHandler<ExceptionMiddleware>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLogContextMiddleware>();
app.UseSerilogRequestLogging();

// Exception handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

// Github Endpoints
app.MapGithubEndpoints();
app.Run();