using Asp.netCoreGraphQL.DataAccess;
using Asp.netCoreGraphQL.Model;
using Asp.netCoreGraphQL.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreGraphQL
{
    public class Startup
    {
        private readonly string AllowedOrigin = "allowedOrigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddRazorPages();
            services.AddDbContext<SampleAppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("SampleAppDbContext")));
            services.AddInMemorySubscriptions();

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>();

            services.AddScoped<EmployeeRepository, EmployeeRepository>();
            services.AddScoped<DepartmentRepository, DepartmentRepository>();

            services.AddCors(option => {
                option.AddPolicy("allowedOrigin",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                    );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(AllowedOrigin);
            app.UseWebSockets();
            app.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                });
        }
    }
}
