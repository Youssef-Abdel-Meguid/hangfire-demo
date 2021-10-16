using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace hangfire_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        public IActionResult Welcome() 
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome"));
            return Ok($"Job ID: {jobId}, welcome email sent to the user");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            int time = 30;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("welcome"), TimeSpan.FromSeconds(time));
            return Ok($"Job ID: {jobId}, discount email will be sent in {time} seconds");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("database update"), Cron.Minutely);
            return Ok("database check job initiated");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            int time = 30;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to be unsubscribed"), TimeSpan.FromSeconds(time));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribed"));
            return Ok("Confirmation job created");
        }

        public void SendWelcomeEmail(string message)
        {
            Console.WriteLine(message);
        }

    }
}
