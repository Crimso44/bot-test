using System;
using Microsoft.Extensions.Logging;

namespace ChatBot.Admin.ReadStorage.Queries
{
    internal abstract class QueryBase
    {
        private readonly ILogger _logger;

        protected QueryBase(ILogger logger)
        {
            _logger = logger;
        }

        protected void CheckSpecificationIsNotNullOrThrow<TSpecification>(TSpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
        }
    }
}
