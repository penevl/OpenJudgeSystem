namespace OJS.Servers.Ui.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Hosting;
    using OJS.Common;
    using OJS.Data;
    using OJS.Servers.Infrastructure.Extensions;

    internal static class WebApplicationExtensions
    {
        public static WebApplication ConfigureWebApplication(
            this WebApplication app,
            string apiVersion)
        {
            app.UseCors(GlobalConstants.CorsDefaultPolicyName);
            app
                .UseDefaults()
                .MapDefaultRoutes();

            app.MigrateDatabase<OjsDbContext>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocs(apiVersion.ToApiName());
            }

            //Added here, because if it is added in the end immediately after the 200 response (Healthy) the FE redirects to 404 page.
            app.UseHealthMonitoring();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}");
            });

            return app;
        }
    }
}