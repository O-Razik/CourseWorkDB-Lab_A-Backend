using Lab_A.Abstraction.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.Abstraction.IData;

public abstract class BaseUnitOfWork<T> where T : DbContext
{
    protected readonly T AContext;

    protected BaseUnitOfWork(T aContext)
    {
        AContext = aContext;
    }

    public T GetContext() => AContext;

    public IAnalysisRepository AnalysisRepository { get; init; } = null!;

    public IAnalysisCategoryRepository AnalysisCategoryRepository { get; init; } = null!;

    public IAnalysisCenterRepository AnalysisCenterRepository { get; init; } = null!;

    public IAnalysisResultRepository AnalysisResultRepository { get; init; } = null!;

    public IAnalysisBiomaterialRepository AnalysisBiomaterialRepository { get; init; } = null!;

    public IBiomaterialRepository BiomaterialRepository { get; init; } = null!;

    public IBiomaterialCollectionRepository BiomaterialCollectionRepository { get; init; } = null!;

    public IBiomaterialDeliveryRepository BiomaterialDeliveryRepository { get; init; } = null!;

    public IClientRepository ClientRepository { get; init; } = null!;

    public ISexRepository SexRepository { get; init; } = null!;

    public IClientOrderRepository ClientOrderRepository { get; init; } = null!;

    public IOrderAnalysisRepository OrderAnalysisRepository { get; init; } = null!;

    public IStatusRepository StatusRepository { get; init; } = null!;

    public ILaboratoryRepository LaboratoryRepository { get; init; } = null!;

    public ICityRepository CityRepository { get; init; } = null!;

    public ILaboratoryScheduleRepository LaboratoryScheduleRepository { get; init; } = null!;

    public IScheduleRepository ScheduleRepository { get; init; } = null!;

    public IDayRepository DayRepository { get; init; } = null!;

    public IEmployeeRepository EmployeeRepository { get; init; } = null!;

    public IPositionRepository PositionRepository { get; init; } = null!;

    public IInventoryRepository InventoryRepository { get; init; } = null!;

    public IInventoryDeliveryRepository InventoryDeliveryRepository { get; init; } = null!;

    public IInventoryOrderRepository InventoryOrderRepository { get; init; } = null!;

    public IInventoryInOrderRepository InventoryInOrderRepository { get; init; } = null!;

    public IInventoryInLaboratoryRepository InventoryInLaboratoryRepository { get; init; } = null!;

    public ISupplierRepository SupplierRepository { get; init; } = null!;
}