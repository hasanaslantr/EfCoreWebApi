namespace Entities.Exceptions
{
    public sealed class BookNotFoundExceptions : NotFoundExceptions
    {
        public BookNotFoundExceptions(int id) : base($"The book with id : {id} could not found.")
        {
        }
    }
}
