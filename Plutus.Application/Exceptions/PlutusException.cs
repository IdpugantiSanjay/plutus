using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plutus.Application.Exceptions;

public abstract record AbstractPlutusException
{
    private readonly PlutusException _plutusException;

    public PlutusException PlutusException => _plutusException;

    public AbstractPlutusException(PlutusException exception)
    {
        _plutusException = exception;
    }
}


public record TransactionNotFoundException : AbstractPlutusException
{
    public TransactionNotFoundException() : base(PlutusException.TransactionNotFound)
    {
    }
}

public enum PlutusException
{
    TransactionNotFound
}