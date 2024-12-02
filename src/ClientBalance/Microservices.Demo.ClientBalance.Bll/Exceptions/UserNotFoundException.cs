using System;

namespace Microservices.Demo.ClientBalance.Bll.Exceptions;

public class UserNotFoundException(long id, string msg = "")
    : Exception($"User with id {id} not found. {msg}");