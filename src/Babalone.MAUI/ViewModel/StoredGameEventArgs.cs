namespace EVAL.Babalone.ViewModel
{
    public class StoredGameEventArgs : EventArgs
    {
        public string Path { get; private set; }

        public StoredGameEventArgs(string path)
        {
            Path = path;
        }
    }
}
