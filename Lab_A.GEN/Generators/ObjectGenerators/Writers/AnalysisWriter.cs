using Lab_A.Abstraction.IData;
using Lab_A.Abstraction.IModels;
using Lab_A.GEN.Models;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lab_A.GEN.Generators.ObjectGenerators.Writers
{
    public class AnalysisWriter
    {
        public AnalysisWriter(BaseUnitOfWork<LabAContext> unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public BaseUnitOfWork<LabAContext> UnitOfWork { get; }
         
        public async Task Write(List<AnalysisJson> list)
        {
            foreach (var item in list)
            {
                var category = await CheckForCategoryCreate(item.Category);

                var biomaterialList = new List<IBiomaterial>();

                foreach (var biomaterial in item.Biomaterials!)
                {
                    var biomaterialEntity = await CheckForBiomaterialCreate(biomaterial.Name);
                    if (biomaterialEntity != null) biomaterialList.Add(biomaterialEntity);
                }

                if (category != null)
                {
                    var analysis = new Analysis()
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        CategoryId = category.AnalysisCategoryId,
                        CreateDatetime = DateTime.Now
                    };

                    await UnitOfWork.AnalysisRepository.CreateAsync(analysis);
                    await AnalysesBiomaterialCreate(analysis, biomaterialList);
                }
            }
        }

        private async Task<IAnalysisCategory?> CheckForCategoryCreate(string? category)
        {
            var result = await UnitOfWork.AnalysisCategoryRepository.GetByName(category);
            if (result is not null) return result as AnalysisCategory;
            result = new AnalysisCategory()
            {
                Category = category,
                CreateDatetime = DateTime.Now
            };
            await UnitOfWork.AnalysisCategoryRepository.CreateAsync(result);
            return result as AnalysisCategory;
        }

        private async Task<IBiomaterial?> CheckForBiomaterialCreate(string biomaterial)
        {
            var result = await UnitOfWork.BiomaterialRepository.GetByName(biomaterial);
            if (result is not null) return result as Biomaterial;
            result = new Biomaterial()
            {
                BiomaterialName = biomaterial,
                CreateDatetime = DateTime.Now
            };
            await UnitOfWork.BiomaterialRepository.CreateAsync(result);
            return result as Biomaterial;
        }

        private async Task AnalysesBiomaterialCreate(Analysis analysis, List<IBiomaterial> biomaterials)
        {
            foreach (var analysisBiomaterial in biomaterials.Select(biomaterial => new AnalysisBiomaterial()
                     {
                         AnalysisId = analysis.AnalysisId,
                         BiomaterialId = biomaterial.BiomaterialId,
                         CreateDatetime = DateTime.Now
                     }))
            {
                await UnitOfWork.AnalysisBiomaterialRepository.CreateAsync(analysisBiomaterial);
            }
        }
    }
}
