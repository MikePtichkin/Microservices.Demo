using FluentAssertions;
using System;

namespace Microservices.Demo.ViewOrder.Infra.UnitTests.Fixtures;

public static class FluentAssertionOptions
{
    public static readonly TimeSpan RequiredDateTimePrecision = TimeSpan.FromMilliseconds(100);

    public static void UseDefaultPrecision()
    {
        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(
                    ctx => ctx.Subject.Should()
                        .BeCloseTo(ctx.Expectation, RequiredDateTimePrecision))
                .WhenTypeIs<DateTime>();

            options.Using<DateTime?>(
                    ctx =>
                    {
                        if (ctx.Expectation.HasValue)
                        {
                            ctx.Subject.Should().BeCloseTo(ctx.Expectation.Value, RequiredDateTimePrecision);
                        }
                        else
                        {
                            ctx.Subject.Should().BeNull();
                        }
                    })
                .WhenTypeIs<DateTime?>();

            options.Using<DateTimeOffset>(
                    ctx => ctx.Subject.Should()
                        .BeCloseTo(ctx.Expectation, RequiredDateTimePrecision))
                .WhenTypeIs<DateTimeOffset>();

            options.Using<DateTimeOffset?>(
                ctx =>
                {
                    if (ctx.Expectation.HasValue)
                    {
                        ctx.Subject.Should().BeCloseTo(ctx.Expectation.Value, RequiredDateTimePrecision);
                    }
                    else
                    {
                        ctx.Subject.Should().BeNull();
                    }
                })
                .WhenTypeIs<DateTimeOffset?>();

            return options;
        });
    }
}
