using Payroll.Controllers.ViewModels;

namespace Payroll.Controllers.Options;

public class NewsFilterOptions
{
  public string? Title { get; set; }

  public string? Author { get; set; }

  public string? CreateDate { get; set; }

  public string? PublishDate { get; set; }

  public string? UpdateDate { get; set; }

  public string? Activity { get; set; }

  public IEnumerable<NewsViewModel> ApplyFilter(IEnumerable<NewsViewModel> newsList)
  {
    if (!String.IsNullOrEmpty(Title))
    {
      newsList = newsList.Where(vm => vm.Title!.Equals(Title.Trim())).ToList();
    }

    if (!String.IsNullOrEmpty(Author))
    {
      newsList = newsList.Where(vm => vm.Author!.UserName!.Trim().Equals(Author.Trim())).ToList();
    }

    if (!String.IsNullOrEmpty(CreateDate))
    {
      newsList = newsList.Where(vm => vm.CreatedAt.ToString("dd.MM.yyyy").Equals(CreateDate)).ToList();
    }

    if (!String.IsNullOrEmpty(PublishDate))
    {
      newsList = newsList.Where(vm => vm.PublishedAt.ToString("dd.MM.yyyy").Equals(PublishDate)).ToList();
    }

    if (!String.IsNullOrEmpty(UpdateDate))
    {
      newsList = newsList.Where(vm => vm.UpdatedAt.ToString("dd.MM.yyyy").Equals(UpdateDate)).ToList();
    }

    if (!String.IsNullOrEmpty(Activity))
    {
      bool isSelectedActivity = bool.Parse(Activity);
      newsList = newsList.Where(vm => vm.IsActive == isSelectedActivity).ToList();
    }

    return newsList;
  }
}
