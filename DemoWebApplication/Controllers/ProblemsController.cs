namespace DemoWebApplication.Controllers
{
    using System.Linq;

    using CustomFramework.Http;

    using CustomFramework.Mvc;
    using CustomFramework.Mvc.Attributes;

    using Services;
    using ViewModels.Problems.Input;
    using ViewModels.Problems.Output;

    public class ProblemsController : Controller
    {
        private readonly IProblemsService problemsService;

        public ProblemsController(IProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }

        public HttpResponse List()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var problems = this.problemsService.GetAllProblemsWithUsers().Select(p => new ProblemViewModel
            {
                Id = p.Id,
                Header = p.Header,
                Description = p.Description,
                IsSolved = p.IsSolved,
                CreatedOn = p.CreatedOn,
                Creator = p.Creator?.Username,
            })
                .ToList();

            return this.View(problems);
        }

        public HttpResponse Create()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(ProblemInputModel problemInput)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.problemsService.CreateProblem(problemInput.Header, problemInput.Description, this.User);

            return this.Redirect("/Problems/List");
        }

        public HttpResponse Solve(string id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!this.problemsService.ProblemWithIdExists(id))
            {
                return this.Error("Id does not exist!");
            }

            this.problemsService.SolveProblem(id);

            return this.Redirect("/Problems/List");
        }
    }
}
