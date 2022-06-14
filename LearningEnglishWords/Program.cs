//******************************
var webApplicationOptions =
	new WebApplicationOptions
	{
		EnvironmentName =
			System.Diagnostics.Debugger.IsAttached ?
			Environments.Development : Environments.Production,
	};
//******************************


////******************************
//var builder =
//	WebApplication.CreateBuilder(options: webApplicationOptions);
////******************************


//******************************
var builder =
	WebApplication.CreateBuilder(args);
//******************************


#region Services
//******************************
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.",
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
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

builder.Services.AddScoped<ITokenServices, TokenServices>();

builder.Services.AddAutoMapper(typeof(Infrustructrue.AutoMapperProfiles.WordProfile));

builder.Services.AddSignalR();

builder.Services.AddControllers(config =>
{
	config.Filters.Add<ModelStateValidationAttribute>();
	config.Filters.Add<CustomExceptionHandlerAttribute>();
	config.Filters.Add(new LogInputParameterAttribute(InputLogLevel.Debug));
})
.ConfigureApiBehaviorOptions(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEasyCaching(options =>
{
	options.UseInMemory();
});
//******************************
#endregion /Services


//******************************
var app =
	builder.Build();
//******************************


#region Middlewares
//******************************
app.UseGlobalExceptionMiddleware();

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

app.UseCors("DevCorsPolicy");

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


public partial class Program { }

