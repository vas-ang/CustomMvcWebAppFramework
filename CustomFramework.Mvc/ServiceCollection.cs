namespace CustomFramework.Mvc
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    using Contracts;

    public class ServiceCollection : IServiceCollection
    {
        private IDictionary<Type, Type> serviceMap = new Dictionary<Type, Type>();

        public void Add<TSource, TDestination>()
            where TDestination : TSource
        {
            serviceMap.Add(typeof(TSource), typeof(TDestination));
        }

        public object CreateInstance(Type type)
        {
            if (this.serviceMap.ContainsKey(type))
            {
                type = this.serviceMap[type];
            }

            ConstructorInfo typeConstructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(c => c.GetParameters().Count())
                .FirstOrDefault();

            ParameterInfo[] paramteterInfos = typeConstructor.GetParameters();

            List<object> parameters = new List<object>();

            foreach (var parameter in paramteterInfos)
            {
                parameters.Add(this.CreateInstance(parameter.ParameterType));
            }

            return typeConstructor.Invoke(parameters.ToArray());
        }
    }
}
