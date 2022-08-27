using System.Text;
using interships_Management.Data;
using interships_Management.Helpers;
using interships_Management.Models;
using interships_Management.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IAuthUserService,AuthUserService>();
builder.Services.AddScoped<IAuthAdminService,AuthAdminService>();

builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<ContextDb>();

builder.Services.AddDbContext<ContextDb>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDB"));
});
builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>{
    options.RequireHttpsMetadata= false;
    options.SaveToken= false;
   
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey= true,
        
        ValidateIssuer=true,
        ValidateAudience= true,
        ValidateLifetime= true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience= builder.Configuration["JWT:Audience"],
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    
});
builder.Services.AddCors(options=>{
    options.AddPolicy("Cors",p=>{
        p.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
        
    });
});
var app = builder.Build();
app.UseCors("Cors");
// Configure the HTTP request pipeline.
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
