using InTimeProAPI.DTOs;

namespace InTimeProAPI.Tests.Contracts;

public class AuthContractTests
{
    [Fact]
    public void LoginRequest_DefaultProvider_IsEmail()
    {
        var request = new LoginRequest
        {
            Email = "employee@company.com"
        };

        Assert.Equal("email", request.Provider);
    }

    [Fact]
    public void ApiResponse_Ok_SetsSuccessAndData()
    {
        var response = ApiResponse<string>.Ok("token", "ok");

        Assert.True(response.Success);
        Assert.Equal("token", response.Data);
        Assert.Equal("ok", response.Message);
        Assert.Null(response.Errors);
    }

    [Fact]
    public void ApiResponse_Fail_SetsFailureAndErrors()
    {
        var response = ApiResponse<object>.Fail("Validation failed", new List<string> { "email required" });

        Assert.False(response.Success);
        Assert.Equal("Validation failed", response.Message);
        Assert.Null(response.Data);
        Assert.Contains("email required", response.Errors!);
    }
}