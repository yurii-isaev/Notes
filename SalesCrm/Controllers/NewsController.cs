using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SalesCrm.Controllers.Options;
using SalesCrm.Controllers.ViewModels;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;
using SalesCrm.Views.Components.Pagination;

namespace SalesCrm.Controllers;

[Authorize(Roles = "Admin")]
public class NewsController : Controller
{
    private readonly INewsService _newsService;
    private readonly IMapper _mapper;
    private readonly IToastNotification _toast;

    public NewsController(INewsService newsService, IMapper mapper, IToastNotification toast)
    {
        _newsService = newsService;
        _mapper = mapper;
        _toast = toast;
    }

    [Route("/admin/news")]
    [HttpGet]
    public async Task<IActionResult> Index(NewsParametersModel prms)
    {
        try
        {
            IEnumerable<NewsViewModel> newsList = _newsService.GetNewsListAsync()
                .Result
                .Select(news => _mapper.Map<NewsViewModel>(news))
                .ToList();

            // Unrefactorable code !!
            // because when changing the news List,
            // the logic of displaying the filter drop-down menu does not work in any way.

            var filterOptions = new NewsFilterOptions
            {
                Title = prms.SelectedTitle,
                Author = prms.SelectedAuthor,
                CreateDate = prms.SelectedCreateDate,
                PublishDate = prms.SelectedPublishDate,
                UpdateDate = prms.SelectedUpdateDate,
                Activity = prms.SelectedActivity
            };

            newsList = filterOptions.ApplyFilter(newsList);

            var uniqueTitleList = newsList
                .GroupBy(vm => vm.Title)
                .OrderBy(newGroup => newGroup.Key)
                .Select(newGroup => new {Title = newGroup.Key});

            // Сlosure function !!
            ViewBag.UniqueTitle = uniqueTitleList
                .Where(v => newsList.Any(vm => vm.Title == v.Title))
                .Select(v => new SelectListItem {Value = v.Title, Text = v.Title});

            ViewBag.SelectedTitle = prms.SelectedTitle!;

            // ...

            var uniqueAuthorList = newsList
                .Where(vm => vm.Author != null)
                .GroupBy(vm => vm.Author)
                .OrderBy(newGroup => newGroup.Key)
                .Select(newGroup => new {Author = newGroup.Key});

            // Сlosure function !!
            ViewBag.UniqueAuthor = uniqueAuthorList
                .Where(user => newsList.Any(vm => vm.Author == user.Author))
                .Select(s => new SelectListItem
                {
                    Value = s.Author!.UserName, Text = s.Author.UserName
                });

            ViewBag.SelectedAuthor = prms.SelectedAuthor!;

            // ...

            var uniqueCreateDateList = newsList
                .Select(vm => vm.CreatedAt.Date)
                .Distinct()
                .OrderByDescending(date => date)
                .ToList();

            // Сlosure function !!
            ViewBag.UniqueCreateDate = uniqueCreateDateList
                .Where(date => newsList.Any(vm => vm.CreatedAt.Date == date))
                .Select(date => new SelectListItem
                {
                    Value = date.ToString("dd.MM.yyyy"),
                    Text = date.ToString("dd.MM.yyyy")
                });

            ViewBag.SelectedCreateDate = prms.SelectedCreateDate!;

            // ...

            var uniquePublishDateList = newsList
                .Select(vm => vm.PublishedAt.Date)
                .Distinct()
                .OrderByDescending(date => date)
                .ToList();

            // Сlosure function !!
            ViewBag.UniquePublishDate = uniquePublishDateList
                .Where(date => newsList.Any(vm => vm.PublishedAt.Date == date))
                .Select(date => new SelectListItem
                {
                    Value = date.ToString("dd.MM.yyyy"),
                    Text = date.ToString("dd.MM.yyyy")
                });

            ViewBag.SelectedPublishDate = prms.SelectedPublishDate!;

            // ...

            var uniqueUpdateList = newsList
                .Where(vm =>
                    string.IsNullOrEmpty(prms.SelectedActivity) || vm.IsActive == bool.Parse(prms.SelectedActivity))
                .Select(vm => vm.UpdatedAt.Date)
                .Distinct()
                .OrderByDescending(date => date)
                .ToList();

            ViewBag.UniqueUpdateDate = uniqueUpdateList.Select(date => new SelectListItem
            {
                Value = date.ToString("dd.MM.yyyy"),
                Text = date.ToString("dd.MM.yyyy")
            });

            ViewBag.SelectedUpdateDate = prms.SelectedUpdateDate!;

            // ...

            var uniqueActivity = newsList
                .GroupBy(vm => vm.IsActive)
                .OrderBy(newGroup => newGroup.Key)
                .Select(newGroup => new {IsActive = newGroup.Key})
                .Select(vm => vm.IsActive);

            ViewBag.UniqueActivity = uniqueActivity.Select(v => new SelectListItem
            {
                Value = v.ToString().ToLower(),
                Text = v.ToString().ToLower()
            });

            ViewBag.SelectedActivity = prms.SelectedActivity!;

            // Sort the news list by the PublishAt field in descending date order
            IList<NewsViewModel> orderedNewsList = newsList.OrderByDescending(s => s.PublishedAt).ToList();

            var paginationList =
                PaginationList<NewsViewModel>.Create(orderedNewsList, prms.PageNumber, prms.PageSize);

            return await Task.FromResult<IActionResult>(View(paginationList));
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
    }

    [Route("/admin/news/create")]
    [HttpGet]
    public async Task<IActionResult> CreateNews()
    {
        return await Task.FromResult<IActionResult>(View());
    }

    [Route("/admin/news/create")]
    [HttpPost]
    public async Task<IActionResult> Create(NewsViewModel viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    viewModel.AuthorId = userId;
                    viewModel.CreatedAt = DateTime.SpecifyKind(viewModel.CreatedAt, DateTimeKind.Utc);

                    var dto = _mapper.Map<NewsDto>(viewModel);
                    await _newsService.CreateNewsAsync(dto);

                    _toast.AddSuccessToastMessage("News created successfully");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error creating news");
                _toast.AddErrorToastMessage("Error creating news");
            }
        }

        return RedirectToAction("Index");
    }

    [Route("/admin/news/edit/{id}")]
    [HttpGet]
    public async Task<IActionResult> EditNews(Guid id)
    {
        try
        {
            var dto = await _newsService.GetNewsItemAsync(id);
            var viewModel = _mapper.Map<NewsViewModel>(dto);
            return await Task.FromResult<IActionResult>(View(viewModel));
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
    }

    [Route("/admin/news/edit/{id}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(NewsViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var dto = _mapper.Map<NewsDto>(viewModel);
                await _newsService.UpdateNewsAsync(dto);

                _toast.AddSuccessToastMessage("News updated successfully");
            }
            catch
            {
                ModelState.AddModelError("", "Error updated news");
                _toast.AddErrorToastMessage("Error updated news");
                return RedirectToAction("Error");
            }
        }

        return RedirectToAction("Index");
    }

    [Route("/admin/news/delete/{id}")]
    [HttpGet]
    public async Task<IActionResult> DeleteNews(Guid id)
    {
        try
        {
            await _newsService.DeleteNewsAsync(id);
            _toast.AddSuccessToastMessage("News deleted successfully");
        }
        catch
        {
            ModelState.AddModelError("", "Error deleted news");
            _toast.AddErrorToastMessage("Error deleted news");
        }

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
