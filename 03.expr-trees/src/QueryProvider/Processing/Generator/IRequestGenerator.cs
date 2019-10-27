using System;

namespace QueryProvider.Processing.Generator
{
    public interface IRequestGenerator
    {
        Uri GenerateRequestUrl<T>(string query = "*", int start = 0, int limit = 10);

        Uri GenerateRequestUrl(Type type, string query = "*", int start = 0, int limit = 10);
    }
}
