namespace DemoWebApplication.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Data;
    using Data.Models;

    public class ProblemsService : IProblemsService
    {
        private readonly DemoDbContext db;

        public ProblemsService(DemoDbContext db)
        {
            this.db = db;
        }

        public void CreateProblem(string header, string description, string creatorId)
        {
            Problem problem = new Problem
            {
                Id = Guid.NewGuid().ToString(),
                Header = header,
                Description = description,
                CreatedOn = DateTime.UtcNow,
                IsSolved = false,
                CreatorId = creatorId,
            };

            this.db.Problems.Add(problem);
            this.db.SaveChanges();
        }

        public void SolveProblem(string id)
        {
            Problem problem = this.db.Problems.Find(id);

            problem.IsSolved = !problem.IsSolved;

            this.db.SaveChanges();
        }

        public IEnumerable<Problem> GetAllProblemsWithUsers()
        {
            return this.db.Problems
                .Select(p => new Problem
                {
                    Id = p.Id,
                    Header = p.Header,
                    Description = p.Description,
                    Creator = p.Creator,
                    CreatedOn = p.CreatedOn,
                    IsSolved = p.IsSolved,
                })
                .ToArray();
        }

        public bool ProblemWithIdExists(string id)
        {
            return this.db.Problems.Find(id) != null;
        }
    }
}
