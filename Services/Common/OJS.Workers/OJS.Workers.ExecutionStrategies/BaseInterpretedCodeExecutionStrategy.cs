﻿namespace OJS.Workers.ExecutionStrategies
{
    using System;
    using OJS.Workers.Common;
    using OJS.Workers.Common.Models;
    using OJS.Workers.Executors;

    using static OJS.Workers.Common.Constants;

    public class BaseInterpretedCodeExecutionStrategy<TSettings> : BaseCodeExecutionStrategy<TSettings>
        where TSettings : BaseInterpretedCodeExecutionStrategySettings
    {
        protected BaseInterpretedCodeExecutionStrategy(
            ExecutionStrategyType type,
            IProcessExecutorFactory processExecutorFactory,
            IExecutionStrategySettingsProvider settingsProvider)
            : base(type, processExecutorFactory, settingsProvider)
        {
        }

        protected static string PrepareTestInput(string testInput)
            => string.Join(
                Environment.NewLine,
                testInput.Split(new[] { NewLineUnix, NewLineWin }, StringSplitOptions.None));

        protected override Task<IExecutionResult<TResult>> InternalExecute<TInput, TResult>(
            IExecutionContext<TInput> executionContext,
            IExecutionResult<TResult> result)
        {
            result.IsCompiledSuccessfully = true;

            return base.InternalExecute(executionContext, result);
        }
    }

    public abstract record BaseInterpretedCodeExecutionStrategySettings(
        int BaseTimeUsed, int BaseMemoryUsed)
        : BaseCodeExecutionStrategySettings(BaseTimeUsed, BaseMemoryUsed);
}
