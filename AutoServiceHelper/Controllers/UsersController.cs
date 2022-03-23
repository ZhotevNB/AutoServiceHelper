using Microsoft.AspNetCore.Mvc;

namespace AutoServiceHelper.Controllers
{
    public class UsersController : BaseController
    {

       
        public IActionResult Settings()=> View();
        
    }
}
