namespace Microsoft.eShopOnContainers.Services.Buying.API.Infrastructure.Exceptions
{
    using System;

    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class BuyerDomainException : Exception
    {
        public BuyerDomainException()
        { }

        public BuyerDomainException(string message)
            : base(message)
        { }

        public BuyerDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

