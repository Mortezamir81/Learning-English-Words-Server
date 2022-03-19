using Infrastructure.Middlewares;
using Infrustructrue.ApplicationSettings;
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

//******************************
var webApplicationOptions =
	new WebApplicationOptions
	{
		EnvironmentName =
			Environments.Development,

		//EnvironmentName =
		//	Environments.Production,
	};
//******************************


//******************************
var builder =
	Microsoft.AspNetCore.Builder
	.WebApplication.CreateBuilder(options: webApplicationOptions);
//******************************


#region Services
//******************************
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiCoreTemplate", Version = "v1" });
});

builder.Services.Configure<MainSettings>(builder.Configuration.GetSection("AppSettings"));

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

builder.Services.AddScoped
	(serviceType: typeof(Softmax.Logging.ILogger<>),
		implementationType: typeof(Softmax.Logging.NLog.NLogAdapter<>));

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
app.UseCors("DevCorsPolicy");

app.UseCustomJwtMiddleware();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication2 v1"));
}

app.UseDeveloperExceptionPage();

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

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

