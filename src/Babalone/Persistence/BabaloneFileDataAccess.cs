namespace EVAL.Babalone.Persistence
{
    /// <summary>
    /// Provides saving and loading functions from Babalone save files.
    /// </summary>
    public class BabaloneFileDataAccess : IBabaloneDataAccess
    {
        /// <inheritdoc/>
        public async Task<BabaloneBoard> LoadAsync(string path)
        {
            BabaloneBoard ret;
            try
            {
                using StreamReader reader = new(path);

                string line = await reader.ReadLineAsync() ?? string.Empty;
                int boardSize = int.Parse(line.Split(' ')[^1]);

                ret = new(boardSize);

                line = await reader.ReadLineAsync() ?? string.Empty;
                int turnCount = int.Parse(line.Split(' ')[^1]);
                ret.Turn = turnCount;

                line = await reader.ReadLineAsync() ?? string.Empty;
                Player currentPlayer = Enum.Parse<Player>($"{line.Last()}");
                ret.ActivePlayer = currentPlayer;

                for (int i = 0; i < boardSize; ++i)
                {
                    line = await reader.ReadLineAsync() ?? string.Empty;
                    string[] row = line.Split(' ');
                    for (int j = 0; j < boardSize; ++j)
                        if (Enum.TryParse(row[j], out Player p))
                            ret[i, j] = p;
                }
            }
            catch (Exception e)
            {
                throw new BabaloneDataException(e.Message);
            }
            return ret;
        }

        /// <inheritdoc/>
        public async Task SaveAsync(string path, BabaloneBoard board)
        {
            try
            {
                using StreamWriter writer = new(path);

                await writer.WriteLineAsync($"Board size: {board.Size}");
                await writer.WriteLineAsync($"Current turn: {board.Turn}");
                await writer.WriteLineAsync($"Current player: {Enum.GetName(board.ActivePlayer)}");
                for (int i = 0; i < board.Size; ++i)
                {
                    for (int j = 0; j < board.Size; ++j)
                    {
                        if (board[i, j] is Player p)
                        {
                            await writer.WriteAsync(Enum.GetName(p) + " ");
                        }
                        else
                        {
                            await writer.WriteAsync("_ ");
                        }
                    }
                    await writer.WriteLineAsync();
                }
            }
            catch (Exception e)
            {
                throw new BabaloneDataException(e.Message);
            }
        }
    }
}
