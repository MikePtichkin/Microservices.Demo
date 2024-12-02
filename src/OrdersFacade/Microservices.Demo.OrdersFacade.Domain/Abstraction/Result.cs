using System;

namespace Microservices.Demo.OrdersFacade.Domain.Abstraction;

public class Result<TValue>
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        _value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; init; }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static Result<TValue> Success(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure(Error error) => new(default, false, error);

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
    private static Result<TValue> Create(TValue? value) => value is not null
        ? Success(value)
        : Failure(Error.NullValue);
}
