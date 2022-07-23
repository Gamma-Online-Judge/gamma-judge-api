using Workers;

namespace Api
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("LocalPolicy", builder =>
            {
                builder.WithOrigins(new[] { "http://localhost:3000", "http://localhost:3006", "https://master.d1sk06otwbnk27.amplifyapp.com" })
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.RegisterServices(Configuration);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("LocalPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}