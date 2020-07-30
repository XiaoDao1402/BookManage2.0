using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


using JW.Base.Configuration;
//using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;


namespace Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
            #region 配置
            ConfigurationManager.Current.Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile("appsettings.Development.json")
#else
                .AddJsonFile("appsettings.json")
#endif
                .Build();
            #endregion
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            #region 控制器注入
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region Session 注入
            services.AddSession();
            #endregion

            #region Swagger Api信息注入
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "服务接口",
                    Description = "焙时 Bast",
                    Contact = new OpenApiContact {
                        Name = "Xiodra",
                        Email = "y.dragon.hu@hotmail.com",
                        Url = new Uri("https://xiodra.github.io"),
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // 加载程序集的xml描述文档
                string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Base.xml");
                c.IncludeXmlComments(basePath);
                string entityPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Entity.xml");
                c.IncludeXmlComments(entityPath);
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Api.xml");
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();
            });
            #endregion

            #region JWT注入
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options => {
                  options.TokenValidationParameters = new TokenValidationParameters {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "bestapi",
                      ValidAudience = "bestapi",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"]))
                  };
              });
            #endregion

            #region 自定义配置注入
            //services.AddCustomConfiguration(Configuration);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "焙时服务接口");
                // 访问Swagger的路由后缀
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            // 开启认证
            app.UseAuthentication();
            // 开启鉴权
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
