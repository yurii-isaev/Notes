using System.Globalization;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Utils.Reports;

namespace SalesCrm.Services;

public class InvoiceService
{
    readonly IPaymentRecordService _paymentRecordService;

    public InvoiceService(IPaymentRecordService paymentRecordService)
    {
        _paymentRecordService = paymentRecordService;
    }

    // Method to generate a PDF invoice document
    public async Task<IActionResult> GenerateInvoicePdf(Guid paymentRecordId)
    {
        // Retrieving Employee Data by Employee ID
        var employeeData = await GetEmployeeFormData(paymentRecordId);

        // Creating a MemoryStream Object to Save a PDF Document
        var memoryStream = new MemoryStream();

        // Creating a PdfWriter Object to Write to MemoryStream
        using (var writer = new PdfWriter(memoryStream))
        {
            // Creating a PdfDocument
            using (var pdf = new PdfDocument(writer))
            {
                // Creating a Document Object
                using (var document = new Document(pdf))
                {
                    float col = 300f;
                    float[] colwidth = {col, col};

                    // Address Table
                    Table address = new Table(colwidth);

                    // Table Header
                    var header = new Cell(1, 1)
                        .SetFontSize(14)
                        .SetBold()
                        .SetBorder(Border.NO_BORDER)
                        //.Add(logo)
                        .Add(new Paragraph("APP"));

                    var cell1 = new Cell(2, 1)
                        .SetFontSize(26)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("Salary receipt"));

                    var cell2 = new Cell(1, 1)
                        .SetItalic()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("FROM"))
                        .SetMarginBottom(20f);

                    var cell3 = new Cell(1, 1)
                        .SetFontSize(10)
                        .SetItalic()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph(
                            "Dummy Street \n Dummy City, 12345 \n" +
                            "00 11 22 33 44 55 \n company@mail.co | example.co"));

                    var cell4 = new Cell(1, 1)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetItalic()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("INVOICE # 15984125 \n DATE April 4, 2023"));


                    string employeeName = employeeData.Employee!.Name!;
                    string city = employeeData.Employee.City!;
                    string employeeAddress = employeeData.Employee.Address!;
                    string phone = employeeData.Employee.Phone!;

                    var cell5 = new Cell(1, 1)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetItalic()
                        .SetPaddingTop(20f)
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph(
                            $"TO \n MR. {employeeName} \n Company \n" +
                            $" {city},{employeeAddress} \n" +
                            $" {employeeName} \n {phone}"));

                    address.AddCell(header);
                    address.AddCell(cell1);
                    address.AddCell(cell2);
                    address.AddCell(cell3);
                    address.AddCell(cell4);
                    address.AddCell(cell5);
                    document.Add(address);

                    // Statement table
                    Table statement = new Table(colwidth);
                    statement.SetMarginTop(20f);

                    var total = new Cell(1, 1)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBold()
                        .Add(new Paragraph("Total"));

                    var totalValue = new Cell(1, 1)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBold()
                        .Add(new Paragraph(employeeData.NetPayment.ToString(CultureInfo.InvariantCulture)));

                    statement.AddCell("Employee Name");
                    statement.AddCell(employeeName);
                    statement.AddCell("Pay Date");
                    statement.AddCell(employeeData.PayDate.ToString("dd/MM/yyyy"));
                    statement.AddCell("Pay Month");
                    statement.AddCell(employeeData.PayMonth);
                    statement.AddCell("Tax Year");
                    statement.AddCell(employeeData.TaxYear!.YearOfTax);
                    statement.AddCell("Total Earnings");
                    statement.AddCell(employeeData.TotalEarnings.ToString(CultureInfo.InvariantCulture));
                    statement.AddCell("Total Deduction");
                    statement.AddCell(employeeData.TotalDeduction.ToString(CultureInfo.InvariantCulture));
                    statement.AddCell("Net Payment");
                    statement.AddCell(employeeData.NetPayment.ToString(CultureInfo.InvariantCulture));
                    statement.AddCell(total);
                    statement.AddCell(totalValue);
                    document.Add(statement);

                    // Footer Table
                    Table footer = new Table(colwidth);
                    footer.SetMarginTop(20f);

                    var footerValue = new Cell(1, 2)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph(
                            "Make all checks payable to Payroll. \n" +
                            " Payment is due within 30 day. \n" +
                            "If you have any questions concerning this invoice," +
                            " contact 00 11 22 33 44 55 | company@mail.co.")
                        );

                    var value = new Cell(1, 2)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(Border.NO_BORDER)
                        .SetPaddingTop(20f)
                        .Add(new Paragraph("THANKS YOU FOR YOUR BUSINESS !"));

                    footer.AddCell(footerValue);
                    footer.AddCell(value);
                    document.Add(footer);
                }
            }
        }

        byte[] file = memoryStream.ToArray();
        MemoryStream ms = new MemoryStream();
        ms.Write(file, 0, file.Length);
        ms.Position = 0;

        return new FileStreamResult(ms, "application/pdf")
        {
            FileDownloadName = "document.pdf"
        };
    }

    private async Task<PaymentRecordDto> GetEmployeeFormData(Guid paymentRecordId)
    {
        try
        {
            return await _paymentRecordService.GetPaymentRecordByIdAsync(paymentRecordId);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }
}
