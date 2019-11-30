using System;

namespace Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = ReplicatedAlgorithm.GenerateKey();

            var split = key.Split('-');
            if (SourceAlgorithm.Original(split) &&
                ReplicatedAlgorithm.Check(split))
            {
                Console.WriteLine("Key generated.");
                Console.WriteLine(key);
            }
            else
            {
                Console.WriteLine($"Generated '{key}' didn't pass all the checks. Review generation algorithm.");
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
