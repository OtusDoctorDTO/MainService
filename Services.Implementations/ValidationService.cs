using Microsoft.Extensions.Logging;
using Services.Abstractions;
using Services.Contracts;

namespace Services.Implementations
{
    public class ValidationService : IValidationService
    {
        private readonly ILogger<ValidationService> _logger;
        public ValidationService(ILogger<ValidationService> logger)
        {
            _logger = logger;
        }

        public async Task<ValidationResult> CheckPassportDataAsync()
        {
            return new ValidationResult() { IsValid = true };
        }
    }
}
