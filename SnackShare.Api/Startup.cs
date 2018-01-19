using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SnackShare.Api.Data;
using SnackShare.Api.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace SnackShare.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SnackDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddAuthentication()
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = "https://id.graydientcreative.com";
            //        options.RequireHttpsMetadata = true;
            //        options.SaveToken
            //    })
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = "https://id.graydientcreative.com";
                   options.RequireHttpsMetadata = true;
                   options.SaveToken = true;
                   

                   options.ApiName = "snackshare";
                   options.JwtBearerEvents = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                   {
                       OnTokenValidated = async e =>
                       {
                           var principal = e.Principal as ClaimsPrincipal;
                           var identity = principal.Identity as ClaimsIdentity;
                           var email = principal.FindFirstValue("preferred_username"); // should be email...

                           var dbContext = e.HttpContext.RequestServices.GetRequiredService<SnackDbContext>();

                           var user = await dbContext.Users.SingleOrDefaultAsync(x => x.EmailAddress == email);
                           if (user != null)
                           {
                               identity.AddClaim(new Claim("ss_user_id", user.Id.ToString()));
                           }
                       }
                   };
               });

            services.AddMvc();
            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SnackShare API", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    AuthorizationUrl = "https://id.graydientcreative.com/connect/authorize",
                    
                    Flow = "implicit",
                    TokenUrl = "https://id.graydientcreative.com/connect/token",

                    Scopes = new Dictionary<string, string>
                    {
                        { "snackshare", "The Scope needed to access API" }
                    }
                });

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "SnackShare.Api.xml");
                c.IncludeXmlComments(filePath);
            });

            var appAssembly = typeof(Startup).GetTypeInfo().Assembly;
            services.AddAutoMapper(appAssembly);

            // App specific services
            services.AddTransient<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(policy =>
            {
                policy.WithOrigins(
                    "http://localhost:50314/",
                    "http://localhost:4200");

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Snack Share V1");
                c.ConfigureOAuth2("snackshare", null, null, "Swagger UI");
                
                
            });
        }
    }
}
