using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class BiomaterialCollectionMapper
{
    public static BiomaterialCollectionDto ToDto(this IBiomaterialCollection biomaterialCollection)
    {
        return new BiomaterialCollectionDto()
        {
            BiomaterialCollectionId = biomaterialCollection.BiomaterialCollectionId,
            BiomaterialId = (int)biomaterialCollection.BiomaterialId!,
            InventoryInLaboratoryId = (int)biomaterialCollection.InventoryInLaboratoryId!,
            ClientOrderId = (int)biomaterialCollection.ClientOrderId!,
            ClientOrderNumber = biomaterialCollection.ClientOrder?.Number,
            CollectionDate = (DateOnly)biomaterialCollection.CollectionDate!,
            ExpirationDate = (DateTime)biomaterialCollection.ExpirationDate!,
            Volume = (double)biomaterialCollection.Volume!,
            Biomaterial = biomaterialCollection.Biomaterial.ToDto(),
            InventoryInLaboratory = biomaterialCollection.InventoryInLaboratory.ToDto(),
        };
    }
    public static IBiomaterialCollection ToEntity(this BiomaterialCollectionDto biomaterialCollectionDto)
    {
        return new BiomaterialCollection
        {
            BiomaterialCollectionId = biomaterialCollectionDto.BiomaterialCollectionId,
            BiomaterialId = biomaterialCollectionDto.BiomaterialId,
            InventoryInLaboratoryId = biomaterialCollectionDto.InventoryInLaboratoryId,
            ClientOrderId = biomaterialCollectionDto.ClientOrderId,
            CollectionDate = biomaterialCollectionDto.CollectionDate,
            ExpirationDate = biomaterialCollectionDto.ExpirationDate,
            Volume = biomaterialCollectionDto.Volume,
            Biomaterial = (Biomaterial)biomaterialCollectionDto.Biomaterial.ToEntity(),
            InventoryInLaboratory = (InventoryInLaboratory)biomaterialCollectionDto.InventoryInLaboratory.ToEntity(),
        };
    }
}