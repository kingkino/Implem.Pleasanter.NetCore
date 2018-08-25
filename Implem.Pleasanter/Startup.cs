using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCoreCurrentRequestContext;
using Implem.DefinitionAccessor;
using Implem.Libraries.Utilities;
using Implem.Pleasanter.Filters;
using Implem.Pleasanter.Libraries.DataSources;
using Implem.Pleasanter.Libraries.Initializers;
using Implem.Pleasanter.Libraries.Migrators;
using Implem.Pleasanter.Libraries.Security;
using Implem.Pleasanter.Libraries.Server;
using Implem.Pleasanter.Libraries.Settings;
using Implem.Pleasanter.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Implem.Pleasanter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Libraries.Server.Applications.SetStartTime(DateTime.Now);
            Libraries.Server.Applications.LastAccessTime = Libraries.Server.Applications.StartTime;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddMvc(
                options =>
                {
                    options.Filters.Add(new HandleErrorExAttribute());
                    options.Filters.Add(new CheckParameterSyntaxError());
                    options.Filters.Add(new RequestLimitAttribute());
                    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                    options.Filters.Add(new CheckUserAttributes());
                    if (Parameters.Service.RequireHttps)
                    {
                        options.Filters.Add(new RequireHttpsAttribute());
                    }
                });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = new PathString("/users/login"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Initializer.Initialize(
                path: env.ContentRootPath,
                assemblyVersion: Assembly.GetExecutingAssembly().GetName().Version.ToString());

            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            lifetime.ApplicationStopping.Register(OnShutdown);

            app.UseSession();
            app.UseCurrentRequestContext();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.Use(async (context, next) => await Invoke(context, next));

            app.Use(async (context, next) => await ErrorHandling(context, next));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}",
                    defaults: new
                    {
                        Controller = "Items",
                        Action = "Index"
                    },
                    constraints: new
                    {
                        Controller = "[A-Za-z][A-Za-z0-9_]*",
                        Action = "[A-Za-z][A-Za-z0-9_]*"
                    }
                );
                routes.MapRoute(
                    name: "Others",
                    template: "{reference}/{id}/{controller}/{action}",
                    defaults: new
                    {
                        Action = "Index"
                    },
                    constraints: new
                    {
                        Reference = "[A-Za-z][A-Za-z0-9_]*",
                        Id = "[0-9]+",
                        Controller = "Binaries|OutgoingMails",
                        Action = "[A-Za-z][A-Za-z0-9_]*"
                    }
                );
                routes.MapRoute(
                    name: "Item",
                    template: "{controller}/{id}/{action}",
                    defaults: new
                    {
                        Controller = "Items",
                        Action = "Edit"
                    },
                    constraints: new
                    {
                        Id = "[0-9]+",
                        Action = "[A-Za-z][A-Za-z0-9_]*"
                    }
                );
                routes.MapRoute(
                    name: "Binaries",
                    template: "binaries/{guid}/{action}",
                    defaults: new
                    {
                        Controller = "Binaries"
                    },
                    constraints: new
                    {
                        Guid = "[A-Z0-9]+",
                        Action = "[A-Za-z][A-Za-z0-9_]*"
                    }
                );

            });
        }

        private static bool isFirst = true;
        public async Task Invoke(HttpContext httpContext, Func<Task> next)
        {
            if (isFirst)
            {
                InitializeLog();
                InitializeSession();
                isFirst = false;
            }
            await next();
        }

        private void InitializeLog()
        {
            var log = new SysLogModel();
            SiteInfo.TenantCaches.Add(0, new TenantCache(0));
            SiteInfo.Reflesh();
            UsersInitializer.Initialize();
            ItemsInitializer.Initialize();
            StatusesMigrator.Migrate();
            SiteSettingsMigrator.Migrate();
            StatusesInitializer.Initialize();
            log.Finish();
        }

        private void InitializeSession()
        {
            Sessions.Set("StartTime", DateTime.Now);
            Sessions.Set("LastAccessTime", Sessions.Get<DateTime>("StartTime"));
            Sessions.Set("SessionGuid", Strings.NewGuid());
            if (Sessions.LoggedIn())
            {
                //TODO Ldap
                //if (Authentications.Windows())
                //{
                //    Ldap.UpdateOrInsert(HttpContext.Current.User.Identity.Name);
                //}
                var userId = Sessions.UserId();
                var tenantId = Rds.ExecuteScalar_int(statements:
                    Rds.SelectUsers(
                        column: Rds.UsersColumn().TenantId(),
                        where: Rds.UsersWhere().UserId(userId)));
                Sessions.SetTenantId(tenantId);
                StatusesInitializer.Initialize(tenantId);
                var userModel = new UserModel(
                    SiteSettingsUtilities.UsersSiteSettings(),
                    userId);
                if (userModel.AccessStatus == Databases.AccessStatuses.Selected &&
                    !userModel.Disabled)
                {
                    userModel.SetSession();
                }
                else
                {
                    Authentications.SignOut();
                    SetAnonymouseSession();
                    var request = AspNetCoreHttpContext.Current.Request;
                    var url = $"{request.Scheme}://{request.Host.Value}{request.Path.Value}{request.QueryString.Value}";
                    AspNetCoreHttpContext.Current.Response.Redirect(url);
                }
            }
            else
            {
                SetAnonymouseSession();
            }

            var @base = AspNetCoreHttpContext.Current.Request.PathBase;
            switch (@base.Value.ToLower())
            {
                case "~/backgroundtasks/do":
                case "~/reminderschedules/remind":
                    break;
                default:
                    new SysLogModel().Finish();
                    break;
            }
        }
        private void SetAnonymouseSession()
        {
            var userModel = new UserModel();
            Sessions.Set("Language", userModel.Language);
            Sessions.Set("RdsUser", userModel.RdsUser());
            Sessions.Set("Developer", userModel.Developer);
        }

        public async Task ErrorHandling(HttpContext httpContext, Func<Task> next)
        {
            try
            {
                await next();
            }
            catch (Exception error)
            {
                try
                {
                    var log = new SysLogModel();
                    log.SysLogType = SysLogModel.SysLogTypes.Execption;
                    log.ErrMessage = error.Message;
                    log.ErrStackTrace = error.StackTrace;
                    log.Finish();
                }
                catch
                {
                    throw;
                }
            }
        }

        private void OnShutdown()
        {
            var log = new SysLogModel();
            Performances.PerformanceCollection.Save(Directories.Logs());
            log.Finish();
        }

    }
}
