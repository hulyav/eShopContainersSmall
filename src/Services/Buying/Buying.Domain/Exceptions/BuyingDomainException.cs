using System;
using System.Collections.Generic;
using System.Text;

namespace Buying.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class BuyingDomainException : Exception
    {
        public  BuyingDomainException()
        { }

        public BuyingDomainException(string message)
            : base(message)
        { }

        public BuyingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}