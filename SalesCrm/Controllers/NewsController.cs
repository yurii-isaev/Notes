using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
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
    public async Task<IActionResult> Index
    (
        int pageNumber = 1,
        int pageSize = 7,
        string sortOrder           = null!,
        string selectedTitle       = null!,
        string selectedAuthor      = null!,
        string selectedCreateDate  = null!,
        string selectedPublishDate = null!,
        string selectedUpdateDate  = null!,
        string selectedActivity    = null!
    )
    {
        try
        {
            IEnumerable<NewsViewModel> news = _newsService.GetNewsListAsync()
                .Result
                .Select(news => _mapper.Map<NewsViewModel>(news))
                .ToList();

            if (!String.IsNullOrEmpty(selectedTitle))
            {
                news = news.Where(vm => vm.Title!.Equals(selectedTitle.Trim())).ToList();
            }

            var uniqueTitleList = news
                .Where(vm => string.IsNullOrEmpty(selectedActivity) || vm.IsActive == bool.Parse(selectedActivity))
                // .Where(viewModel => viewModel.Title != null)
                .GroupBy(vm => vm.Title)
                .OrderBy(newGroup => newGroup.Key)
                .Select(newGroup => new {Title = newGroup.Key});

            ViewBag.UniqueTitle = uniqueTitleList
                .Select(s => new SelectListItem 
                {
                    Value = s.Title, Text = s.Title
                });

            ViewBag.SelectedTitle = selectedTitle;

            // ...

            if (!String.IsNullOrEmpty(selectedAuthor))
            {
                news = news.Where(vm => vm.Author!.UserName!.Trim().Equals(selectedAuthor.Trim())).ToList();
            }

            var uniqueAuthorList = news
                .Where(vm => vm.Author != null)
                .GroupBy(vm => vm.Author)
                .OrderBy(newGroup => newGroup.Key)
                .Select(newGroup => new {Author = newGroup.Key});

            ViewBag.UniqueAuthor = uniqueAuthorList
                .Select(s => new SelectListItem 
                {
                    Value = s.Author!.UserName, Text = s.Author.UserName
                });

            ViewBag.SelectedAuthor = selectedAuthor;

            // ...

            if (!String.IsNullOrEmpty(selectedCreateDate))
            {
                news = news.Where(vm => vm.CreatedAt.ToString("dd.MM.yyyy").Equals(selectedCreateDate)).ToList();
            }

            var uniqueCreateDateList = news
                .Where(vm => string.IsNullOrEmpty(selectedActivity) || vm.IsActive == bool.Parse(selectedActivity))
                .Select(vm => vm.CreatedAt.Date)
                .Distinct()
                .OrderByDescending(date => date)
                .ToList();

            ViewBag.UniqueCreateDate = uniqueCreateDateList
                .Select(date => new SelectListItem
                {
                    Value = date.ToString("dd.MM.yyyy"),
                    Text = date.ToString("dd.MM.yyyy")
                });

            ViewBag.SelectedCreateDate = selectedCreateDate;

            // ...

            if (!String.IsNullOrEmpty(selectedPublishDate))
            {
                news = news.Where(vm => vm.PublishedAt.ToString("dd.MM.yyyy").Equals(selectedPublishDate)).ToList();
            }

            var uniquePublishDates = news
                .Select(vm => vm.PublishedAt.Date)
                .Distinct()
                .OrderByDescending(date => date)
                .ToList();

            ViewBag.UniquePublishDate = uniquePublishDates
                .Select(date => new SelectListItem
                {
                    Value = date.ToString("dd.MM.yyyy"),
                    Text = date.ToString("dd.MM.yyyy")
                });

            ViewBag.SelectedPublishDate = selectedPublishDate;

            // ...

            //var filteredNews = news; // Сохраняем отфильтрованный список новостей

            if (!String.IsNullOrEmpty(selectedUpdateDate))
            {
                news = news.Where(vm => vm.UpdatedAt.ToString("dd.MM.yyyy").Equals(selectedUpdateDate)).ToList();
            }

            var uniqueUpdateList = news
                .Where(vm => string.IsNullOrEmpty(selectedActivity) || vm.IsActive == bool.Parse(selectedActivity))
                .Select(vm => vm.UpdatedAt.Date)
                .Distinct()
                .OrderByDescending(date => date)
                .ToList();

            ViewBag.UniqueUpdateDate = uniqueUpdateList
                .Select(date => new SelectListItem
                {
                    Value = date.ToString("dd.MM.yyyy"),
                    Text = date.ToString("dd.MM.yyyy")
                });

            ViewBag.SelectedUpdateDate = selectedUpdateDate;

            // ...


            if (!String.IsNullOrEmpty(selectedActivity))
            {
                bool isSelectedActivity = bool.Parse(selectedActivity);
                news = news.Where(vm => vm.IsActive == isSelectedActivity).ToList();
            }

            var uniqueActivity = news
                .GroupBy(vm => vm.IsActive)
                .OrderBy(newGroup => newGroup.Key)
                .Select(newGroup => new {IsActive = newGroup.Key})
                .Select(vm => vm.IsActive);

            ViewBag.UniqueActivity = uniqueActivity
                .Select(v => new SelectListItem
                {
                    Value = v.ToString().ToLower(),
                    Text = v.ToString().ToLower()
                });

            ViewBag.SelectedActivity = selectedActivity;

            // Sort the news list by the PublishAt field in descending date order
            IOrderedEnumerable<NewsViewModel> orderedNews = news.OrderByDescending(s => s.PublishedAt);

            var paginationList = PaginationList<NewsViewModel>.Create(orderedNews.ToList(), pageNumber, pageSize);

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
