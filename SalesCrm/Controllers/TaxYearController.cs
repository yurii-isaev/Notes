using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SalesCrm.Controllers.Providers;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Exceptions;
using SalesCrm.Services.Input;
using SalesCrm.Utils.Reports;

namespace SalesCrm.Controllers
{
    public class TaxYearController : Controller
    {
        readonly IHttpStatusCodeDescriptionProvider _httpStatusProvider;
        readonly IMapper _mapper;
        readonly ITaxYearService _taxService;
        readonly IToastNotification _toast;

        public TaxYearController
        (
            IHttpStatusCodeDescriptionProvider httpStatusProvider,
            IMapper mapper,
            ITaxYearService taxService,
            IToastNotification toast
        )
        {
            _httpStatusProvider = httpStatusProvider;
            _mapper = mapper;
            _taxService = taxService;
            _toast = toast;
        }

        [HttpGet]
        [Route("/tax-year")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var taxYear = _taxService.GetTaxYearList()
                    .Result
                    .Select(tax => _mapper.Map<TaxYearViewModel>(tax));

                return await Task.FromResult<IActionResult>(View(taxYear));
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
            catch (Exception ex) // Unexpected Exception 
            {
                Logger.LogError(ex);
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpGet]
        [Route("/tax-year/create")]
        public ActionResult CreateTaxYear()
        {
            return View(new TaxYearViewModel());
        }

        [HttpPost]
        [Route("/tax-year/create")]
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
                }
            }
            catch (TaxYearExistsException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("CreateTaxYear");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode, string? message)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode ?? 500,
                Message = message ?? "Internal Server Error"
            });
        }
    }
}
