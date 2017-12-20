using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Passenger.Infrastructure.Services
{
    public class Handler : IHandler
    {
        private readonly ISet<IHandlerTask> _handlerTask = new HashSet<IHandlerTask>();
        public async Task ExecuteAllAsync()
        {
            foreach(var handlerTask in _handlerTask)
            {
                await handlerTask.ExecuteAsync();
            }
            _handlerTask.Clear();
        }

        public IHandlerTask Run(Func<Task> run)
        {
            var handlerTask = new HandlerTask(this,run);
            _handlerTask.Add(handlerTask);
            return handlerTask;
        }

        public IHandlerTaskRunner Validate(Func<Task> validate)
            => new HandlerTaskRunner(this, validate,_handlerTask);
    }
}