using System;

namespace Microservices.Demo.ClientOrders.Bll.Exceptions;

public class CustomerNotFoundException(long id, string msg = "")
    : Exception($"Customer with id {id} not found. {msg}");