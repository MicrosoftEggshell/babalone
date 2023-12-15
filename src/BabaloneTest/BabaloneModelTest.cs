using EVAL.Babalone.Model;
using EVAL.Babalone.Persistence;
using Moq;

namespace BabaloneTest
{
    [TestClass]
    public class BabaloneModelTest
    {
        private BabaloneModel _model = null!; // a tesztelendő modell
        private BabaloneBoard _mockedBoard = null!; // mockolt játéktábla
        private Mock<IBabaloneDataAccess> _mock = null!; // az adatelérés mock-ja

        [TestInitialize]
        public void Initialize()
        {
            _mockedBoard = new BabaloneBoard(4);
            _mockedBoard[0, 0] = Player.B;
            _mockedBoard[0, 1] = Player.A;
            _mockedBoard[0, 2] = Player.A;
            _mockedBoard[1, 1] = Player.B;

            _mock = new Mock<IBabaloneDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(_mockedBoard));

            _model = new BabaloneModel(_mock.Object);

            _model.CellChanged += Model_CellChanged;
            _model.GameOver += Model_GameOver;
            _model.GameAdvanced += Model_GameAdvanced;
        }

        [TestMethod]
        public void BabaloneGameModelNewGame3Test() => BabaloneGameModelNewGameTest(3);

        [TestMethod]
        public void BabaloneGameModelNewGame4Test() => BabaloneGameModelNewGameTest(4);

        [TestMethod]
        public void BabaloneGameModelNewGame6Test() => BabaloneGameModelNewGameTest(6);

        public void BabaloneGameModelNewGameTest(int boardSize)
        {
            _model = new BabaloneModel(_mock.Object, boardSize);

            Assert.AreEqual(1, _model.Turn);
            Assert.AreEqual(boardSize, _model.BoardSize);
            Assert.AreEqual(5 * boardSize, _model.MaxTurns);

            int playerACount = 0;
            int playerBCount = 0;

            for (int i = 0; i < boardSize; ++i)
            {
                for(int j = 0; j < boardSize; ++j)
                {
                    switch (_model.GetPosition(i, j))
                    {
                        case Player.A:
                            ++playerACount;
                            break;
                        case Player.B:
                            ++playerBCount;
                            break;
                    }
                }
            }

            Assert.AreEqual(boardSize, playerACount);
            Assert.AreEqual(boardSize, playerBCount);
        }

        [TestMethod]
        public void SudokuGameModelLoadTest()
        {
            _model = _model.LoadGameAsync("paath").Result;
            Assert.AreEqual(1, _model.Turn);

            Assert.AreEqual(Player.A, _model.GetPosition(0, 2));
            _model.Push(0, 2, 0, 1);
            Assert.AreEqual(1, _model.Turn);
            Assert.AreEqual(Player.A, _model.GetPosition(0, 1));
            Assert.AreEqual(Player.A, _model.GetPosition(0, 0));
            Assert.AreEqual(null, _model.GetPosition(0, 2));
            Assert.AreEqual(Player.B, _model.GetPosition(1, 1));

            _model.Push(1, 1, 0, 1);
            Assert.AreEqual(2, _model.Turn);
            Assert.AreEqual(Player.A, _model.GetPosition(0, 0));
            Assert.AreEqual(Player.B, _model.GetPosition(0, 1));
            Assert.AreEqual(null, _model.GetPosition(0, 2));
            Assert.AreEqual(null, _model.GetPosition(1, 1));

            int turn = 2;

            do
            {
                _model.Push(0, 0, 0, 1);
                Assert.AreEqual(null, _model.GetPosition(0, 0), $"turn {turn}");
                Assert.AreEqual(Player.A, _model.GetPosition(0, 1), $"turn {turn}");
                Assert.AreEqual(Player.B, _model.GetPosition(0, 2), $"turn {turn}");

                _model.Push(0, 2, 0, 1);
                Assert.AreEqual(Player.A, _model.GetPosition(0, 0), $"turn {turn}");
                Assert.AreEqual(Player.B, _model.GetPosition(0, 1), $"turn {turn}");
                Assert.AreEqual(null, _model.GetPosition(0, 2), $"turn {turn}");

                ++turn;
            }
            while (!_model.IsGameOver);

            Assert.AreEqual(20, turn);
            Assert.AreEqual(turn, _model.Turn);
            Assert.AreEqual(_model.Turn, _model.MaxTurns);
        }

        [TestMethod]
        public void InvalidMoveTest()
        {
            _model = new BabaloneModel(_mock.Object);

            // moving from empty cells
            for(int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (_model.GetPosition(i, j) is null)
                    {
                        try
                        {
                            _model.Push(i, j, i+1, j+1);
                            Assert.Fail();
                        }
                        catch { }
                    }
                }
            }

            // pushing off board
            try
            {
                _model.Push(0, 0, -1, 0);
                Assert.Fail();
            }
            catch { }

            try
            {
                _model.Push(1, 2, 1, 3);
                Assert.Fail();
            }
            catch { }

            // pushing diagonally
            try
            {
                _model.Push(0, 0, 1, 1);
                Assert.Fail();
            }
            catch { }

            // pushing over multiple cells
            try
            {
                _model.Push(0, 0, 0, 2);
                Assert.Fail();
            }
            catch { }
        }

        private void Model_GameAdvanced(object? sender, EventArgs e)
        {
            Assert.IsFalse(_model.IsGameOver);
        }

        private void Model_CellChanged(object? sender, BabaloneCellChangedEventArgs e)
        {
            Assert.IsFalse(_model.IsGameOver);
            Assert.IsTrue(_model.Turn > 0);
            Assert.IsTrue(e.X is >= 0 and < 4);
            Assert.IsTrue(e.Y is >= 0 and < 4);
        }

        private void Model_GameOver(object? sender, BabaloneGameOverEventArgs e)
        {
            bool hasLoser = e.Points.Any(p => p.Value <= 0);
            bool timeout = _model.Turn >= _model.MaxTurns;

            Assert.IsTrue(_model.IsGameOver);
            Assert.IsTrue(timeout || hasLoser);

            if (e.Winner is Player)
            {
                Assert.IsTrue(hasLoser);
                Assert.AreEqual(e.Winner, 
                    (from kvp
                    in e.Points
                    orderby kvp.Value descending
                    select kvp.Key).First());
            }
            else
            {
                Assert.IsFalse(hasLoser);
                Assert.IsTrue(timeout);
            }
        }
    }
}