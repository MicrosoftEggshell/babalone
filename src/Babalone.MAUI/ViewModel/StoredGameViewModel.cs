using EVAL.Babalone.Model;

namespace EVAL.Babalone.ViewModel
{
    public class StoredGameViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private DateTime _modified;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Modified
        {
            get => _modified;
            set
            {
                if (_modified != value)
                {
                    _modified = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand? LoadGameCommand { get; set; }

        public DelegateCommand? SaveGameCommand { get; set; }

        public StoredGameViewModel(StoredGameModel storedGame)
        {
            Name = storedGame.Name;
            Modified = storedGame.Modified;
        }
    }
}
