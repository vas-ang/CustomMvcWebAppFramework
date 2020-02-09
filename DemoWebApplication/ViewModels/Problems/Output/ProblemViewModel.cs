namespace DemoWebApplication.ViewModels.Problems.Output
{
    using System;

    public class ProblemViewModel
    {
        public string Id { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsSolved { get; set; }

        public string Creator { get; set; }
    }
}
