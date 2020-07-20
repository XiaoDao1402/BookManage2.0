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
            #region ����
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

            #region ������ע��
            services.AddControllersWithViews();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region Spa ApplicationĿ¼ע��
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/dist";
            });
            #endregion

            #region Session ע��
            services.AddSession();
            #endregion

            #region Swagger Api�汾��Ϣע��
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "��̨����ӿ�",
                    Description = "���︪����̨�ӿ�",
                    Contact = new OpenApiContact {
                        Name = "Xiodra",
                        Email = "y.dragon.hu@hotmail.com",
                        Url = new Uri("https://xiodra.github.io"),
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
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

                // ���س��򼯵�xml�����ĵ�
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Admin.xml");
                c.IncludeXmlComments(xmlPath);
                /*string entityPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Entity.xml");
                c.IncludeXmlComments(entityPath);*/

                c.EnableAnnotations();
            });
            #endregion

            #region JWTע��
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "��ʱ�ӿ�");
                // ����Swagger��·�ɺ�׺
                c.RoutePrefix = "swagger";
            });
            #endregion

            #region ����
            app.UseCors(builder => {
                builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
            });
            #endregion

            // ������������ ע���±���Щ�м����˳�򣬺���Ҫ ������������
            // ��תhttps
            //app.UseHttpsRedirection();
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // ʹ��Sap Application��̬�ļ�
            app.UseSpaStaticFiles();
            // Routing
            app.UseRouting();
            // ������֤
            app.UseAuthentication();
            // ������Ȩ
            app.UseAuthorization();
            // Endpoints
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            // ����Spa Application������˿�/�ص���ʱ��ǰ��Ҳ���뿪/�أ�
            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";
                // ������������NPM�ű�
                //if (env.IsDevelopment()) {
                //    spa.UseReactDevelopmentServer(npmScript: "start");
                //}
            });
        }
    }
}