using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Restaurants.BusinessLogic;
using Restaurants.BusinessLogic.Interfaces;
using Restaurants.Common.Configuration;
using Restaurants.DataAccess.Entity;
using Restaurants.DataAccess.Interfaces;
using System.Text;

namespace Restaurants.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200")
                                .AllowCredentials()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .WithExposedHeaders(new[] { "Location" })
                                .AllowCredentials());
            });

            var tokenConfiguration = new TokenConfiguration();
            Configuration.Bind("Token", tokenConfiguration);

            services.AddSingleton(tokenConfiguration);

            services.AddTransient<RestaurantsDbContext>();
            services.AddTransient<IUserProvider, UserProvider>();
            services.AddTransient<IRestaurantProvider, RestaurantProvider>();
            services.AddTransient<IReviewProvider, ReviewProvider>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReviewService, ReviewService>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            var key = Encoding.ASCII.GetBytes(tokenConfiguration.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
