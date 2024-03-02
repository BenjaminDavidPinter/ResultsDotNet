using System;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BensWorkbench.Models;
using TraceReloggerLib;

namespace MyBenchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ExceptionBenches>();
        }
    }

    [MemoryDiagnoser]
    public class ExceptionBenches
    {
        [Benchmark]
        public void ThrowException()
        {
            string s = null!;
            try
            {
                StringTestException(s);
            }
            catch
            {
                Console.Write("");
            }
        }

        [Benchmark]
        public void ReturnException()
        {
            string s = null!;
            var results = StringTestResult(s) switch
            {
                var errResult when errResult.IsErr<Exception>() => true
            };

            if (results)
            {
                Console.Write("");
            }
        }

        public Result<bool> StringTestResult(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new Exception("False");
            }

            return true;
        }

        public bool StringTestException(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new Exception("False");
            }

            return true;
        }
    }
}