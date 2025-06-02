using iTextSharp.text.pdf;
using iTextSharp.text;
using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;

namespace Lab_A.BLL.Services;

public class AnalysisResultService : IAnalysisResultService
{
    private readonly IAnalysisResultRepository _repository;

    public AnalysisResultService(IAnalysisResultRepository repository)
    {
        _repository = repository;
    }

    public async Task<IAnalysisResult> CreateAsync(IAnalysisResult entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IAnalysisResult?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }
    
    public async Task<IEnumerable<IAnalysisResult>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? analysisCenterId = null,
        int? analysisId = null,
        string? clientFullname = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = (await _repository.ReadAllAsync()).AsQueryable();
        var pipeline = new Pipeline<IAnalysisResult>();

        // Date range filtering (execution date)
        if (fromDate.HasValue || toDate.HasValue)
        {
            pipeline.Register(new DateRangeStep<IAnalysisResult>(
                fromDate,
                toDate,
                nameof(IAnalysisResult.ExecutionDate)));
        }

        // Analysis center filtering
        if (analysisCenterId.HasValue)
        {
            pipeline.Register(new EqualityStep<IAnalysisResult, int?>(
                analysisCenterId,
                nameof(IAnalysisResult.AnalysisCenterId)));
        }

        // Analysis type filtering
        if (analysisId.HasValue)
        {
            pipeline.Register(new EqualityStep<IAnalysisResult, int?>(
                analysisId,
                $"{nameof(IAnalysisResult.OrderAnalysis)}.{nameof(IOrderAnalysis.Analysis)}.{nameof(IAnalysis.AnalysisId)}"));
        }

        // Client name search
        if (!string.IsNullOrWhiteSpace(clientFullname))
        {
            pipeline.Register(new StringContainsStep<IAnalysisResult>(
                clientFullname,
                $"{nameof(IAnalysisResult.OrderAnalysis)}.{nameof(IOrderAnalysis.ClientOrder)}.{nameof(IClientOrder.Client)}.{nameof(IClient.FirstName)}",
                $"{nameof(IAnalysisResult.OrderAnalysis)}.{nameof(IOrderAnalysis.ClientOrder)}.{nameof(IClientOrder.Client)}.{nameof(IClient.LastName)}"));
        }

        // Apply all filters first
        var filteredQuery = pipeline.Execute(query);

        // Then apply paging
        var pagedQuery = new PagingStep<IAnalysisResult>(pageNumber, pageSize)
            .Process(filteredQuery);

