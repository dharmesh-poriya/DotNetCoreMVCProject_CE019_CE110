using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentAttendanceManagementSystem.Models;
using StudentAttendanceManagementSystem.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAttendanceManagementSystem
{
    public class Startup
    {
        private IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
            });
            
            services.AddScoped<ILoginRepository, SQLLoginRepository>();
            services.AddScoped<IAdminRepository, SQLAdminRepository>();
            services.AddScoped<IBranchRepository, SQLBranchRepository>();
            services.AddScoped<ISubjectRepository, SQLSubjectRepository>();
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            services.AddScoped<IFacultyRepository, SQLFacultyRepository>();
            services.AddScoped<IStudentSubjectRepository, SQLStudentSubjectRepository>();

            //string cs = @"server=(localdb)\MSSQLLocalDB; database=StudentAttendanceManagementSystem; trusted_connection=true;";
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SAMScs")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            // this is for session object
            app.UseSession();

            app.UseMvc();
        }
    }
}
