using Lab_A.Abstraction.IData;
using Lab_A.DAL.Data;
using LabA.DAL.Repositories;

namespace LabA.DAL.Data;

public class UnitOfWork : BaseUnitOfWork<LabAContext>
{
    public UnitOfWork(LabAContext aContext) : base(aContext)
    {
        AnalysisRepository = new AnalysisRepository(AContext);
        AnalysisCategoryRepository = new AnalysisCategoryRepository(AContext);
        AnalysisCenterRepository = new AnalysisCenterRepository(AContext);
        AnalysisResultRepository = new AnalysisResultRepository(AContext);
        AnalysisBiomaterialRepository = new AnalysisBiomaterialRepository(AContext);
        BiomaterialRepository = new BiomaterialRepository(AContext);
        BiomaterialCollectionRepository = new BiomaterialCollectionRepository(AContext);
        BiomaterialDeliveryRepository = new BiomaterialDeliveryRepository(AContext);
        ClientRepository = new ClientRepository(AContext);
        SexRepository = new SexRepository(AContext);
        ClientOrderRepository = new ClientOrderRepository(AContext);
        OrderAnalysisRepository = new OrderAnalysisRepository(AContext);
        StatusRepository = new StatusRepository(AContext);
        LaboratoryRepository = new LaboratoryRepository(AContext);
        CityRepository = new CityRepository(AContext);
        LaboratoryScheduleRepository = new LaboratoryScheduleRepository(AContext);
        ScheduleRepository = new ScheduleRepository(AContext);
        DayRepository = new DayRepository(AContext);
        EmployeeRepository = new EmployeeRepository(AContext);
        PositionRepository = new PositionRepository(AContext);
        InventoryRepository = new InventoryRepository(AContext);
        InventoryDeliveryRepository = new InventoryDeliveryRepository(AContext);
        InventoryOrderRepository = new InventoryOrderRepository(AContext);
        InventoryInOrderRepository = new InventoryInOrderRepository(AContext);
        InventoryInLaboratoryRepository = new InventoryInLaboratoryRepository(AContext);
        SupplierRepository = new SupplierRepository(AContext);
    }
}