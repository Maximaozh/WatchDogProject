using Dapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatchDog
{


    class ReportGenerator
    {

        public void GenerateReport(IEnumerable<CheckDataBaseItem> records, string filePath)
        {
            using (var document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = document.AddMainDocumentPart();
                var body = new Body();

                mainPart.Document = new Document();
                mainPart.Document.Append(body);

                var title = new Paragraph(new Run(new Text("Отчет по результатам проверок")));
                title.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
                body.Append(title);

                var emptyParagraph = new Paragraph(new Run(new Text("")))
                {
                    ParagraphProperties = new ParagraphProperties(
                        new SpacingBetweenLines() { After = "200" } // Установить отступ после абзаца
                    )
                };
                body.Append(emptyParagraph);

                if (records == null || !records.Any())
                {
                    body.Append(new Paragraph(new Run(new Text("Нет доступных данных для отображения."))));
                }
                else
                {
                    foreach (var record in records)
                    {
                        body.Append(new Paragraph(new Run(new Text($"ID: {record.Id}, Address: {record.Address}, IsAlive: {record.IsAlive}, ResponseTime: {record.ResponseTime}, ExtraMessage: {record.ExtraMessage}, Timestamp: {record.Timestamp}"))));
                    }
                }

                mainPart.Document.Save();
            }
        }



        public void GenerateTimeWorkReport(IEnumerable<CheckResultsReport> records, string filePath, string date)
        {
            using (var document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = document.AddMainDocumentPart();
                var body = new Body();

                mainPart.Document = new Document();
                mainPart.Document.Append(body);

                var title = new Paragraph(new Run(new Text("Отчет по времени работы"+ date)));
                title.ParagraphProperties = new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center });
                body.Append(title);

                var emptyParagraph = new Paragraph(new Run(new Text("")))
                {
                    ParagraphProperties = new ParagraphProperties(
                        new SpacingBetweenLines() { After = "200" })
                };
                body.Append(emptyParagraph);

                if (records == null || !records.Any())
                {
                    body.Append(new Paragraph(new Run(new Text("Нет доступных данных для отображения."))));
                }
                else
                {
                    body.Append(new Paragraph(new Run(new Text("Адрес\t\tКоличество проверок\tДоступность (%)"))));
                    body.Append(new Paragraph(new Run(new Text("--------------------------------------------------------------"))));

                    foreach (var record in records)
                    {
                        body.Append(new Paragraph(new Run(new Text($"{record.Address}\t{record.TotalChecks}\t{record.AvailabilityPercentage:0.00}"))));
                    }
                }

                mainPart.Document.Save();
            }
        }
    }
}