        return pagedQuery.ToList();
    }

    public async Task<IAnalysisResult?> UpdateAsync(IAnalysisResult entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<Stream> GeneratePdfReportAsync(int analysisResultId)
    {
        var analysisResult = await _repository.ReadAsync(analysisResultId);
        if (analysisResult == null)
        {
            throw new KeyNotFoundException("Результат аналізу не знайдено.");
        }

        var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4, 40, 40, 40, 40);
        var writer = PdfWriter.GetInstance(document, memoryStream);
        writer.CloseStream = false;

        document.Open();

        // Font configuration with Arial Unicode for proper Cyrillic support
        string arialFontPath;
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            arialFontPath = @"C:\Windows\Fonts\arial.ttf";
        else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            arialFontPath = "/Library/Fonts/Arial Unicode.ttf";
        else // Linux fallback
            arialFontPath = "/usr/share/fonts/truetype/msttcorefonts/Arial.ttf";
        
        var headerFont = FontFactory.GetFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 18, Font.BOLD, BaseColor.DARK_GRAY);
        var analysisNameFont = FontFactory.GetFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 14, Font.BOLD, BaseColor.DARK_GRAY);
        var sectionFont = FontFactory.GetFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 14, Font.BOLD, BaseColor.BLACK);
        var normalFont = FontFactory.GetFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL, BaseColor.BLACK);
        var tableHeaderFont = FontFactory.GetFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.BOLD, BaseColor.WHITE);

        // 1. Header - Order number and analysis name (smaller and better formatted)
        document.Add(new Paragraph($"Замовлення №{analysisResult.OrderAnalysis?.ClientOrder?.Number}", headerFont));

        // Split long analysis names into multiple lines
        var analysisName = analysisResult.OrderAnalysis?.Analysis?.Name ?? "Не вказано";
        document.Add(new Paragraph(analysisName, analysisNameFont));

        document.Add(new Paragraph(" "));

        // 2. Date and Analysis Center section
        document.Add(new Paragraph("Дата виконання та Центр аналізу", sectionFont));

        var executionTable = new PdfPTable(2)
        {
            WidthPercentage = 100,
            SpacingBefore = 5f,
            SpacingAfter = 10f
        };

        // Configure cell padding for all cells
        var cellPadding = 5f;

        // Table headers
        var headerCell = new PdfPCell(new Phrase("Дата виконання", tableHeaderFont));
        headerCell.BackgroundColor = new BaseColor(79, 129, 189);
        headerCell.Padding = cellPadding;
        executionTable.AddCell(headerCell);

        headerCell = new PdfPCell(new Phrase("Центр аналізу", tableHeaderFont));
        headerCell.BackgroundColor = new BaseColor(79, 129, 189);
        headerCell.Padding = cellPadding;
        executionTable.AddCell(headerCell);

        // Table content with consistent padding
        var contentCell = new PdfPCell(new Phrase(
            analysisResult.ExecutionDate?.ToString("HH:mm, dd.MM.yyyy") ?? "Не вказано",
            normalFont));
        contentCell.Padding = cellPadding;
        executionTable.AddCell(contentCell);

        var centerAddress = analysisResult.AnalysisCenter != null
            ? $"{analysisResult.AnalysisCenter.City?.CityName}, {analysisResult.AnalysisCenter.Address}"
            : "Не вказано";
        contentCell = new PdfPCell(new Phrase(centerAddress, normalFont));
        contentCell.Padding = cellPadding;
        executionTable.AddCell(contentCell);

        document.Add(executionTable);

        // 3. Results section
        document.Add(new Paragraph("Результат аналізу", sectionFont));

        var resultsTable = new PdfPTable(2)
        {
            WidthPercentage = 100,
            SpacingBefore = 5f,
            SpacingAfter = 10f
        };

        // Table headers
        headerCell = new PdfPCell(new Phrase("Показник", tableHeaderFont));
        headerCell.BackgroundColor = new BaseColor(79, 129, 189);
        headerCell.Padding = cellPadding;
        resultsTable.AddCell(headerCell);

        headerCell = new PdfPCell(new Phrase("Опис", tableHeaderFont));
        headerCell.BackgroundColor = new BaseColor(79, 129, 189);
        headerCell.Padding = cellPadding;
        resultsTable.AddCell(headerCell);

        // Table content
        contentCell = new PdfPCell(new Phrase(
            analysisResult.Indicator?.ToString() ?? "Не вказано",
            normalFont));
        contentCell.Padding = cellPadding;
        resultsTable.AddCell(contentCell);

        contentCell = new PdfPCell(new Phrase(
            analysisResult.Description ?? "Не вказано",
            normalFont));
        contentCell.Padding = cellPadding;
        resultsTable.AddCell(contentCell);

        document.Add(resultsTable);

        // 4. Client information section
        document.Add(new Paragraph("Інформація про клієнта", sectionFont));

        var clientTable = new PdfPTable(2)
        {
            WidthPercentage = 100,
            SpacingBefore = 5f
        };

        var client = analysisResult.OrderAnalysis?.ClientOrder?.Client;
        if (client != null)
        {
            // Row 1 - Name and Phone
            headerCell = new PdfPCell(new Phrase("ПІБ", tableHeaderFont));
            headerCell.BackgroundColor = new BaseColor(79, 129, 189);
            headerCell.Padding = cellPadding;
            clientTable.AddCell(headerCell);

            headerCell = new PdfPCell(new Phrase("Телефон", tableHeaderFont));
            headerCell.BackgroundColor = new BaseColor(79, 129, 189);
            headerCell.Padding = cellPadding;
            clientTable.AddCell(headerCell);

            contentCell = new PdfPCell(new Phrase($"{client.FirstName} {client.LastName}", normalFont));
            contentCell.Padding = cellPadding;
            clientTable.AddCell(contentCell);

            contentCell = new PdfPCell(new Phrase(client.PhoneNumber ?? "Не вказано", normalFont));
            contentCell.Padding = cellPadding;
            clientTable.AddCell(contentCell);

            // Row 2 - Birthdate and Sex
            headerCell = new PdfPCell(new Phrase("Дата народження", tableHeaderFont));
            headerCell.BackgroundColor = new BaseColor(79, 129, 189);
            headerCell.Padding = cellPadding;
            clientTable.AddCell(headerCell);

            headerCell = new PdfPCell(new Phrase("Стать", tableHeaderFont));
            headerCell.BackgroundColor = new BaseColor(79, 129, 189);
            headerCell.Padding = cellPadding;
            clientTable.AddCell(headerCell);

            contentCell = new PdfPCell(new Phrase(
                client.Birthdate.ToString("dd.MM.yyyy") ?? "Не вказано",
                normalFont));
            contentCell.Padding = cellPadding;
            clientTable.AddCell(contentCell);

            contentCell = new PdfPCell(new Phrase(
                client.Sex?.SexName ?? "Не вказано",
                normalFont));
            contentCell.Padding = cellPadding;
            clientTable.AddCell(contentCell);
        }
        else
        {
            contentCell = new PdfPCell(new Phrase("Інформація про клієнта відсутня", normalFont));
            contentCell.Colspan = 2;
            contentCell.Padding = cellPadding;
            clientTable.AddCell(contentCell);
        }

        document.Add(clientTable);

        document.Close();
        writer.Close();

        memoryStream.Position = 0;
        return memoryStream;
    }

}