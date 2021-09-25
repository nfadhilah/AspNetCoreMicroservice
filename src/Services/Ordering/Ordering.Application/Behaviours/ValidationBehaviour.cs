﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
      private readonly IEnumerable<IValidator<TRequest>> _validators;

      public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
      {
        _validators = validators;
      }

      public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
      {
        if (_validators.Any())
        {
          var context = new ValidationContext<TRequest>(request);
          var validationResults =
            await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));
          var failures = validationResults.SelectMany(x => x.Errors).Where(x => x != null).ToList();
          if (failures.Any()) throw new ValidationException(failures);
        }

        return await next();
      }
    }
}
