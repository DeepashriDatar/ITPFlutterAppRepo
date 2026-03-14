using InTimePro.Api.Services;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace InTimePro.Api
{
    public class SimpleResolver : IDependencyResolver
    {
        private readonly IAuthService _authService;

        public SimpleResolver(IAuthService authService)
        {
            _authService = authService;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IAuthService))
            {
                return _authService;
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
