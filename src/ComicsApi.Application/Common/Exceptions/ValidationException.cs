using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace ComicsApi.Application.Common.Exceptions
{
    /// <summary>
    /// Exception được ném khi có lỗi validation
    /// </summary>
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}