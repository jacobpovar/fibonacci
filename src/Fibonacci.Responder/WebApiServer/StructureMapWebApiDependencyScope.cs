namespace Fibonacci.Responder.WebApiServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;

    using StructureMap;

    /// <summary>
    /// The structure map web api dependency scope.
    /// </summary>
    public class StructureMapWebApiDependencyScope : IDependencyScope
    {
        private IContainer _container;

        public StructureMapWebApiDependencyScope(IContainer container)
        {
            this._container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface) return this._container.TryGetInstance(serviceType);

                return this._container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.GetAllInstances<object>().Where(x => x.GetType() == serviceType);
        }

        public void Dispose()
        {
            this._container.Dispose();
            this._container = null;
        }
    }
}