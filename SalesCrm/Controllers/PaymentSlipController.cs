using AutoMapper;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Controllers
{
    public class PaymentSlipController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<PaymentSlipController> _logger;
        private readonly IMapper _mapper;
        private readonly IPaymentRecordService _paymentRecordService;
        private readonly ITaxYearService _taxYearService;
        private readonly IToastNotification _toast;

        public PaymentSlipController
        (
            IEmployeeService employeeService,
            IMapper mapper,
            IPaymentRecordService paymentRecordService,
            ITaxYearService taxYearService,
            IToastNotification toast,
            ILogger<PaymentSlipController> logger
        )
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _paymentRecordService = paymentRecordService;
            _taxYearService = taxYearService;
            _toast = toast;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var list = _paymentRecordService.GetPaymentRecordList()
                .Result
                .Select(dto => _mapper.Map<PaymentRecordViewModel>(dto))
                .ToList();

            return await Task.FromResult<IActionResult>(View(list));
        }

        [HttpGet]
        public async Task<IActionResult> CreatePaymentSlip()
        {
            ViewBag.Employees = new SelectList(await _employeeService.GetEmployeeListAsync(), "Id", "Name");
            ViewBag.TaxYear = new SelectList(await _taxYearService.GetTaxYearList(), "Id", "YearOfTax");
            var viewModel = new PaymentRecordViewModel();
            return await Task.FromResult<IActionResult>(View(viewModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentRecordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dto = await _employeeService.GetEmployeeByIdAsync(vm.EmployeeId) ?? throw new NullReferenceException();
                    vm.InsuranceNumber = dto.InsuranceNumber;

                    vm.OvertimeHours = _paymentRecordService.GetOvertimeHorse(vm.HoursWorked, vm.ContractualHours);
                    vm.ContractualEarnings = _paymentRecordService.GetContractualEarnings(vm.HoursWorked, vm.ContractualHours, vm.HourlyRate);

                    decimal overtimeRate = _paymentRecordService.GetOvertimeRate(vm.HourlyRate);

                    vm.OvertimeEarnings = _paymentRecordService.GetOvertimeEarning(vm.OvertimeHours, overtimeRate);
                    vm.TotalEarnings = _paymentRecordService.GetTotalEarning(vm.OvertimeEarnings, vm.ContractualEarnings);
                    vm.UnionFree = await _employeeService.GetUnionFree(vm.EmployeeId);
                    vm.Tax = _taxYearService.GetTotalTax(vm.TotalEarnings);
                    vm.TotalDeduction = await _paymentRecordService.GetTotalDeductionAsync(vm.UnionFree, vm.Tax);
                    vm.NetPayment = await _paymentRecordService.GetNetPaymentAsync(vm.TotalEarnings, vm.TotalDeduction);

                    var transfer = _mapper.Map<PaymentRecordDto>(vm);
                    await _paymentRecordService.CreatePaymentRecord(transfer);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error creating new Payment Record");
                    _toast.AddErrorToastMessage("Error creating new Payment Record");
                }
            }

            ViewBag.Employees = new SelectList(await _employeeService.GetEmployeeListAsync(), "Id", "Name");
            ViewBag.TaxYear = new SelectList(await _taxYearService.GetTaxYearList(), "Id", "YearOfTax");
            return RedirectToAction("Index");
        }

        // Method to generate a PDF invoice document
        [HttpGet]
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
                        // Creating a Table
                        var table = new Table(2);

                        // Adding a Table Header
                        var headerCell = new Cell(1, 2).Add(new Paragraph("Invoice"));
                        table.AddCell(headerCell);

                        // Adding employee data to a table
                        table.AddCell("Employee Name");
                        table.AddCell(employeeData.Employee!.Name);
                        table.AddCell("Pay Date");
                        table.AddCell(employeeData.PayDate.ToString("dd/MM/yyyy"));
                        table.AddCell("Pay Month");
                        table.AddCell(employeeData.PayMonth);
                        table.AddCell("Tax Year");
                        table.AddCell(employeeData.TaxYear!.YearOfTax);
                        table.AddCell("Total Earnings");
                        table.AddCell(employeeData.TotalEarnings.ToString("C"));
                        table.AddCell("Total Deduction");
                        table.AddCell(employeeData.TotalDeduction.ToString("C"));
                        table.AddCell("Net Payment");
                        table.AddCell(employeeData.NetPayment.ToString("C"));

                        // Adding a Table to a Document
                        document.Add(table);
                    }
                }
            }

            byte[] file = memoryStream.ToArray();
            MemoryStream ms = new MemoryStream();
            ms.Write(file, 0, file.Length);
            ms.Position = 0;

            return File(ms, "application/pdf", "test_file_name" + ".pdf");
        }
        
        private async Task<PaymentRecordDto> GetEmployeeFormData(Guid paymentRecordId)
        {
            try
            {
                return await _paymentRecordService.GetEmployeePaymentRecordAsync(paymentRecordId);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Get Employee Form Data]: " + ex.Message);
                throw;
            }
        }
    }
}
