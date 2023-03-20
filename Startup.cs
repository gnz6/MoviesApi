using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Helpers;
using MoviesApi.Models;
using MoviesApi.Services;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Text;

namespace MoviesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IStorage, AzureStorage>();

            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            services.AddSingleton(provider =>
            
            new MapperConfiguration(config =>
            {
                var geometryFactory = provider.GetService<GeometryFactory>();
                config.AddProfile(new AutoMapperProfile(geometryFactory));
            }).CreateMapper()
            ); 

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<MoviesApiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"), 
                sqlServerOptions => sqlServerOptions.UseNetTopologySuite()));
            services.AddControllers().AddNewtonsoftJson();
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<MoviesApiDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                 ClockSkew = TimeSpan.Zero
             });

            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
