using ButlerLee.API.Extensions;
using ButlerLee.API.HttpClient.Framework;
using ButlerLee.API.HttpClient.Options;
using ButlerLee.API.Models;
using ButlerLee.API.Services;
using ButlerLee.API.Utilities.Converters;
using ButlerLee.API.Utilities.Helpers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ButlerLee.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;

            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables(); //시스템 환경변수(윈도우즈) Product

            //if (env.IsDevelopment())
            //{
            //    builder.AddUserSecrets<Startup>();
            //}

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddHttpContextAccessor();
            services.AddControllers();

            Configurations configurations = new Configurations();
            Configuration.Bind("Configurations", configurations);
            Models.HostAwayClient hostAwayClient = new Models.HostAwayClient();
            Configuration.Bind("HostAwayClient", hostAwayClient);

            services.AddSingleton(configurations);
            services.AddSingleton(hostAwayClient);

            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureMySqlContext(configurations.ConnectionString);
            services.ConfigureRepositoryWrapper(); 
            services.ConfigureServiceWrapper();
            services.ConfigureClientWrapper();

            services.AddPolicies(this.Configuration);

            var hostAwayConfig =
                Configuration.GetSection("HostAway").Get<HostAway>();

            services.AddAccessTokenManagement(options =>
            {
                options.Client.Clients.Add("identityserver", new ClientCredentialsTokenRequest
                {
                    Address = hostAwayConfig.Endpoint,
                    ClientId = hostAwayConfig.ClientId,
                    ClientSecret = hostAwayConfig.ClientSecret,
                    Scope = hostAwayConfig.Scope
                });
            });

            services.AddHttpClient<HostAwayClientOptions>(this.Configuration,
                    nameof(HostAwayClient));

            services.AddKakaoHttpClient<KakaoPayClientOptions>(this.Configuration,
                   nameof(KakaoPayClient));

            services.AddTossHttpClient<TossPayClientOptions>(this.Configuration,
                  nameof(TossPayClient));


            services.AddAutoMapper(typeof(ServiceWrapper).Assembly);

            //services.AddMaintenance(() => false, null); //service에 미들웨어로 등록

            if (WebHostEnvironment.IsProduction())
            {
                //백그라운드 서비스
                services.ConfigureBackgroundService();
            }
            else 
            {
                //services.ConfigureBackgroundService();
                #region Swagger
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Version = "V1",
                        Title = "ButlerLee",
                        Description = "ButlerLee API",
                        Contact = new OpenApiContact()
                        {
                            Name = "HeeJi Kim",
                            Email = "hjKim@saladsoft.com",
                        }
                    });

                    // Set the comments path for the Swagger JSON and UI.
                    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    string xmlPath = Path.Combine(System.AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);

                    //Swagger Schema Generator
                    options.DocumentFilter<CustomModelDocumentFilter<Listing>>();
                    options.DocumentFilter<CustomModelDocumentFilter<CalendarDay>>();
                    options.DocumentFilter<CustomModelDocumentFilter<PriceDetail>>();
                    options.DocumentFilter<CustomModelDocumentFilter<Payment>>();

                    //Bearer 설정
                    /*
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new string[] { }
                        }});*/
                });
                #endregion
            }


            #region Authorization

            ////Azure 인증
            //services.AddAuthentication(sharedOptions =>
            //{
            //    sharedOptions.
            //    sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddAzureAdBearer(options => Configuration.Bind("Configurations:Azure", options));

            #endregion

            services
               .AddControllers(jsonOptions => jsonOptions.ReturnHttpNotAcceptable = true)
               .AddNewtonsoftJson(jsonOptions =>
               {
                   jsonOptions.SerializerSettings.ContractResolver = new OriginalPropertyContractResolver();
                   jsonOptions.SerializerSettings.Converters.Add((JsonConverter)new StringEnumConverter());
                   //jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
               });
            if (WebHostEnvironment.IsProduction() == false)
            {
                services.AddSwaggerGenNewtonsoftSupport();
            }

            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment() || env.IsStaging() || env.EnvironmentName == "Testing")
            {
                app.UseDeveloperExceptionPage();

                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ButtlerLee API V1");
                    options.RoutePrefix = string.Empty;
                });
                #endregion
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseCors(x => x
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .WithExposedHeaders("x-pagination")
                 .WithExposedHeaders("x-filename"));

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseGlobalExceptionMiddleware();

            app.UseMvc();
            SetThreadPool();
        }
        private void SetThreadPool()
        {
            int newLimits = 100 * System.Environment.ProcessorCount;
            int existingMaxWorkerThreads;
            int existingMaxIocpThreads;
            ThreadPool.GetMaxThreads(out existingMaxWorkerThreads, out existingMaxIocpThreads);
            ThreadPool.SetMaxThreads(newLimits, Math.Max(newLimits, existingMaxIocpThreads));
        }

        string CustomSchemaIdSelector(Type modelType)
        {
            if (!modelType.IsConstructedGenericType) return modelType.FullName.Replace("[]", "Array");

            var prefix = modelType.GetGenericArguments()
                .Select(genericArg => CustomSchemaIdSelector(genericArg))
                .Aggregate((previous, current) => previous + current);

            return prefix + modelType.FullName.Split('`').First();
        }
    }
}
