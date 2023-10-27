using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Controllers
{
    public class TaxYearController : Controller
    {
        private readonly ILogger<TaxYearController> _logger;
        private readonly IMapper _mapper;
        private readonly ITaxYearService _taxService;
        private readonly IToastNotification _toast;

        public TaxYearController
        (
            ILogger<TaxYearController> logger,
            IMapper mapper,
            ITaxYearService taxService,
            IToastNotification toast
        )
        {
            _logger = logger;
            _mapper = mapper;
            _taxService = taxService;
            _toast = toast;
        }

        [HttpGet]
        public Task<IActionResult> Index()
        {
            var taxYear = _taxService.GetTaxYearList().Result.Select(tax => _mapper.Map<TaxYearViewModel>(tax));
            
            return Task.FromResult<IActionResult>(View(taxYear));
        }

        // GET: TaxYear/Details/5
        public ActionResult Details(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult CreateTaxYear()
        {
            return View(new TaxYearViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaxYearViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<TaxYearDto>(viewModel);
                    await _taxService.CreateTaxYearAsync(dto);
                    _toast.AddSuccessToastMessage("Tax Year successfully created");
                    // return RedirectToAction("Index");
                }
            }

            catch (Exception ex)
            {
                _logger.LogError("[Exception create Tax Year]: " + ex.Message);
                _toast.AddErrorToastMessage("Error creating new Tax Year");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TaxYear/Edit/5
        public ActionResult Edit(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        // POST: TaxYear/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: TaxYear/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        // POST: TaxYear/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
