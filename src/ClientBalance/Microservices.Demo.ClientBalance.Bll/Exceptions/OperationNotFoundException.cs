using System;

namespace Microservices.Demo.ClientBalance.Bll.Exceptions;

public class OperationNotFoundException(Guid Id)
    : Exception($"Operation with id {Id} not found");