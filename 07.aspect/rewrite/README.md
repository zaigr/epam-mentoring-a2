# AOP using Code Rewriting

In this project [client service](https://github.com/zaigr/epam-mentoring-a2/tree/master/05.msg-queues/client) of messaging task extended.

Method logging aspect `MethodCallLoggingAspect.cs` applied on assembly by using [PostSharp Ultimate](https://www.postsharp.net/Purchase/Subscription.aspx) (600$ !!!) visual studio extension.

## Aspect

Type level aspect defined in `MethodCallLoggingAspect.cs` as an attribute

```
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
```

Parameters logging implemented by [Serilog structured data](https://github.com/serilog/serilog/wiki/Structured-Data#preserving-object-structure) formatting.

## Slice 

Place where aspect should be applied described in `AssemblyInfo.cs` by using assembly level attribute
```
[assembly: MethodCallLoggingAspect(AttributeTargetTypes = "regex:.*")]
```

As a result, all methods (compiler generated too) will have this logging aspect applied.

What's the magic!

## Logs

Logs are stream to file and console with help of `Serilog` sinks. Method call logged in the following order:
- name of type and method
- parameters name and value
- serialized return value

Values that cannot be serialized logged as type name

```
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Call method 'get_ConfigTopicSubscriptionName' of type 'Client.ScanService.Configuration.ServiceConfig'
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Method 'get_ConfigTopicSubscriptionName' returns 'client-service-configuration'
[10:32:52 VRB] serviceConfig - {"MessageMaxSizeBytes": 180, "LogFilePath": "ScanServiceLog.log", "MonitoringFoldersConfigFile": "ScanFolders.txt", "FolderScanFrequencySeconds": 30, "AvailabilityCheckFrequencySeconds": 10, "ClientId": "fe967f71-6164-453b-8eda-91cd5b1c6260", "DataQueueConnectionString": "Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=LziGOa7UMlgc8yqvATrIgfWQd7RnxUJ9Hb6zViMA33Q=", "DataQueueName": "data-queue", "MonitoringQueueConnectionString": "Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=npqMtk/k46o/py498ozKbJwU16aniZ0NRBfNfjhia1c=", "MonitoringQueueName": "monitoring-queue", "ConfigTopicConnectionString": "Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=6U0k6rYLz6GqwxKFTFDmNPn4E8pgFSQIx1ljTTJ8oAI=", "ConfigTopicName": "configuration-topic", "ConfigTopicSubscriptionName": "client-service-configuration", "$type": "ServiceConfig"}
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Call method 'get_MonitoringFoldersConfigFile' of type 'Client.ScanService.Configuration.ServiceConfig'
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Method 'get_MonitoringFoldersConfigFile' returns 'ScanFolders.txt'
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Method 'GetScanFoldersConfig' returns 'System.String[]'
[10:32:52 DBG] Start timer with interval '30' seconds.
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Call method 'get_MonitoringQueueConnectionString' of type 'Client.ScanService.Configuration.ServiceConfig'
[10:32:52 DBG] Start folders scanning.
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Method 'get_MonitoringQueueConnectionString' returns 'Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=npqMtk/k46o/py498ozKbJwU16aniZ0NRBfNfjhia1c='
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Call method 'get_MonitoringQueueName' of type 'Client.ScanService.Configuration.ServiceConfig'
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Method 'get_MonitoringQueueName' returns 'monitoring-queue'
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Call method 'GetDbContextOptions' of type 'Client.ScanService.ServiceLocator'
[10:32:52 DBG] Scan 'C:\Users\zahar\Desktop\Projects\epam-mentoring-a2\05.msg-queues\client\src\app\Client.WinService\bin\Debug\a' folder.
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Call method 'ConfigureDbContext' of type 'Client.ScanService.ServiceLocator'
[10:32:52 VRB] builder - {"Options": {"ContextType": "Microsoft.EntityFrameworkCore.DbContext", "Extensions": [], "IsFrozen": false, "$type": "DbContextOptions`1"}, "IsConfigured": false, "$type": "DbContextOptionsBuilder"}
[10:32:52 VRB] [12/6/2019 7:32:52 AM] - Method 'ConfigureDbContext' returns ''
```