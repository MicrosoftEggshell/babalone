namespace EVAL.Babalone.Persistence
{
    /// <summary>
    /// The exception that is thrown when saving or loading a Babalone
    /// save file fails.
    /// </summary>
    public class BabaloneDataException : IOException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneDataException"/> class.
        /// </summary>
        public BabaloneDataException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneDataException"/>
        /// class with its message set to <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BabaloneDataException(string message) : base(message) { }
    }
}
