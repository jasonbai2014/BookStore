using BookStore.Web.Abstract;
using BookStore.Web.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Infrastructure
{
    /// <summary>
    /// This is a class used for dependency injection 
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// This is the kernel
        /// </summary>
        IKernel kernel;

        /// <summary>
        /// This is a constructor of the class
        /// </summary>
        /// <param name="kernel">This is the kernel</param>
        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        /// <summary>
        /// Gets an object of the required type
        /// </summary>
        /// <param name="serviceType">This is the required type</param>
        /// <returns>An object of the required type</returns>
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        /// <summary>
        /// Gets a list of objects of the required type
        /// </summary>
        /// <param name="serviceType">This is the required type</param>
        /// <returns>A list of objects of the required type</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        /// <summary>
        /// Adds bindings 
        /// </summary>
        private void AddBindings()
        {
            kernel.Bind<IBookRepository>().To<BookRepository>();
        }
    }
}