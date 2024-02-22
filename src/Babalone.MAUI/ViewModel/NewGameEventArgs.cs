namespace EVAL.Babalone.ViewModel
{
    public class NewGameEventArgs
    {
        public int BoardSize { get; private set; }

        public NewGameEventArgs(int boardSize)
        {
            BoardSize = boardSize;
        }
    }
}
