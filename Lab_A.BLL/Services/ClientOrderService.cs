using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;
using Lab_A.DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace Lab_A.BLL.Services;

public class ClientOrderService : IClientOrderService
{
    private readonly IClientOrderRepository _repository;

    public ClientOrderService(IClientOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IClientOrder> CreateAsync(IClientOrder entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IClientOrder?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IClientOrder>> ReadAllAsync(DateTime? fromDate = null,
        DateTime? toDate = null,
        int? employeeId = null,
        string? clientFullname = null,
        double? minPrice = null,
        double? maxPrice = null,
        IEnumerable<int>? statusIds = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = (await _repository.ReadAllAsync()).AsQueryable();
        var pipeline = new Pipeline<IClientOrder>();

        // Date range filtering
        if (fromDate.HasValue || toDate.HasValue)
        {
            pipeline.Register(new DateRangeStep<IClientOrder>(
                fromDate,
                toDate,
                nameof(IClientOrder.BiomaterialCollectionDate)));
        }

        // Employee filtering
        if (employeeId.HasValue)
        {
            pipeline.Register(new EqualityStep<IClientOrder, int?>(
                employeeId,
                nameof(IClientOrder.EmployeeId)));
        }

        // Search filtering
        // Client name search
        if (!string.IsNullOrWhiteSpace(clientFullname))
        {
            pipeline.Register(new StringContainsStep<IClientOrder>(
                clientFullname,
                $"{nameof(IClientOrder.Client)}.{nameof(IClient.FirstName)}",
                $"{nameof(IClientOrder.Client)}.{nameof(IClient.LastName)}"));
        }

        // Price range filtering
        if (maxPrice.HasValue)
        {
            if(minPrice.HasValue)
            {
                if (minPrice > maxPrice)
                {
                    throw new ArgumentException("Min price cannot be greater than max price.");
                }
            }
            else
            {
                minPrice = 0;
            }

            pipeline.Register(new NumericRangeStep<IClientOrder>(
                minPrice,
                maxPrice,
                nameof(IClientOrder.Fullprice)));
        }

        // Status filtering
        if (statusIds != null)
        {
            var ids = statusIds.ToList();
            if (ids.Any())
            {
                pipeline.Register(new ClassificationStep<IClientOrder>(
                    ids,
                    nameof(IClientOrder.StatusId)));
            }
        }

        // Apply all filters first
        var filteredQuery = pipeline.Execute(query);

        // Then apply paging
        var pagedQuery = new PagingStep<IClientOrder>(pageNumber, pageSize)
            .Process(filteredQuery);

        return pagedQuery.ToList();
    }

    public async Task<IEnumerable<IClientOrder>> ReadAllByClientIdAsync(int clientId)
    {
        return await _repository.ReadAllByClientIdAsync(clientId);
    }

    public async Task<IClientOrder?> UpdateAsync(IClientOrder entity)
    {
        return await _repository.UpdateAsync(entity);
    }
    
    public async Task<IClientOrder?> CancelOrderAsync(int clientOrderId)
    {
        return await _repository.CancelOrderAsync(clientOrderId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}