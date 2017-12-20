using System;
using System.Threading.Tasks;
using Passenger.Core.Domain;

namespace Passenger.Infrastructure.Services
{
    public interface IHandlerTask
    {
        IHandlerTask Always(Func<Task> always);
        IHandlerTask OnCustomError(Func<PassengerException,Task> onCustomError,
            bool propagateException =false, bool executeOnError = false);
        IHandlerTask OnError(Func<Exception,Task> onError,
            bool propagateException =false, bool executeOnError = false);
        IHandlerTask OnSuccess(Func<Task> onSuccess);
        IHandlerTask PropagateException();
        IHandlerTask DoNotPropagateException();
        IHandler Next();
        Task ExecuteAsync();
    }
}