using Castle.DynamicProxy;
using Client.ScanService.AOP.Advices;

namespace Client.ScanService.AOP.Aspects
{
    public static class MethodCallLoggingAspect
    {
        public static T Apply<T>()
            where T : class
        {
            var generator = new ProxyGenerator();
            var proxy = generator.CreateClassProxy<T>(
                new MethodCallLoggingAdvice());

            return proxy;
        }
    }
}
