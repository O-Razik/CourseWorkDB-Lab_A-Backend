using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface ILaboratoryScheduleRepository : ICrud<ILaboratorySchedule>
{
    Task<IEnumerable<ILaboratorySchedule>> ReadByLaboratoryAsync(int laboratoryId);
}