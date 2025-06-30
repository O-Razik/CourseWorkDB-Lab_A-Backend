using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IInventoryDeliveryRepository : ICrud<IInventoryDelivery>
{
    Task<IInventoryDelivery?> UpdateStatusAsync(int deliveryId, int status);
}