using System.ComponentModel.DataAnnotations;

using MIB.Core.Domain.Events;

namespace MIB.Core.Domain.Commands
{
    public abstract class Command : Message
    {
        public DateTime TimeStamp { get; protected set; }

        public ValidationResult? ValidationResult { get; set; }

        protected Command()
        {
            TimeStamp = DateTime.Now;
        }
    }
}