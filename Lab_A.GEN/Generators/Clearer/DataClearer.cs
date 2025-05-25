using Lab_A.Abstraction.IData;
using Lab_A.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.GEN.Generators.Clearer;

public class DataClearer
{
    private readonly BaseUnitOfWork<LabAContext> _unitOfWork;

    public DataClearer(BaseUnitOfWork<LabAContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Clear()
    {
        ResetAllAutoIncrements();

        _unitOfWork.GetContext().ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

        await _unitOfWork.AnalysisResultRepository.DeleteAllAsync();
        await _unitOfWork.AnalysisCenterRepository.DeleteAllAsync();
        await _unitOfWork.BiomaterialDeliveryRepository.DeleteAllAsync();
        await _unitOfWork.BiomaterialCollectionRepository.DeleteAllAsync();
        await _unitOfWork.InventoryInLaboratoryRepository.DeleteAllAsync();
        await _unitOfWork.InventoryDeliveryRepository.DeleteAllAsync();
        await _unitOfWork.InventoryInOrderRepository.DeleteAllAsync();
        await _unitOfWork.InventoryRepository.DeleteAllAsync();
        await _unitOfWork.InventoryOrderRepository.DeleteAllAsync();
        await _unitOfWork.SupplierRepository.DeleteAllAsync();

        await _unitOfWork.OrderAnalysisRepository.DeleteAllAsync();
        await _unitOfWork.ClientOrderRepository.DeleteAllAsync();
        await _unitOfWork.ClientRepository.DeleteAllAsync();
        await _unitOfWork.SexRepository.DeleteAllAsync();

        await _unitOfWork.AnalysisBiomaterialRepository.DeleteAllAsync();
        await _unitOfWork.AnalysisRepository.DeleteAllAsync();
        await _unitOfWork.AnalysisCategoryRepository.DeleteAllAsync();
        await _unitOfWork.BiomaterialRepository.DeleteAllAsync();
        
        await _unitOfWork.EmployeeRepository.DeleteAllAsync();
        await _unitOfWork.PositionRepository.DeleteAllAsync();
        await _unitOfWork.LaboratoryScheduleRepository.DeleteAllAsync();
        await _unitOfWork.ScheduleRepository.DeleteAllAsync();
        await _unitOfWork.DayRepository.DeleteAllAsync();
        await _unitOfWork.LaboratoryRepository.DeleteAllAsync();
        await _unitOfWork.CityRepository.DeleteAllAsync();
        await _unitOfWork.StatusRepository.DeleteAllAsync();

        _unitOfWork.GetContext().ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    private void ResetAllAutoIncrements()
    {
        var tableNames = _unitOfWork.GetContext().Model.GetEntityTypes().Select(x => x.GetTableName()).ToList();
        foreach (var tableName in tableNames)
        {
            if (tableName != null) ResetAutoIncrement(tableName);
        }
    }

    private void ResetAutoIncrement(string tableName)
    {
        var rawSqlString = $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)";
        _unitOfWork.GetContext().Database.ExecuteSqlRaw(rawSqlString);
    }
}