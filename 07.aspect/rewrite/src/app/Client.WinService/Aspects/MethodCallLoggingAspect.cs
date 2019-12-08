using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
using PostSharp.Serialization;
using Serilog;

namespace Client.ScanService.Aspects
{
    [PSerializable]
    public class MethodCallLoggingAspect : TypeLevelAspect
    {
        [OnMethodEntryAdvice]
        [MulticastPointcut(Targets = MulticastTargets.Method)]
        public void OnEntry(MethodExecutionArgs args)
        {
            Log.Verbose($"[{DateTime.UtcNow}] - Call method '{args.Method.Name}' of type '{args.Method.DeclaringType?.FullName}'");

            if (args.Arguments.Any())
            {
                LogArguments(args.Method.GetParameters(), args.Arguments);
            }
        }

        [OnMethodExitAdvice]
        [MulticastPointcut(Targets = MulticastTargets.Method)]
        public void OnExit(MethodExecutionArgs args)
        {
            Log.Verbose($"[{DateTime.UtcNow}] - Method '{args.Method.Name}' returns '{args.ReturnValue}'");
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
