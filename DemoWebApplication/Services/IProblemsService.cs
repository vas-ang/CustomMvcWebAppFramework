namespace DemoWebApplication.Services
{
    using System.Collections.Generic;

    using Data.Models;

    public interface IProblemsService
    {
        void CreateProblem(string header, string description, string creatorId);

        IEnumerable<Problem> GetAllProblemsWithUsers();

        void SolveProblem(string id);

        bool ProblemWithIdExists(string id);
    }
}
