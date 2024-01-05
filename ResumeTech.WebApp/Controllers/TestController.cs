using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTech.Application.Util;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Actions;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Application.Controllers;

public sealed record InnerObj {
    [MinLength(2)]
    public string Name { get; init; }

    public InnerObj([MinLength(2)] string name) {
        this.Name = name;
    }
    
    public void Deconstruct(out string Name) {
        Name = this.Name;
    }
}

public sealed record Currency {
    public Currency(decimal Amount, string Code) {
        this.Amount = Amount;
        this.Code = Code;
        if (Amount < 0) {
            throw new ArgumentException("Amount cannot be negative");
        }
    }
    
    public decimal Amount { get; init; }
    public string Code { get; init; }
    
    public void Deconstruct(out decimal Amount, out string Code) {
        Amount = this.Amount;
        Code = this.Code;
    }
}

public sealed record RequestObj(
    Currency Buy,
    Currency Sell
) {
}

[ApiController]
[Route("test")]
public class TestController : ControllerBase {

    /// <summary>
    /// Create a new Job
    /// </summary>
    [Route("")]
    [HttpPost]
    public bool ValidationTest([FromBody] RequestObj request) {
        Console.WriteLine(request.Buy);
        Console.WriteLine(request.Sell);
        return true;
    }
}