﻿using OJS.Services.Busines.Submissions.Models;

namespace OJS.Services.Ui.Models.Submissions
{
    public class ExecutionResultResponseModel
    {
        public string Id { get; set; }

        public bool IsCompiledSuccessfully { get; set; }

        public string CompilerComment { get; set; }

        public OutputResultResponseModel OutputResult { get; set; }

        public TaskResultResponseModel TaskResult { get; set; }
    }
}