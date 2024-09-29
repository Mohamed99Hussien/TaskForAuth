using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TaskForAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet]
        [Route("Any")]
        public async Task<ActionResult> Anonymous()
        {

            return Content("Hello Anynomous");
        }
        [HttpGet]
        [Authorize(Policy = "Teacher")]
        public async Task<ActionResult> Teacher()
        {

            return Content("Hello teacher");
        }
        [HttpGet]
        [Authorize(Policy ="Student")]
        public async Task<ActionResult> Student()
        {

            return Content("Hello student");
        }
        [HttpGet]
        [Authorize(Policy = "Teacher , Student")]
        public async Task<ActionResult> TeacherAndStudent()
        {

            return Content("Hello teacher and student");
        }
    }
}
