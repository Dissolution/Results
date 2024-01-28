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
        if (result)
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

    [Theory]
    [MemberData(nameof(MemberData))]
    [Obsolete("ignore", false)]
    public void BitwiseNegate(Result result)
    {
        Assert.Throws<NotSupportedException>(() => ~result);
    }
    
    #endregion
    #region binary
    
    [Theory]
    [MemberData(nameof(MemberDataPair))]
    public void EqualResultOp(Result first, Result second)
    {
        var eqOp = first == second;
        var eqMeth = first.Equals(second);
        Assert.Equal(eqOp, eqMeth);
        if (eqOp)
        {
            Assert.Equal(first.IsOk(), second.IsOk());
        }
        else
        {
            Assert.NotEqual(first.IsOk(), second.IsOk());
        }
    }
    
    [Theory]
    [MemberData(nameof(MemberData))]
    public void EqualBoolOp(Result result)
    {
        var opEqualResultBool = result == true;
        var opEqualBoolResult = true == result;
        Assert.Equal(opEqualResultBool, opEqualBoolResult);
        Assert.Equal(opEqualResultBool, result.IsOk());
    }
    
    [Theory]
    [MemberData(nameof(MemberDataPair))]
    public void NotEqualOp(Result first, Result second)
    {
        var eqOp = first != second;
        var eqMeth = !first.Equals(second);
        Assert.Equal(eqOp, eqMeth);
        if (eqOp)
        {
            Assert.NotEqual(first.IsOk(), second.IsOk());
        }
        else
        {
            Assert.Equal(first.IsOk(), second.IsOk());
        }
    }
    
    [Theory]
    [MemberData(nameof(MemberData))]
    public void NotEqualBoolOp(Result result)
    {
        var opEqualResultBool = result != true;
        var opEqualBoolResult = true != result;
        Assert.Equal(opEqualResultBool, opEqualBoolResult);
        Assert.Equal(opEqualResultBool, result.IsError());
    }

    [Theory]
    [MemberData(nameof(MemberDataPair))]
    public void ResultBitwiseOr(Result left, Result right)
    {
        var resultOr = left | right;
        Assert.IsType<Result>(resultOr);
        bool boolOr = ((bool)left) | ((bool)right);
        Assert.Equal(boolOr, resultOr.IsOk());

        var resultShortOr = left || right;
        Assert.IsType<Result>(resultShortOr);
        bool boolShortOr = ((bool)left) || ((bool)right);
        Assert.Equal(boolShortOr, resultShortOr.IsOk());
    }
    
    [Theory]
    [MemberData(nameof(MemberData))]
    public void BoolBitwiseOr(Result result)
    {
        var resultOr = result | true;
        Assert.IsType<Result>(resultOr);
        bool boolOr = ((bool)result) | ((bool)true);
        Assert.Equal(boolOr, resultOr.IsOk());

        var resultShortOr = true || result;
        Assert.IsType<Result>(resultShortOr);
        bool boolShortOr = ((bool)left) || ((bool)right);
        Assert.Equal(boolShortOr, resultShortOr.IsOk());
    }

    [Theory]
    [MemberData(nameof(MemberDataPair))]
    public void BitwiseAnd(Result left, Result right)
    {
        var resultAnd = left & right;
        Assert.IsType<Result>(resultAnd);
        bool boolAnd = ((bool)left) & ((bool)right);
        Assert.Equal(boolAnd, resultAnd.IsOk());

        var resultShortAnd = left && right;
        Assert.IsType<Result>(resultShortAnd);
        bool boolShortAnd = ((bool)left) && ((bool)right);
        Assert.Equal(boolShortAnd, resultShortAnd.IsOk());
    }

    [Theory]
    [MemberData(nameof(MemberDataPair))]
    public void BitwiseXor(Result left, Result right)
    {
        var resultXor = left ^ right;
        Assert.IsType<Result>(resultXor);
        bool boolXor = ((bool)left) ^ ((bool)right);
        Assert.Equal(boolXor, resultXor.IsOk());
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