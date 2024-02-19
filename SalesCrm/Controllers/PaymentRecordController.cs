using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SalesCrm.Controllers.Options;
using SalesCrm.Controllers.Providers;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Utils.Reports;

namespace SalesCrm.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class PaymentRecordController : BaseController
    {
        readonly IEmployeeService _employeeService;
        readonly IMapper _mapper;
        readonly IPaymentRecordService _paymentRecordService;
        readonly IPaymentSlipService _paymentSlipService;
        readonly ITaxYearService _taxYearService;
        readonly IToastNotification _toast;
        readonly InvoiceService _invoiceService;
        readonly IHttpStatusCodeDescriptionProvider _httpStatusProvider;


        public PaymentRecordController
        (
            IEmployeeService employeeService,
            IMapper mapper,
            IPaymentRecordService paymentRecordService,
            ITaxYearService taxYearService,
            IToastNotification toast,
            IPaymentSlipService paymentSlipService,
            InvoiceService invoiceService,
            IHttpStatusCodeDescriptionProvider httpStatusProvider
        )
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _paymentRecordService = paymentRecordService;
            _taxYearService = taxYearService;
            _toast = toast;
            _paymentSlipService = paymentSlipService;
            _invoiceService = invoiceService;
            _httpStatusProvider = httpStatusProvider;
        }

        [HttpGet]
        [Route("/payment-record")]
        public async Task<IActionResult> Index(SearchOptions searcher)
        {
            string keyword = ViewBag.keyword = searcher.Keyword!;
            
            try
            {
                var paymentRecordList = _paymentRecordService.GetPaymentRecordListAsync(keyword)
                    .Result
                    .Select(dto => _mapper.Map<PaymentRecordViewModel>(dto))
                    .ToList();

                return await Task.FromResult<IActionResult>(View(paymentRecordList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpGet]
        [Route("/payment-record/create")]
        public async Task<IActionResult> CreatePaymentRecord()
        {
            try
            {
                ViewBag.Employees = new SelectList(await _employeeService.GetEmployeeListAsync(null!), "Id", "Name");
                ViewBag.TaxYears = new SelectList(await _taxYearService.GetTaxYearListAsync(), "Id", "YearOfTax");
                
                var viewModel = new PaymentRecordViewModel();
                return await Task.FromResult<IActionResult>(View(viewModel));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        [Route("/payment-record/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentRecordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dto = await _employeeService.GetEmployeeByIdAsync(viewModel.EmployeeId);
                    await ComputePaymentRecord(viewModel, dto.InsuranceNumber!);
                    var transfer = _mapper.Map<PaymentRecordDto>(viewModel);
                    await _paymentRecordService.CreatePaymentRecord(transfer);
                    _toast.AddSuccessToastMessage("Payment Record created successfully");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    _toast.AddErrorToastMessage("Error creating new Payment Record");
                }
            }
            else
            {
                ModelState.Values
                    .SelectMany(v => v.Errors)
                    .ToList()
                    .ForEach(error => Logger.LogError(error.ErrorMessage));

                return View("CreatePaymentRecord");
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task ComputePaymentRecord(PaymentRecordViewModel vm, string insuranceNumber)
        {
            vm.InsuranceNumber = insuranceNumber;
            vm.OvertimeHours = _paymentSlipService.GetOvertimeHorse(vm.HoursWorked, vm.ContractualHours);

            vm.ContractualEarnings =
                _paymentSlipService.GetContractualEarnings(vm.HoursWorked, vm.ContractualHours, vm.HourlyRate);

            decimal overtimeRate = _paymentSlipService.GetOvertimeRate(vm.HourlyRate);
            vm.OvertimeEarnings = _paymentSlipService.GetOvertimeEarning(vm.OvertimeHours, overtimeRate);
            vm.TotalEarnings = _paymentSlipService.GetTotalEarning(vm.OvertimeEarnings, vm.ContractualEarnings);
            vm.UnionFree = await _employeeService.GetUnionFree(vm.EmployeeId);
            vm.Tax = _taxYearService.GetTotalTax(vm.TotalEarnings);
            vm.TotalDeduction = await _paymentSlipService.GetTotalDeductionAsync(vm.UnionFree, vm.Tax);
            vm.NetPayment = await _paymentSlipService.GetNetPaymentAsync(vm.TotalEarnings, vm.TotalDeduction);
        }

        public async Task<IActionResult> GenerateInvoicePdf(Guid paymentRecordId)
        {
            try
            {
                if (paymentRecordId != Guid.Empty)
                {
                    return await _invoiceService.GenerateInvoicePdf(paymentRecordId);
                }
                else
                {
                    throw new HttpRequestException("Invalid params", null, HttpStatusCode.BadRequest);
                }
            }
            catch (HttpRequestException ex)
            {
                int? statusCode = (int?) ex.StatusCode;

                if (statusCode.HasValue)
                {
                    string statusDescription = _httpStatusProvider.GetStatusDescription(statusCode.Value)!;

                    return RedirectToAction("Error", new
                    {
                        statusCode = statusCode.Value,
                        message = statusDescription
                    });
                }
                else
                {
                    return RedirectToAction(nameof(Error));
                }
            }
        }

        [HttpGet]
        [Route("/payment-record/delete/{id}")]
        public async Task<IActionResult> DeletePaymentRecord(Guid id)
        {
            try
            {
                await _paymentRecordService.DeletePaymentRecordAsync(id);
                _toast.AddSuccessToastMessage("Payment Record created successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                _toast.AddErrorToastMessage("Error deleted Payment Record");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
