namespace CustomFramework.Mvc.Contracts
{
    using System;

    public interface IServiceCollection
    {
        void Add<TSource, TDestination>()
            where TDestination : TSource;

        object CreateInstance(Type type);
    }
}
