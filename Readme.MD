### Run bechmarks

```
cd HopacTaskAsyncBenchmark
dotnet run -c Release
```

### Results on my machine
``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.285 (1803/April2018Update/Redstone4)
Intel Core i5-6400 CPU 2.70GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT DEBUG
  Job-ZTCCQZ : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT

Server=True  IterationCount=5  LaunchCount=3  
WarmupCount=3  

```
|                    Method |      Mean |     Error |    StdDev | Scaled | ScaledSD |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|-------------------------- |----------:|----------:|----------:|-------:|---------:|---------:|---------:|---------:|----------:|
| reduceParallelAsyncsBench | 38.332 ms | 2.5424 ms | 2.3782 ms |   3.51 |     0.25 | 384.6154 | 153.8462 |  76.9231 |  826232 B |
|  reduceParallelTasksBench | 10.925 ms | 0.4466 ms | 0.4178 ms |   1.00 |     0.00 | 531.2500 | 203.1250 |  62.5000 |     468 B |
|   reduceParallelHopacBech |  3.579 ms | 0.3155 ms | 0.2951 ms |   0.33 |     0.03 | 507.8125 | 195.3125 | 125.0000 | 1625472 B |

