using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Flight_Doc_Manager_Systems.RestControllers
{
    [Route("api/userlist")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserListController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var uerlist = await Task.FromResult(new string[] { "Virat", "Messi", "Ozil", "Lara", "MS Dhoni" });
            return Ok(uerlist);

        }
    }
}
