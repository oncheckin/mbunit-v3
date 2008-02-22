﻿using System;
using System.Reflection;
using Gallio.Logging;
using Gallio.Model;

namespace Gallio.Framework
{
    /// <summary>
    /// The <see cref="TestInvoker" /> provides methods for safely running
    /// actions as part of tests.
    /// </summary>
    public static class TestInvoker
    {
        /// <summary>
        /// Runs a particular action.  If the action throws an exception, this method
        /// catches it, logs it, and produces an appropriate <see cref="TestOutcome" />.
        /// </summary>
        /// <param name="action">The action to run</param>
        /// <param name="description">A description of the action being performed,
        /// to be used when reporting failures, or null if none</param>
        /// <returns>The outcome of the action, never null</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null</exception>
        public static TestOutcome Run(Action action, string description)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            try
            {
                action();
                return TestOutcome.Passed;
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                    ex = ex.InnerException;

                TestOutcome outcome;
                if (ex is TestException)
                    outcome = ((TestException)ex).Outcome;
                else
                    outcome = TestOutcome.Failed;

                switch (outcome.Status)
                {
                    case TestStatus.Inconclusive:
                        Log.Warnings.WriteException(ex, description);
                        break;

                    case TestStatus.Failed:
                        Log.Failures.WriteException(ex, description);
                        break;
                }

                return outcome;
            }
        }
    }
}