using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Services.Implementations;
using System.Text;

namespace MainServiceWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
                .Build();

            if (configuration.Get<ApplicationConfig>() is not IApplicationConfig receptionConfig)
                throw new ConfigurationException("Не удалось прочитать конфигурацию сервиса");

            string connection = configuration!.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connection))
                throw new ConfigurationException("Не удалось прочитать строку подключения");
            builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(x=>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x=>
                {
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = receptionConfig.AuthOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(receptionConfig.AuthOptions.Key))
                    };
                    x.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[receptionConfig.CookiesName];
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Auth/";
                });
            builder.Services.AddAuthorization();

            builder.Services.AddControllersWithViews();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            /*builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<MainConsumer>();


                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(receptionConfig.BusConfig.Host, receptionConfig.BusConfig.Port, receptionConfig.BusConfig.Path, h =>
                    {
                        h.Username(receptionConfig.BusConfig.Username);
                        h.Password(receptionConfig.BusConfig.Password);
                    });

                    cfg.UseTransaction(_ =>
                    {
                        _.Timeout = TimeSpan.FromSeconds(60);
                        _.IsolationLevel = IsolationLevel.ReadCommitted;
                    });

                    cfg.ReceiveEndpoint(new TemporaryEndpointDefinition(), e =>
                    {
                        e.ConfigureConsumer<MainConsumer>(context);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });*/
            builder.Services.AddSingleton(receptionConfig);
            builder.Services.AddTransient<IMainService, MainService>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<ITokenService, TokenService>();

            ////Добавление страницы Пациента
            //builder.Services.AddHttpClient<IPatientServiceClient, PatientServiceClient>(client =>
            //{
            //    client.BaseAddress = new Uri("https://localhost:7056"); // Адрес API PatientService
            //});


            var app = builder.Build();

            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();
            app.MapControllers();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            app.Run();
        }
    }
}
