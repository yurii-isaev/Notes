using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Controllers
{
    public class PaymentSlipController : Controller
    {
        private IEmployeeService _employeeService;
        private ITaxYearService _taxYearService;
        private IPaymentRecordService _paymentRecordService;

        public PaymentSlipController
        (IEmployeeService employeeService, ITaxYearService taxYearService,
            IPaymentRecordService paymentRecordService
        )
        {
            _employeeService = employeeService;
            _taxYearService = taxYearService;
            _paymentRecordService = paymentRecordService;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Index));
            // return await Task.FromResult<IActionResult>(View());
        }

        [HttpGet]
        public async Task<IActionResult> CreatePaymentSlip()
        {
            ViewBag.Employees = new SelectList(_employeeService.GetEmployeeListAsync().Result, "Id", "Name");
            ViewBag.TaxYear = new SelectList(_taxYearService.GetTaxYearList(), "Id", "YearOfTax");
            var viewModel = new PaymentRecordViewModel();

            return await Task.FromResult<IActionResult>(View(viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentRecordViewModel vm)
        {
            vm.InsuranceNumber = _employeeService.GetEmployeeByIdAsync(vm.Id).Result.InsuranceNumber;

            vm.OvertimeHours = _paymentRecordService.GetOvertimeHorse(vm.HoursWorked, vm.ContractualHours);

            vm.ContractualEarnings = _paymentRecordService.GetContractualEarnings(
                vm.HoursWorked, vm.ContractualHours, vm.HourlyRate
            );

            decimal overtimeRate = _paymentRecordService.GetOvertimeRate(vm.HourlyRate);
                
            vm.OvertimeEarnings = _paymentRecordService.GetOvertimeEarning(vm.OvertimeHours, overtimeRate);

            vm.TotalEarnings = _paymentRecordService.GetTotalEarning(vm.OvertimeEarnings, vm.ContractualEarnings);

            return await RedirectToAction(nameof(Index));
        }
    }
}
