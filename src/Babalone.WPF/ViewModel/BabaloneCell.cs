using System;
using System.Windows;

namespace EVAL.Babalone.ViewModel
{
    public class BabaloneCell : ViewModelBase
    {
        #region Fields

        private BabaloneViewModel _parent;
        private string _text = string.Empty;

        #endregion

        #region Commands

        public DelegateCommand? OnClick { get; set; }

        #endregion

        #region Properties

        public string Text
        {
            get => _text;
            private set
            {
                if (_text.Equals(value))
                    return;
                _text = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan RainbowBeginTime => new(0, 0, 0, 0, 100 * (X + Y));

        public bool IsActive => _parent.IsSelectedOrNeighbor(this);

        public int X { get; private set; }

        public int Y { get; private set; }

        #endregion

        #region Constructors

        public BabaloneCell (BabaloneViewModel _parent, int x, int y)
        {
            this._parent = _parent;
            X = x;
            Y = y;
            UpdateText();
        }

        #endregion

        #region Methods

        public void UpdateText()
        {
            Text = _parent.GetPosition(X, Y);
        }

        #endregion
    }
}
