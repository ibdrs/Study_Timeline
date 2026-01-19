namespace Study_Timeline.Logic.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string resource, int id)
            : base($"{resource} not found (Id: {id}).") { }
    }
}
