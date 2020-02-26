using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WildHare.Web.Pages
{
    public class TakeRandom : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
}
