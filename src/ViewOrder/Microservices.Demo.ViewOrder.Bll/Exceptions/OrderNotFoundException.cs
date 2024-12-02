using System;

namespace Microservices.Demo.ViewOrder.Bll.Exceptions;

public class OrderNotFoundException(long id, string msg = "")
    : Exception($"Order with id {id} not found. {msg}");