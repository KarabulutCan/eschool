using ESCHOOL.ControllerClass;
using ESCHOOL.Hubs;
using ESCHOOL.Models;
using ESCHOOL.Services;
using ESCHOOL.Subscription;
using ESCHOOL.Subscription.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.WebReportDesigner.Services;

namespace ESCHOOL
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.Configuration = configuration;
            this.WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages().AddNewtonsoftJson();
            services.AddSingleton<DatabaseSubscription<UsersChat>>();
            services.AddSignalR();

            // Bu b�l�m T�rk�e karakter sorunu ile kar��la��ld���nda TELERIK ��z�m olarak bildirilmi�ti, bu ilaveden sonra 
            // T�rk�e karakter sorunu ��z�lm��, fark�nda olmadan ConnectionString b�l�m�n� tetikleyen �zelli�i kald�rm���z.
            // �imdi a�a��daki SetConnectionString yeniden yap�land�r�ld� ve �al��t�r�ld�, di�er sorun'a bak�lacak.
            //services.TryAddSingleton<IReportServiceConfiguration>(sp =>
            //    new ReportServiceConfiguration
            //    {
            //        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
            //        HostAppId = "ReportingNet5",
            //        Storage = new FileStorage(),
            //        // ReportSourceResolver = new CustomReportSourceResolver()
            //        ReportSourceResolver = new TypeReportSourceResolver()
            //            .AddFallbackResolver(new UriReportSourceResolver(
            //                System.IO.Path.Combine(
            //                    sp.GetService<IWebHostEnvironment>().WebRootPath,
            //                    "Reports")))
            //    });
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ////Raporlama b�l�m�nde "public void SetConnectionString(string schoolCode)" b�l�m� tetikliyor
            //services.TryAddSingleton<IReportServiceConfiguration>(sp =>
            //    new ReportServiceConfiguration
            //    {
            //        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
            //        HostAppId = "ReportingNet5",
            //        Storage = new FileStorage(),
            //        ReportSourceResolver = new CustomReportSourceResolver()

            //    });
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ////Yeni Gelen
            services.TryAddSingleton<IReportServiceConfiguration>(sp =>
                new ReportServiceConfiguration
                {
                    ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
                    HostAppId = "ReportingNet5",
                    Storage = new FileStorage(),
                    //ReportSourceResolver = new CustomReportSourceResolver()
                    ReportSourceResolver = new UriReportSourceResolver(System.IO.Path.Combine(sp.GetService<IWebHostEnvironment>().WebRootPath, "Reports"))
                            .AddFallbackResolver(new CustomReportSourceResolver())

                });
            services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
            {
                DefinitionStorage = new FileDefinitionStorage(Path.Combine(sp.GetService<IWebHostEnvironment>().WebRootPath, "Reports")),
                SettingsStorage = new FileSettingsStorage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting")),
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                        .AddViewLocalization(options => options.ResourcesPath = "Resources")
                        .AddDataAnnotationsLocalization();

            services.AddDbContext<CustomersDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FirstConnection")));

            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IUsersChatRepository, UsersChatRepository>();

            //services.AddDbContext<SchoolDbContext>(options =>  options.UseSqlServer(Configuration.GetConnectionString("DevConnection")), ServiceLifetime.Scoped);

            services.AddDbContext<SchoolDbContext>((services, optionsBuilder) =>
            {
                var cs = Configuration.GetConnectionString("DevConnection");

                var httpContextAccessor = services.GetService<IHttpContextAccessor>();
                HttpContext httpContext = httpContextAccessor.HttpContext;

                if (httpContext != null)
                {
                    var schoolCode = httpContextAccessor.HttpContext.Request.Cookies["schoolCode"];
                    if (schoolCode != null)
                    {
                        cs = string.Format(cs, schoolCode);
                        optionsBuilder.UseSqlServer(cs);
                    }
                    else
                    {
                        optionsBuilder.UseSqlServer(cs);
                    }
                }
                else
                {
                    optionsBuilder.UseSqlServer(cs);
                }

            }, ServiceLifetime.Transient);
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ISchoolInfoRepository, SchoolInfoRepository>();

            services.AddScoped<ISchoolBusServicesRepository, SchoolBusServicesRepository>();
            services.AddScoped<IPSerialNumberRepository, PSerialNumberRepository>();
            services.AddScoped<ISchoolFeeRepository, SchoolFeeRepository>();
            services.AddScoped<IDiscountTableRepository, DiscountTableRepository>();
            services.AddScoped<IStudentDiscountRepository, StudentDiscountRepository>();

            services.AddScoped<ISchoolFeeRepository, SchoolFeeRepository>();
            services.AddScoped<ISchoolFeeTableRepository, SchoolFeeTableRepository>();
            services.AddScoped<IClassroomRepository, ClassroomRepository>();
            services.AddScoped<IParameterRepository, ParameterRepository>();

            services.AddScoped<IBankRepository, BankRepository>();

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentPeriodsRepository, StudentPeriodsRepository>();
            services.AddScoped<IStudentAddressRepository, StudentAddressRepository>();
            services.AddScoped<IStudentNoteRepository, StudentNoteRepository>();
            services.AddScoped<IStudentParentAddressRepository, StudentParentAddressRepository>();
            services.AddScoped<IStudentFamilyAddressRepository, StudentFamilyAddressRepository>();
            services.AddScoped<IStudentInvoiceAddressRepository, StudentInvoiceAddressRepository>();
            services.AddScoped<IStudentInvoiceRepository, StudentInvoiceRepository>();
            services.AddScoped<IStudentInvoiceDetailRepository, StudentInvoiceDetailRepository>();
            services.AddScoped<IStudentDebtRepository, StudentDebtRepository>();
            services.AddScoped<IStudentDebtDetailRepository, StudentDebtDetailRepository>();
            services.AddScoped<IStudentDebtDetailTableRepository, StudentDebtDetailTableRepository>();
            services.AddScoped<IStudentInstallmentRepository, StudentInstallmentRepository>();
            services.AddScoped<IStudentInstallmentPaymentRepository, StudentInstallmentPaymentRepository>();
            services.AddScoped<ITempDataRepository, TempDataRepository>();
            services.AddScoped<ITempPlanRepository, TempPlanRepository>();

            services.AddScoped<IStudentPaymentRepository, StudentPaymentRepository>();
            services.AddScoped<IStudentTempRepository, StudentTempRepository>();

            services.AddScoped<IUsersLogRepository, UsersLogRepository>();
            services.AddScoped<IAbsenteeismRepository, AbsenteeismRepository>();            

            services.AddScoped<IUsersWorkAreasRepository, UsersWorkAreasRepository>();
            services.AddScoped<IAccountCodesRepository, AccountCodesRepository>();
            services.AddScoped<IAccountCodesDetailRepository, AccountCodesDetailRepository>();
            services.AddScoped<IAccountingRepository, AccountingRepository>();
            services.AddScoped<ISmsEmailRepository, SmsEmailRepository>();
            services.AddScoped<IMultipurposeListRepository, MultipurposeListRepository>();
            services.AddScoped<IExcelDataRepository, ExcelDataRepository>();

            services.AddScoped<IStudentTaskDataSourceRepository, StudentTaskDataSourceRepository>();
            services.AddScoped<IUsersTaskDataSourceRepository, UsersTaskDataSourceRepository>();
            services.AddScoped<ISchoolsTaskDataSourceRepository, SchoolsTaskDataSourceRepository>();
            services.AddScoped<ITempM101Repository, TempM101Repository>();
            services.AddScoped<ITempM101HeaderRepository, TempM101HeaderRepository>();
            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SchoolDbContext>();
                var cs = context.Database.GetDbConnection().ConnectionString;
                if (!cs.Contains("{0}"))
                {
                    context.Database.EnsureCreated();
                }
            }

            if (env.IsDevelopment())
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
            app.UseCors("ReportingRestPolicy");

            app.UseDatabaseSubscription<DatabaseSubscription<UsersChat>>("UsersChat");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UsersChatHub>("/usersChatHub");
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                     name: "default",
               // pattern: "{controller=ListPanel}/{action=List116}/{userID=2}/{msg=0}");

               pattern: "{controller=Home}/{action=Index}/{userID=2}");
             // pattern: "{controller=Login}/{action=Login}/{id?}");

            //    pattern: "{controller=M999ImportToWeb}/{action=ImportToWeb}/{userID=2}/{msg=0}");

            });
        }
    }
}
