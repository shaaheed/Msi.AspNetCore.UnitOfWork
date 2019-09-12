using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Msi.AspNetCore.UnitOfWork
{
    public static class DataContextExtensions
    {

        public static DbContextOptionsBuilder UseDefaultSqlServer(this DbContextOptionsBuilder options, string connectionString, string migrationAssembly)
        {
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
                sqlOptions.MigrationsAssembly(migrationAssembly);
            })
                    .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
            return options;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, Type dataContextType, Type dataContextOptionsType)
        {
            string connectionStringKey = $"{dataContextOptionsType.Name}:{nameof(IConnectionStringOptions.ConnectionString)}";
            var connectionString = services.GetConfiguration().GetValue<string>(connectionStringKey);
            return services.AddUnitOfWork(dataContextType, connectionString);
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, Type dataContextType, string connectionString)
        {

            var unitOfWorkInterfaceType = typeof(IUnitOfWork<>).MakeGenericType(dataContextType);

            // UnitOfWork
            services.AddTransient(unitOfWorkInterfaceType, serviceProvider =>
            {
                var scope = serviceProvider.CreateScope();
                var dataContext = scope.ServiceProvider.GetService(dataContextType);

                if (dataContext == null)
                {
                    throw new ArgumentNullException(nameof(dataContext));
                }

                var unitOfWorkType = typeof(UnitOfWork<>).MakeGenericType(dataContextType);
                return ActivatorUtilities.CreateInstance(serviceProvider, unitOfWorkType, dataContext);
            });

            // DbContextOptionsBuilder
            Action<DbContextOptionsBuilder> dbContextOptionsBuilder = options =>
            {

                if (connectionString == null)
                {
                    throw new ArgumentNullException(nameof(connectionString));
                }

                options.UseDefaultSqlServer(connectionString, dataContextType.Assembly.GetName().Name);

            };

            var addDbContextMethod = GetMethodAddDbContextMethod();

            if (addDbContextMethod == null)
            {
                throw new ArgumentNullException(nameof(addDbContextMethod));
            }

            var genericAddDbContextMethod = addDbContextMethod.MakeGenericMethod(dataContextType);
            genericAddDbContextMethod.Invoke(null, new object[] { services, dbContextOptionsBuilder, ServiceLifetime.Scoped, ServiceLifetime.Scoped });

            return services;
        }

        public static IServiceCollection AddUnitOfWork<TDataContext, TDataContextOptions>(this IServiceCollection services)
            where TDataContext : DbContext, IDataContext
            where TDataContextOptions : class, IConnectionStringOptions, new()
        {
            return services.AddUnitOfWork(typeof(TDataContext), typeof(TDataContextOptions));
        }

        public static IServiceCollection AddPersistence<TDataContext>(this IServiceCollection services, string connectionString)
            where TDataContext : DbContext, IDataContext
        {
            return services.AddUnitOfWork(typeof(TDataContext), connectionString);
        }

        // looking for bellow signature method
        // public static IServiceCollection AddDbContext<TContext>([NotNullAttribute] this IServiceCollection serviceCollection, [CanBeNullAttribute] Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext;
        private static MethodInfo GetMethodAddDbContextMethod()
        {
            return typeof(EntityFrameworkServiceCollectionExtensions)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(methodInfo =>
                {
                    var parameters = methodInfo.GetParameters();
                    if (methodInfo.GetGenericArguments().Count() == 1
                    && parameters.Count() == 4
                    && parameters[1].ParameterType == typeof(Action<DbContextOptionsBuilder>)
                    && parameters[2].ParameterType == typeof(ServiceLifetime)
                    && parameters[3].ParameterType == typeof(ServiceLifetime))
                    {
                        return true;
                    }
                    return false;
                });
        }

        private static IServiceProvider GetServiceProvider(this IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        private static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<IConfiguration>();
        }

    }
}
