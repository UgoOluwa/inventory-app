using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Inventory.API.Data.Implementations;
using Inventory.API.Data.Interfaces;
using Inventory.API.Repositories.Implementations;
using Inventory.API.Repositories.Interfaces;
using Inventory.API.Services.Implementations;
using Inventory.API.Settings.Implementations;
using Inventory.API.Settings.Interfaces;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Inventory
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

            #region Configuration Dependencies

            services.Configure<ProductDatabaseSettings>(Configuration.GetSection(nameof(ProductDatabaseSettings)));

            services.AddSingleton<IProductDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);

            #endregion

            services.AddControllers();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory", Version = "v1" });
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IContext, Context>();
            services.AddTransient<IContextSeed, ContextSeed>();
            services.AddTransient<IUtility, Utility>();
            services.AddTransient<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory v1");
                c.RoutePrefix = string.Empty;
            });

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
