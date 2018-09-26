``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.285 (1803/April2018Update/Redstone4)
Intel Core i7-8550U CPU 1.80GHz (Max: 1.79GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT DEBUG
  Job-UFYJJD : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT

Server=True  IterationCount=5  LaunchCount=3  
WarmupCount=3  

```
|                          Method |        Mean |     Error |     StdDev | Scaled | ScaledSD |    Gen 0 |   Gen 1 | Allocated |
|-------------------------------- |------------:|----------:|-----------:|-------:|---------:|---------:|--------:|----------:|
|      reduceParallelFSharpAsyncs | 29,672.9 us | 919.34 us | 859.952 us |  12.87 |     0.39 | 218.7500 | 93.7500 |   27744 B |
|      reduceParallelCSharpAsyncs |  2,305.9 us |  29.40 us |  27.503 us |   1.00 |     0.00 |  50.7813 |       - | 4718608 B |
|               reduceParallelTPL |    836.8 us |  11.35 us |  10.616 us |   0.36 |     0.01 |  49.8047 |       - | 4718608 B |
|        reduceParallelValueTasks |    874.2 us |  10.65 us |   9.960 us |   0.38 |     0.01 |        - |       - |      88 B |
| reduceParallelValueTasksPattern |  2,307.7 us |  54.55 us |  51.024 us |   1.00 |     0.02 |        - |       - |      88 B |
|             reduceParallelHopac |  1,988.4 us |  30.23 us |  28.276 us |   0.86 |     0.02 | 126.9531 |  3.9063 |   27784 B |
