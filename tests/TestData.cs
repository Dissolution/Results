namespace ScrubJay.Results.Tests;

public static class TestData
{
    public static Exception Exception { get; } = new Exception();
    public static Exception[] Exceptions { get; } = new Exception[2] { Exception, Exception };

    public static Result[] Results { get; } = new Result[]
    {
        default(Result),
        (Result)true,
        (Result)false,
        (Result)Exception,
        Result.Ok(),
        Result.Error(Exception),
        Result.Errors(Exceptions),
    };

    public static IEnumerable<object[]> AsMemberData<T>(IEnumerable<T> source)
        where T : notnull
    {
        foreach (var value in source)
        {
            yield return new object[1] { (object)value };
        }
    }

    public static IEnumerable<object[]> AsMemberDataPair<T>(IReadOnlyList<T> source)
        where T : notnull
    {
        foreach (var first in source)
        foreach (var second in source)
        {
            yield return new object[2] { (object)first, (object)second };
        }
    }
}