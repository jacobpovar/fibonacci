using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

using StructureMap;

namespace Fibonacci.Initiator.DependencyResolution
{
    /// <summary>
    /// The structure map web api dependency scope.
    /// </summary>
    public class StructureMapWebApiDependencyScope : IDependencyScope
    {
        private IContainer _container;

        public StructureMapWebApiDependencyScope(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface) return _container.TryGetInstance(serviceType);

                return _container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances<object>().Where(x => x.GetType() == serviceType);
        }

        public void Dispose()
        {
            _container.Dispose();
            _container = null;
        }
    }
}