namespace DemoWebApplication.ViewModels
{
    using System;

    public class ProblemViewModel
    {
        public string Header { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsSolved { get; set; }

        public string Creator { get; set; }
    }
}
