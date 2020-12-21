using System;

namespace CloakedUI.Exceptions
{
    [Serializable()]
    public class OrphanedGuiComponentException : System.Exception
    {
        public OrphanedGuiComponentException() : base() { }
        public OrphanedGuiComponentException(string message) : base(message) { }
        public OrphanedGuiComponentException(string message, System.Exception inner) : base(message, inner) { }
        protected OrphanedGuiComponentException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}