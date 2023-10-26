using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GAVResorts.Utils;
using System.Text;
using GAVResorts.Models;
using GAVResorts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GAVResorts.DTOs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureSwagger(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } });
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GAVResort V1");
    });
}

//app.UseHttpsRedirection();
ConfigureEndpoints(app);

app.UseAuthentication();
app.UseAuthorization();


app.Run();


void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                RoleClaimType = "Role",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
}

void ConfigureSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "GAV Resorts", Version = "v1" });

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Enter JWT Bearer token **_only_**",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

        var securityRequirement = new OpenApiSecurityRequirement
        {
            {securityScheme, new string[] { }}
        };

        c.AddSecurityRequirement(securityRequirement);
    });
}

void ConfigureEndpoints(WebApplication app)
{
    app.MapPost("/login", (UserDTO user, UserService userService, IConfiguration configuration) =>
    {
        var authenticatedUser = userService.Authenticate(user.Usuario, user.Senha);
        if (authenticatedUser == null)
        {
            return Results.Unauthorized();
        }

        var token = GenerateJwtToken.GenerateToken(authenticatedUser, configuration);
        return Results.Ok(new { token });
    });

    app.MapPost("/createUser", (User user, UserService userService) =>
    {
        try
        {
            var createdUser = userService.Create(user);

            return Results.Ok(new { createdUser });
        }
        catch (Exception ex)
        {

            return Results.BadRequest(ex.Message);
        }

    });

    app.MapGet("/getContacts", (ContactService contactService) =>
    {
        try
        {
            var contacts = contactService.GetAllContacts();

            if (contacts == null || !contacts.Any())
            {
                return Results.NotFound("Nenhum contato encontrado");
            }

            return Results.Ok(contacts);

        }
        catch (Exception ex)
        {

            return Results.BadRequest(ex.Message);
        }
    }).RequireAuthorization();

    app.MapGet("/getByIdContacts/{id}", (int id, ContactService contactService) =>
    {
        try
        {
            var contact = contactService.GetContactById(id);
            if (contact == null)
            {
                return Results.NotFound($"Nenhum contato encontrado com o ID: {id}.");
            }
            return Results.Ok(contact);
        }
        catch (Exception ex)
        {

            return Results.BadRequest(ex.Message);
        }
    }).RequireAuthorization();

    app.MapPost("/createContacts", (Contact contact, ContactService contactService) =>
    {
        try
        {
            var createdContact = contactService.Create(contact);
            return Results.Ok(new { createdContact });
        }
        catch (Exception ex)
        {

            return Results.BadRequest(ex.Message);
        }
    }).RequireAuthorization();

    app.MapPut("/editContacts/{id}", (int id, Contact updatedContact, ContactService contactService) =>
    {
        try
        {
            if (id != updatedContact.Id)
            {
                return Results.BadRequest("O ID do contato na URL não corresponde ao ID do contato no corpo da requisição.");
            }

            var contact = contactService.UpdateContact(updatedContact);
            if (contact == null)
            {
                return Results.NotFound($"Nenhum contato encontrado com o ID: {id}.");
            }
            return Results.Ok(contact);
        }
        catch (Exception ex)
        {

            return Results.BadRequest(ex.Message);
        }
    }).RequireAuthorization();

    app.MapDelete("/deleteContacts/{id}", [Authorize(Roles = "ADM")] (int id, ContactService contactService) =>
    {
        try
        {
            var isDeleted = contactService.Delete(id);

            if (!isDeleted)
                return Results.NotFound($"Nenhum contato encontrado com o ID: {id}.");

            return Results.Ok($"Contado com ID: {id} foi removido com sucesso.");
        }
        catch (Exception ex)
        {

            return Results.BadRequest(ex.Message);
        }
    });

}