namespace DemoWebApplication.Controllers
{
    using System;
    using System.Collections.Generic;

    using CustomFramework.Http;

    using CustomFramework.Mvc;

    using ViewModels;

    public class ProblemsController : Controller
    {
        public HttpResponse List(HttpRequest httpRequest)
        {
            List<ProblemViewModel> problems = new List<ProblemViewModel>()
            {
                 new ProblemViewModel()
                 {
                     Creator = "Pesho",
                     Header = "Dad, please fix the car!",
                     Description = "Dad, please fix the car! I wanna drift!",
                     CreatedOn = DateTime.UtcNow.AddDays(-10),
                     IsSolved = true,
                 },
                 new ProblemViewModel()
                 {
                     Creator = "Some mother",
                     Header = "Clean the dishes!",
                     Description = "Clean the dishes or you are grounded!",
                     CreatedOn = DateTime.UtcNow,
                     IsSolved = false,
                 },
            };

            return this.View(problems);
        }
    }
}
