using System;

namespace Macaria.Infrastructure.Exceptions
{
    public class DomainException: Exception
    {
        public int Code { get; set; } = 0;
    }
}
