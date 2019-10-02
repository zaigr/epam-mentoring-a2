``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.1.13 (CoreCLR 4.6.28008.01, CoreFX 4.6.28008.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.13 (CoreCLR 4.6.28008.01, CoreFX 4.6.28008.01), 64bit RyuJIT


```
|      Method | MatrixSize |           Mean |         Error |        StdDev |         Median | Rank |
|------------ |----------- |---------------:|--------------:|--------------:|---------------:|-----:|
|    **Parallel** |         **10** |      **69.663 us** |     **2.5339 us** |     **7.4714 us** |      **69.166 us** |    **2** |
| NonParallel |         10 |       8.530 us |     0.1980 us |     0.5713 us |       8.255 us |    1 |
|    **Parallel** |        **100** |  **21,576.893 us** |   **593.8732 us** | **1,741.7274 us** |  **21,269.694 us** |    **4** |
| NonParallel |        100 |   8,725.148 us |   443.8816 us | 1,287.7809 us |   8,394.763 us |    3 |
|    **Parallel** |        **250** | **255,470.677 us** | **5,009.2904 us** | **6,687.2588 us** | **253,531.000 us** |    **6** |
| NonParallel |        250 | 125,037.403 us | 1,125.2906 us | 1,052.5975 us | 125,058.250 us |    5 |
