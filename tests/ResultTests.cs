namespace ScrubJay.Results.Tests;

/// <summary>
/// Tests for <see cref="Result"/>
/// </summary>
public class ResultTests
{
    public static IEnumerable<object[]> MemberData => TestData.AsMemberData<Result>(TestData.Results);
    public static IEnumerable<object[]> MemberDataPair => TestData.AsMemberDataPair<Result>(TestData.Results);
    
#region operators

#region implicit/explicit

    [Fact]
    public void ImplicitCastFromBool()
    {
        Result trueResult = true;
        Assert.True(trueResult.IsOk());

        Result falseResult = false;
        Assert.True(falseResult.IsError());
    }

    [Fact]
    public void ExplicitCastFromBool()
    {
        Result trueResult = (Result)true;
        Assert.True(trueResult.IsOk());

        Result falseResult = (Result)false;
        Assert.True(falseResult.IsError());
    }

    [Fact]
    public void ImplicitCastFromException()
    {
        InvalidOperationException exception = new InvalidOperationException("BAD");
        Result exResult = exception;
        Assert.True(exResult.IsError(out var ex));
        Assert.NotNull(ex);
        Assert.True(object.ReferenceEquals(ex, exception));
    }

    [Fact]
    public void ExplicitCastFromException()
    {
        InvalidOperationException exception = new InvalidOperationException("BAD");
        Result exResult = (Result)exception;
        Assert.True(exResult.IsError(out var ex));
        Assert.NotNull(ex);
        Assert.True(object.ReferenceEquals(ex, exception));
    }

    [Theory]
    [MemberData(nameof(MemberData))]
    public void ImplicitCastToBool(Result result)
    {
        bool b = result;
        Assert.Equal(b, result.IsOk());
        Assert.NotEqual(b, result.IsError());
    }

    [Theory]
    [MemberData(nameof(MemberData))]
    public void ExplicitCastToBool(Result result)
    {
        bool b = (bool)result;
        Assert.Equal(b, result.IsOk());
        Assert.NotEqual(b, result.IsError());
    }

#endregion
    
#region unary

    [Theory]
    [MemberData(nameof(MemberData))]
    public void TrueOperator(Result result)
    {
        if (result)
        {
            Assert.True(result.IsOk());
        }
        else
        {
            Assert.False(result.IsOk());
        }
    }
    
    [Theory]
    [MemberData(nameof(MemberData))]
    public void FalseOperator(Result result)
    {
        if (!result)
        {
            Assert.False(result.IsOk());
        }
        else
        {
            Assert.True(result.IsOk());
        }
    }
    
    
    [Theory]
    [MemberData(nameof(MemberData))]
    public void LogicalNot(Result result)
    {
        var not = !result;
        Assert.IsType<bool>(not);
        Assert.Equal(not, result.IsError());
        Assert.NotEqual(not, result.IsOk());
    }
    
    #endregion
#endregion

    [Fact]
    public void OkWorks()
    {
        Result result = Result.Ok();
        Assert.True(result);
        Assert.True(result.IsOk());
        Assert.False(result.IsError());
    }
    
    [Fact]
    public void ErrorWorks()
    {
        Result result = Result.Error(null!);
        Assert.False(result);
        Assert.False(result.IsOk());
        Assert.True(result.IsError());
    }


    [Fact]
    [Obsolete("", false)]
    public void CanDealWithDefault()
    {
        Result alpha = default(Result);
        Assert.True(alpha.IsError());

        Result beta = new Result();
        Assert.True(beta.IsError());
    }


    [Theory]
    [MemberData(nameof(MemberData))]
    public void CannotBeOkAndError(Result result)
    {
        bool ok = result.IsOk();
        bool error = result.IsError();
        Assert.NotEqual(ok, error);
    }

    [Theory]
    [MemberData(nameof(MemberData))]
    public void ErrorAlwaysHasException(Result result)
    {
        if (result.IsError(out var error))
        {
            Assert.NotNull(error);
        }
    }

    [Theory]
    [MemberData(nameof(MemberData))]
    public void OkCycle(Result result)
    {
        // true operator
        if (result)
        {
            // implicit bool cast
            Assert.True((bool)result);
            // operators
            Assert.True(result == true);
            Assert.True(true == result);
            Assert.True(result != false);
            Assert.True(false != result);
            Assert.True(result | true);
            Assert.True(true | result);
            Assert.True(true & result);
            Assert.True(result & true);
            // methods
            Assert.True(result.IsOk());
            Assert.False(result.IsError());
            Assert.False(result.IsError(out _));
        }
        else
        {
            // implicit bool cast
            Assert.False((bool)result);
            // operators
            Assert.False(result == true);
            Assert.False(true == result);
            Assert.False(result != false);
            Assert.False(false != result);
            Assert.False(result | false);
            Assert.False(false | result);
            Assert.False(true & result);
            Assert.False(result & true);
            // methods
            Assert.False(result.IsOk());
            Assert.True(result.IsError());
            Assert.True(result.IsError(out _));
        }
    }
}