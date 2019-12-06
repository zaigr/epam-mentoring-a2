using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Serilog;

namespace Client.ScanService.AOP.Advices
{
    public class MethodCallLoggingAdvice : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Log.Verbose($"[{DateTime.UtcNow}] - Call method '{invocation.Method.Name}' of type '{invocation.TargetType.FullName}'");

            if (invocation.Arguments.Any())
            {
                LogArguments(invocation.Method.GetParameters(), invocation.Arguments);
            }

            invocation.Proceed();

            Log.Verbose($"[{DateTime.UtcNow}] - Method '{invocation.Method.Name}' returns '{invocation.ReturnValue}'");
        }

        private void LogArguments(IEnumerable<ParameterInfo> parameters, IEnumerable<object> values)
        {
            foreach (var paramValue in parameters
                .Zip(values, (p, v) => (parameter: p, value: v)))
            {
                Log.Verbose($"{paramValue.parameter.Name}" + " - {@value}", paramValue.value);
            }
        }
    }
}
