using Application;
using Application.UseCases.Cart.Commands.RemoveFromCart;
using Application.UseCases.Cart.Queries.GetCartById;
using Application.UseCases.Product.Commands.CreateProduct;
using Application.UseCases.Product.Queries.GetAllProducts;
using Application.UseCases.User.Commands.CreateUser;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using SmartE_commercePlatform.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions();
builder.Services.AddControllers(config =>
{
    config.Filters.Add(new FilterExceptions());
}).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TokenSerializer>();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options => options.AddPolicy(name: "CorsPolicy",
                      builder =>
                      {
                          builder.AllowAnyHeader()
                          .AllowAnyMethod()
                          .SetIsOriginAllowed((host) => true)
                          .AllowCredentials()
                          .WithExposedHeaders("Authorization");
                      })
);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>(),
            IssuerSigningKey = new SymmetricSecurityKey(SecretProvider.Instance.Secret),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    }).AddNegotiate();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
