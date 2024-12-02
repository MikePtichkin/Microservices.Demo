using System;

namespace Microservices.Demo.OrdersFacade.Application.Exceptions;

public class CustomerNotFoundException(long Id)
    : Exception($"Customer with id {Id} not found");
