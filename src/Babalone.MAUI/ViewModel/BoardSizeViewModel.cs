using EVAL.Babalone.Persistence;

namespace EVAL.Babalone.ViewModel;

public class BoardSizeViewModel : ViewModelBase
{
    public int Size { get; private set; }

    public string Name => BabaloneBoardSize.ToString(Size);

    public BoardSizeViewModel(int boardSize)
    {
        Size = boardSize;
    }
}
