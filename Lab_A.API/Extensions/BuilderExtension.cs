using Lab_A.Abstraction.IData;
using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.Services;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using LabA.DAL.Data;
using LabA.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Lab_A.API.Extensions;

public static class BuilderExtension
{
    public static void AddDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<LabAContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(180);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
            });
        });

    }

    public static void AddModels(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAnalysis, Analysis>();
        builder.Services.AddScoped<IAnalysisCategory, AnalysisCategory>();
        builder.Services.AddScoped<IAnalysisResult, AnalysisResult>();
        builder.Services.AddScoped<IAnalysisBiomaterial, AnalysisBiomaterial>();
        builder.Services.AddScoped<IAnalysisCenter, AnalysisCenter>();
        builder.Services.AddScoped<IBiomaterial, Biomaterial>();
        builder.Services.AddScoped<IBiomaterialDelivery, BiomaterialDelivery>();
        builder.Services.AddScoped<IBiomaterialCollection, BiomaterialCollection>();
        builder.Services.AddScoped<ICity, City>();
        builder.Services.AddScoped<IClient, Client>();
        builder.Services.AddScoped<IClientOrder, ClientOrder>();
        builder.Services.AddScoped<IDay, Day>();
        builder.Services.AddScoped<IEmployee, Employee>();
        builder.Services.AddScoped<IInventory, Inventory>();
        builder.Services.AddScoped<IInventoryDelivery, InventoryDelivery>();
        builder.Services.AddScoped<IInventoryInLaboratory, InventoryInLaboratory>();
        builder.Services.AddScoped<IInventoryInOrder, InventoryInOrder>();
        builder.Services.AddScoped<IInventoryOrder, InventoryOrder>();
        builder.Services.AddScoped<ILaboratory, Laboratory>();
        builder.Services.AddScoped<ILaboratorySchedule, LaboratorySchedule>();
        builder.Services.AddScoped<IOrderAnalysis, OrderAnalysis>();
        builder.Services.AddScoped<IPosition, Position>();
        builder.Services.AddScoped<ISchedule, Schedule>();
        builder.Services.AddScoped<IStatus, Status>();
        builder.Services.AddScoped<ISex, Sex>();
        builder.Services.AddScoped<ISupplier, Supplier>();
        builder.Services.AddScoped<IUserEmployee, UserEmployee>();
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAnalysisService, AnalysisService>();
        builder.Services.AddScoped<IAnalysisCategoryService, AnalysisCategoryService>();
        builder.Services.AddScoped<IAnalysisResultService, AnalysisResultService>();
        builder.Services.AddScoped<IAnalysisBiomaterialService, AnalysisBiomaterialService>();
        builder.Services.AddScoped<IAnalysisCenterService, AnalysisCenterService>();
        builder.Services.AddScoped<IBiomaterialService, BiomaterialService>();
        builder.Services.AddScoped<IBiomaterialDeliveryService, BiomaterialDeliveryService>();
        builder.Services.AddScoped<IBiomaterialCollectionService, BiomaterialCollectionService>();
        builder.Services.AddScoped<ICityService, CityService>();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IClientOrderService, ClientOrderService>();
        builder.Services.AddScoped<IDayService, DayService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IInventoryService, InventoryService>();
        builder.Services.AddScoped<IInventoryDeliveryService, InventoryDeliveryService>();
        builder.Services.AddScoped<IInventoryInLaboratoryService, InventoryInLaboratoryService>();
        builder.Services.AddScoped<IInventoryInOrderService, InventoryInOrderService>();
        builder.Services.AddScoped<IInventoryOrderService, InventoryOrderService>();
        builder.Services.AddScoped<ILaboratoryService, LaboratoryService>();
        builder.Services.AddScoped<ILaboratoryScheduleService, LaboratoryScheduleService>();
        builder.Services.AddScoped<IOrderAnalysisService, OrderAnalysisService>();
        builder.Services.AddScoped<IPositionService, PositionService>();
        builder.Services.AddScoped<IScheduleService, ScheduleService>();
        builder.Services.AddScoped<IStatusService, StatusService>();
        builder.Services.AddScoped<ISexService, SexService>();
        builder.Services.AddScoped<ISupplierService, SupplierService>();
        builder.Services.AddScoped<IAuthService<UserDto>, AuthService>();
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<BaseUnitOfWork<LabAContext>, UnitOfWork>();
        builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        builder.Services.AddScoped<IAnalysisCategoryRepository, AnalysisCategoryRepository>();
        builder.Services.AddScoped<IAnalysisResultRepository, AnalysisResultRepository>();
        builder.Services.AddScoped<IAnalysisBiomaterialRepository, AnalysisBiomaterialRepository>();
        builder.Services.AddScoped<IAnalysisCenterRepository, AnalysisCenterRepository>();
        builder.Services.AddScoped<IBiomaterialRepository, BiomaterialRepository>();
        builder.Services.AddScoped<IBiomaterialDeliveryRepository, BiomaterialDeliveryRepository>();
        builder.Services.AddScoped<IBiomaterialCollectionRepository, BiomaterialCollectionRepository>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IClientOrderRepository, ClientOrderRepository>();
        builder.Services.AddScoped<IDayRepository, DayRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
        builder.Services.AddScoped<IInventoryDeliveryRepository, InventoryDeliveryRepository>();
        builder.Services.AddScoped<IInventoryInLaboratoryRepository, InventoryInLaboratoryRepository>();
        builder.Services.AddScoped<IInventoryInOrderRepository, InventoryInOrderRepository>();
        builder.Services.AddScoped<IInventoryOrderRepository, InventoryOrderRepository>();
        builder.Services.AddScoped<ILaboratoryRepository, LaboratoryRepository>();
        builder.Services.AddScoped<ILaboratoryScheduleRepository, LaboratoryScheduleRepository>();
        builder.Services.AddScoped<IOrderAnalysisRepository, OrderAnalysisRepository>();
        builder.Services.AddScoped<IPositionRepository, PositionRepository>();
        builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
        builder.Services.AddScoped<IStatusRepository, StatusRepository>();
        builder.Services.AddScoped<ISexRepository, SexRepository>();
        builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
        builder.Services.AddScoped<IUserEmployeeRepository, UserEmployeeRepository>();
    }
    
    public static void AddSwaggerDev(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });
    }
}