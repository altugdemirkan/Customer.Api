using Customer.Business.Customer;
using Customer.Facade;
using Customer.Facade.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Customer.Infrastructure;
using Customer.Core.Entities;
using Customer.Core.Enums;
using System.Reflection;
using Customer.Infrastructure.Repositories.Interfaces;
using Customer.Infrastructure.Repositories.Concrete;

namespace Customer.Api
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
            AddDI(services);
            services.AddControllers();
            services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ApiDatabase"));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
            AddTestData(context);

        }

        public void AddDI(IServiceCollection services)
        {
            //Facade
            services.AddTransient<ICustomerFacade, CustomerFacade>();

            //Repositories
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            //
            services.AddMediatR(typeof(CustomerInsertCommandHandler));
        }

        private static void AddTestData(ApiDbContext context)
        {
            context.Customer.AddRange(new List<CustomerEntity> {
                new CustomerEntity{ id = 1, first_name = "Altuð", last_name = "Demirkan", nationality = "Turkey", status = Status.TakenForProcessing},
                new CustomerEntity{ id = 2, first_name = "Noah", last_name = "Bakker ",nationality = "Holland", status = Status.TakenForProcessing},
                new CustomerEntity{ id = 3, first_name = "Anders", last_name = "Hansen",nationality = "Denmark", status = Status.TakenForProcessing},
                new CustomerEntity{ id = 4, first_name = "Bora", last_name = "Bilgn",nationality = "Turkey", status = Status.OutOfScope},
                new CustomerEntity{ id = 5, first_name = "Sem", last_name = "Jansen",nationality = "Holland", status = Status.OutOfScope},
                new CustomerEntity{ id = 6, first_name = "Anker", last_name = "Andersen",nationality = "Denmark", status = Status.OutOfScope},
                new CustomerEntity{ id = 7, first_name = "Büþra", last_name = "Pekoz",nationality = "Turkey", status = Status.Qualified},
                new CustomerEntity{ id = 8, first_name = "Liam", last_name = "Meijer",nationality = "Holland", status = Status.Qualified},
                new CustomerEntity{ id = 9, first_name = "Annelise", last_name = "Pedersen",nationality = "Denmark", status = Status.Qualified}
            });
            context.SaveChanges();
        }
    }
}
