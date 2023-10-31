using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserJourney.Core.Common;
using UserJourney.Core.Contracts;
using UserJourney.Core.Services;
using UserJourney.Repositories;
using UserJourney.Repositories.Contracts;
using UserJourney.Repositories.EF;

namespace UserJourney.Site
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add framework services.
            services.AddDbContext<UserJourneyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UserJourneyDB")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = "ers_auth_cookie";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
                options.LoginPath = string.Format("/{0}/{1}", UserJourney.Core.Constants.Controllers.UserController, UserJourney.Core.Constants.Actions.Login);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;

                // To not emit the attribute at all set
                // SameSite = (SameSiteMode)(-1)
                options.MinimumSameSitePolicy = SameSiteMode.Strict;

                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            });

            services.Configure<EMailSettings>(options => Configuration.GetSection("EMailSettings").Bind(options));
            services.AddTransient<IClaimsManager, ClaimsManager>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IProjectUnitOfWork, ProjectUnitOfWork>();
            services.AddTransient<IHelperService, HelperService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = UserJourney.Core.Constants.Controllers.UserController, action = UserJourney.Core.Constants.Actions.Login });
            });
        }
    }
}
