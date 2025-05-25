using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class BiomaterialDeliveryMapper
{
    public static BiomaterialDeliveryDto ToDto(this IBiomaterialDelivery biomaterialDelivery)
    {
        return new BiomaterialDeliveryDto()
        {
            BiomaterialDeliveryId = biomaterialDelivery.BiomaterialDeliveryId,
            BiomaterialCollectionId = (int)biomaterialDelivery.BiomaterialCollectionId!,
            AnalysisCenterId = (int)biomaterialDelivery.AnalysisCenterId!,
            DeliveryDate = (DateTime)biomaterialDelivery.DeliveryDate!,
            StatusId = biomaterialDelivery.StatusId,
            BiomaterialCollection = biomaterialDelivery.BiomaterialCollection.ToDto(),
            AnalysisCenter = biomaterialDelivery.AnalysisCenter.ToDto(),
            Status = biomaterialDelivery.Status.ToDto()
        };
    }
    public static IBiomaterialDelivery ToEntity(this BiomaterialDeliveryDto biomaterialDeliveryDto)
    {
        return new BiomaterialDelivery
        {
            BiomaterialDeliveryId = biomaterialDeliveryDto.BiomaterialDeliveryId,
            BiomaterialCollectionId = biomaterialDeliveryDto.BiomaterialCollectionId,
            AnalysisCenterId = biomaterialDeliveryDto.AnalysisCenterId,
            DeliveryDate = biomaterialDeliveryDto.DeliveryDate,
            StatusId = biomaterialDeliveryDto.StatusId,
            BiomaterialCollection = (BiomaterialCollection)biomaterialDeliveryDto.BiomaterialCollection.ToEntity(),
            AnalysisCenter = (AnalysisCenter)biomaterialDeliveryDto.AnalysisCenter.ToEntity(),
            Status = (Status)biomaterialDeliveryDto.Status.ToEntity()
        };
    }
}