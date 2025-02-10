using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TECin2.API.Database;
using TECin2.API.Repositories;

namespace TECin2.API
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
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_CORSRules",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddScoped<ICheckInRepository, CheckInRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<LoggerRepository>();
            services.AddScoped<IPasswordRepository, PasswordRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //services.AddScoped<ICheckInService, CheckInService>();
            //services.AddScoped<IDepartmentService, DepartmentService>();
            //services.AddScoped<IGroupService, GroupService>();
            //services.AddScoped<IInstruktorService, InstruktorService>();
            //services.AddScoped<ILoginService, LoginService>();
            //services.AddScoped<ILoggerService, LoggerService>();
            //services.AddScoped<IPermissionService, PermissionService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<ISettingService, SettingService>();
            //services.AddScoped<IStudentService, StudentService>();

            services.AddDbContext<TECinContext>(
                x => x.UseSqlServer(Configuration.GetConnectionString("Default"))
                );
            services.AddDbContext<TECinContext2>(
                x => x.UseSqlServer(Configuration.GetConnectionString("Security"))
                );
            services.AddControllers();
            services.AddRazorPages();
            services.AddSwaggerGen();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseAuthentication();
            //app.UseMvc();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("_CORSRules");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Main");
            });
        }
    }
}
