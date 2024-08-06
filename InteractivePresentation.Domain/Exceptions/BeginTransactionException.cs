using System;
using System.Runtime.Serialization;

namespace InteractivePresentation.Domain.Exceptions
{
    [Serializable]
    public sealed class BeginTransactionException :
       Exception
    {
        public BeginTransactionException()
        {
        }

        public BeginTransactionException(string message)
            : base(message)
        {
        }

        public BeginTransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private BeginTransactionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) =>
            base.GetObjectData(info, context);
    }
}
