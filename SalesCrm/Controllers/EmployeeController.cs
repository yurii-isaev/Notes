using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Views;

namespace SalesCrm.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(EmployeeService service, IMapper mapper)
    {
        _employeeService = service;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        List<EmployeeListViewModel> list = _employeeService.GetEmployeeListAsync()
            .Result
            .Select(emp => _mapper.Map<EmployeeListViewModel>(emp))
            .ToList();

        var paginationList = PaginationList<EmployeeListViewModel>.Create(list, pageNumber, pageSize);

        return await Task.FromResult<IActionResult>(View(paginationList));
    }

    [Route("/employee/create")]
    [HttpGet]
    public async Task<IActionResult> CreateEmployee()
    {
        return await Task.FromResult<IActionResult>(View());
    }

    [Route("/employee/create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<EmployeeInputDto>(viewModel);
            await _employeeService.CreateEmployeeAsync(dto);
        }

        return RedirectToAction("Index");
    }
}
