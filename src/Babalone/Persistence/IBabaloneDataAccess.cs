namespace EVAL.Babalone.Persistence
{
    /// <summary>
    /// Defines functions for saving and loading Babalone games.
    /// </summary>
    public interface IBabaloneDataAccess
    {
        /// <summary>
        /// Loads state of game from save file at <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Save file path.</param>
        /// <returns>A <see cref="BabaloneBoard"/> instance with the loaded game state.</returns>
        Task<BabaloneBoard> LoadAsync(string path);

        /// <summary>
        /// Saves state of game to a new file at <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Target file path.</param>
        /// <param name="board">Game board to save.</param>
        Task SaveAsync(string path, BabaloneBoard board);
    }
}
