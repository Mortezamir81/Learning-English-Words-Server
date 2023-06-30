//******************************
var builder =
	WebApplication.CreateBuilder(args);
//******************************


#region Services
//******************************
builder.Services.Configure<ApplicationSettings>
	(builder.Configuration.GetSection(ApplicationSettings.KeyName));

builder.Services.AddCustomDbContext
	(connectionString: builder.Configuration.GetConnectionString("MySqlServerConnectionString"));

builder.Services.AddCustomIdentity
	(builder.Configuration.GetSection($"{nameof(ApplicationSettings)}:{nameof(IdentitySettings)}").Get<IdentitySettings>());

builder.Services.AddCustomCORS();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomLogger();

builder.Services.AddAutoDetectedServices();

builder.Services.AddAutoMapper(typeof(Infrastructure.AutoMapperProfiles.WordProfile));

builder.Services.AddSignalR();

builder.Services.AddCustomApiVersioning();

builder.Services.AddCustomController();

builder.Services.AddCustomCaching();

builder.Services.AddCustomSwaggerGen(builder.Configuration);

builder.Services.AddCustomJwtAuthentication
	(builder.Configuration.GetSection($"{nameof(ApplicationSettings)}:{nameof(JwtSettings)}").Get<JwtSettings>());
//******************************
#endregion /Services


//******************************
var app =
	builder.Build();
//******************************


#region Middlewares
//******************************
await app.IntializeDatabase();

if (app.Environment.IsProduction())
{
	app.UseGlobalExceptionMiddleware();
	app.UseSwaggerBasicAuthorization();
}
else
{
	app.UseDeveloperExceptionPage();
}

app.UseCustomSwaggerAndUI();

app.UseCors("DevCorsPolicy");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SignalHub>("/api/signal");
//******************************
#endregion /Middlewares


//******************************
app.Run();
//******************************


public partial class Program { }

