using FunnyWebRazor.Data;
using FunnyWebRazor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FunnyWebRazor.Pages.Worklogs
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public string SortOrder { get; set; }
        public string SearchString { get; set; }

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Worklog> Worklogs { get; set; } = new List<Worklog>();

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            SortOrder = sortOrder;
            SearchString = searchString;

            var worklogsQuery = _context.Worklogs
                .Include(w => w.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                worklogsQuery = worklogsQuery.Where(w => w.User.FullName.Contains(SearchString) ||
                                                         w.TaskName.Contains(SearchString));
            }

            worklogsQuery = sortOrder switch
            {
                "name_desc" => worklogsQuery.OrderByDescending(w => w.User.FullName),
                "name_asc" => worklogsQuery.OrderBy(w => w.User.FullName),
                "task_desc" => worklogsQuery.OrderByDescending(w => w.TaskName),
                "task_asc" => worklogsQuery.OrderBy(w => w.TaskName),
                "description_desc" => worklogsQuery.OrderByDescending(w => w.Description),
                "description_asc" => worklogsQuery.OrderBy(w => w.Description),
                "start_desc" => worklogsQuery.OrderByDescending(w => w.StartTime),
                "start_asc" => worklogsQuery.OrderBy(w => w.StartTime),
                "end_desc" => worklogsQuery.OrderByDescending(w => w.EndTime),
                "end_asc" => worklogsQuery.OrderBy(w => w.EndTime),
                _ => worklogsQuery.OrderBy(w => w.User.FullName) // Domy�lne sortowanie
            };

            Worklogs = await worklogsQuery.ToListAsync();
        }
    }
}
