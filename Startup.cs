using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using YetAnotherBugTracker.Initializer;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Roles;

namespace YetAnotherBugTracker
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
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            string demoDatabaseName = Guid.NewGuid().ToString();
            services.AddDbContext<DemoDbContext>(options => options.UseInMemoryDatabase(demoDatabaseName), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.User.RequireUniqueEmail = true;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<IRepository<Attachment>, AttachmentRepository>();
            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<IRepository<Project>, ProjectRepository>();
            services.AddScoped<IRepository<Ticket>, TicketRepository>();
            services.AddScoped<IRepository<State>, StateRepository>();
            services.AddScoped<IRepository<Priority>, PriorityRepository>();
            services.AddScoped<IRepository<ItemType>, ItemTypeRepository>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<Admin>();
            services.AddScoped<DemoAdmin>();
            services.AddScoped<DemoDeveloper>();
            services.AddScoped<DemoProjectManager>();
            services.AddScoped<DemoStakeholder>();
            services.AddScoped<Developer>();
            services.AddScoped<ProjectManager>();
            services.AddScoped<Stakeholder>();
            services.AddScoped<IRoleFactory, RoleFactory>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            dbInitializer.Init();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
