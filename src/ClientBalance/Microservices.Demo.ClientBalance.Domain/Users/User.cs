using Microservices.Demo.ClientBalance.Domain.Abstractions;

namespace Microservices.Demo.ClientBalance.Domain.Users;

public sealed class User : Entity<long>
{
    public User(
        long id,
        decimal balance = 0)
        : base(id)
    {
        Balance = balance;
    }

    public decimal Balance { get; private set; }

    public bool IsWithdrawAvailable(decimal amount) =>
        amount <= Balance;

    public void TopUp(decimal amount) =>
        Balance += amount;

    public void Withdraw(decimal amount) =>
        Balance -= amount;

    public void Refund(decimal amount) =>
        Balance += amount;
}
