using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IPaymentRecordService _paymentRecordService;
        private readonly ITaxYearService _taxYearService;
        private readonly IToastNotification _toast;

        public PaymentSlipController(
            IEmployeeService employeeService,
            IMapper mapper,
            IPaymentRecordService paymentRecordService,
            ITaxYearService taxYearService,
            IToastNotification toast
        )
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _paymentRecordService = paymentRecordService;
            _taxYearService = taxYearService;
            _toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Index));
            // return await Task.FromResult<IActionResult>(View());
        }

        [HttpGet]
        public async Task<IActionResult> CreatePaymentSlip()
        {
            ViewBag.Employees = new SelectList(await _employeeService.GetEmployeeListAsync(), "Id", "Name");
            ViewBag.TaxYear = new SelectList(await _taxYearService.GetTaxYearList(), "Id", "YearOfTax");
            var viewModel = new PaymentRecordViewModel();
            return await Task.FromResult<IActionResult>(View(viewModel));
        }

        //[Route("/employee/create")]
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
    }
}
