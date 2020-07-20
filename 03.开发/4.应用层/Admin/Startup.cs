using JW.Base.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;

namespace Admin {
    /// <summary>
    /// 
    /// </summary>
    public class Startup {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
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

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            #region 控制器注入
            services.AddControllersWithViews();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region Spa Application目录注入
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/dist";
            });
            #endregion

            #region Session 注入
            services.AddSession();
            #endregion

            #region Swagger Api版本信息注入
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "后台服务接口",
                    Description = "坂田釜作后台接口",
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
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Admin.xml");
                c.IncludeXmlComments(xmlPath);
                /*string entityPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Entity.xml");
                c.IncludeXmlComments(entityPath);*/

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


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            #region Environment
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            #endregion

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "焙时接口");
                // 访问Swagger的路由后缀
                c.RoutePrefix = "swagger";
            });
            #endregion

            #region 跨域
            app.UseCors(builder => {
                builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
            });
            #endregion

            // ↓↓↓↓↓↓ 注意下边这些中间件的顺序，很重要 ↓↓↓↓↓↓
            // 跳转https
            //app.UseHttpsRedirection();
            // 使用静态文件
            app.UseStaticFiles();
            // 使用Sap Application静态文件
            app.UseSpaStaticFiles();
            // Routing
            app.UseRouting();
            // 开启认证
            app.UseAuthentication();
            // 开启鉴权
            app.UseAuthorization();
            // Endpoints
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            // 开启Spa Application（服务端开/关调试时，前端也必须开/关）
            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";
                // 开发环境启动NPM脚本
                //if (env.IsDevelopment()) {
                //    spa.UseReactDevelopmentServer(npmScript: "start");
                //}
            });
        }
    }
}