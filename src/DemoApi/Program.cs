using DemoApi;
using DemoApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<EntraOptions>()
    .Bind(builder.Configuration.GetSection(EntraOptions.SectionName))
    .Validate(options =>
        !string.IsNullOrWhiteSpace(options.TenantId)
        && !string.IsNullOrWhiteSpace(options.ClientId)
        && !string.IsNullOrWhiteSpace(options.Audience),
        "Entra configuration must define TenantId, ClientId, and Audience.")
    .ValidateOnStart();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection(EntraOptions.SectionName));

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy(AuthorizationPolicies.ScopePolicy("access_as_user"), policy =>
        policy.RequireScope("access_as_user"));

    options.AddPolicy(AuthorizationPolicies.RolePolicy("Demo.Read"), policy =>
        policy.RequireRole("Demo.Read"));

    options.AddPolicy(AuthorizationPolicies.RolePolicy("Demo.Admin"), policy =>
        policy.RequireRole("Demo.Admin"));

    options.AddPolicy("Scopes:access_as_user_OR_Roles:Demo.Read", policy =>
        policy.RequireScopeOrRole("access_as_user", "Demo.Read"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("RequestLogger");
    logger.LogInformation("Handling request {Method} {Path} with request id {RequestId}",
        context.Request.Method,
        context.Request.Path,
        context.TraceIdentifier);

    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
