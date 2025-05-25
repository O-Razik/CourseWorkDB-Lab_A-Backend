using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class BiomaterialMapper
{
    public static BiomaterialDto ToDto(this IBiomaterial biomaterial)
    {
        return new BiomaterialDto
        {
            BiomaterialId = biomaterial.BiomaterialId,
            BiomaterialName = biomaterial.BiomaterialName,
        };
    }

    public static IBiomaterial ToEntity(this BiomaterialDto biomaterialDto)
    {
        return new Biomaterial
        {
            BiomaterialId = biomaterialDto.BiomaterialId,
            BiomaterialName = biomaterialDto.BiomaterialName,
        };
    }
}