using Infrastructure.Middlewares;
using Infrustructrue.Settings;
using Infrustructrue.Middlewares;
using Infrustructrue.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services;
using Services.SignalR;
using ViewModels.General;
using Microsoft.AspNetCore.Http;

//******************************
var webApplicationOptions =
	new WebApplicationOptions
	{
		EnvironmentName =
			System.Diagnostics.Debugger.IsAttached ?
			Environments.Development : Environments.Production,
	};
//******************************


//******************************
var builder =
	WebApplication.CreateBuilder(options: webApplicationOptions);
//******************************


#region Services
//******************************
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
});

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(ApplicationSettings.KeyName));

builder.Services.AddDbContextPool<Persistence.DatabaseContext>(option =>
{
	option.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("MySqlServerConnectionString"));
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("DevCorsPolicy", builder =>
	{
		builder
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient
	(serviceType: typeof(Dtat.Logging.ILogger<>),
		implementationType: typeof(Dtat.Logging.NLog.NLogAdapter<>));

builder.Services.AddScoped<Persistence.IUnitOfWork, Persistence.UnitOfWork>();

builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddScoped<IWordServices, WordServices>();

builder.Services.AddScoped<INotificationServices, NotificationServices>();

builder.Services.AddScoped<ITokenUtility, TokenUtility>();

builder.Services.AddAutoMapper(typeof(Infrustructrue.AutoMapperProfiles.WordProfile));

builder.Services.AddSignalR();

builder.Services.AddControllers();
//******************************
#endregion /Services


//******************************
var app =
	builder.Build();
//******************************


#region Middlewares
//******************************
app.UseGlobalExceptionMiddleware();

app.UseCors("DevCorsPolicy");

app.UseCustomJwtMiddleware();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseDeveloperExceptionPage();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));
}

app.UseRouting();

app.UseAuthorization();

app.UseCustomStaticFilesMiddleware();

app.UseEndpoints(endpoints =>
{
	endpoints.MapHub<SignalHub>("/api/signal");
	endpoints.MapControllers();
});
//******************************
#endregion /Middlewares


//******************************
app.Run();
//******************************

