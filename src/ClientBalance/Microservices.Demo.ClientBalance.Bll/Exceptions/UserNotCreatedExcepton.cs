using System;

namespace Microservices.Demo.ClientBalance.Bll.Exceptions;

public class UserNotCreatedExcepton(long id, string msg = "")
    : Exception($"User wit id {id} was not created {msg}");