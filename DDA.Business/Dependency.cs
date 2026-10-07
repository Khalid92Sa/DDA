using Castle.MicroKernel.Registration;
using DDA.Business.Interfaces;
using DDA.Business.Repositories;
using DDA.Business.Services;
using DDA.DAL;
using SSSFramework;
using SSSFramework.Repository;
using System;

namespace DDA.Business
{
    public class Dependency
    {
        public static void Register()
        {
            IoC.Register(Component
             .For(typeof(IUnitOfWork<DDAEntities>))
             .ImplementedBy(typeof(UnitOfWork<DDAEntities>))
             .LifestyleTransient());

            IoC.Register(Component
             .For(typeof(IGenericRepository))
             .ImplementedBy(typeof(GenericRepository<DDAEntities>))
             .LifestyleTransient());

            IoC.Register(Component.For<DDAEntities>().ImplementedBy<DDAEntities>().LifestyleTransient());

            IoC.Register(Component.For<IUserRepository>().ImplementedBy<UserRepository>().LifestyleTransient());
            IoC.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestyleTransient());
        }
    }
}
