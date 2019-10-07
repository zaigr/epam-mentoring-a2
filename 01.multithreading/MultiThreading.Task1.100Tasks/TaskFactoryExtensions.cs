using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    public static class TaskFactoryExtensions
    {
        public static Task[] StartNewTasks(this TaskFactory factory, Action action, int numberOfTasks)
        {
            var tasks = new Task[numberOfTasks];
            for (var taskIndex = 0; taskIndex < numberOfTasks; taskIndex++)
            {
                var taskIndexCopy = taskIndex;
                tasks[taskIndex] = factory.StartNew(action);
            }

            return tasks;
        }
    }
}
