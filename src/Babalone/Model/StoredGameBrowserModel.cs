using EVAL.Babalone.Persistence;

namespace EVAL.Babalone.Model
{    
    public class StoredGameBrowserModel
    {
        private readonly IStore _store;

        public event EventHandler? StoreChanged;

        public ICollection<StoredGameModel> StoredGames { get; private set; }

        public StoredGameBrowserModel(IStore store)
        {
            _store = store;
            StoredGames = new List<StoredGameModel>();
        }

        public async Task UpdateAsync()
        {
            StoredGames.Clear();

            foreach (string name in await _store.GetFilesAsync())
            {
                if (name == "SuspendedGame")
                    continue;

                StoredGames.Add(new StoredGameModel
                {
                    Name = name,
                    Modified = await _store.GetModifiedTimeAsync(name)
                });
            }

            StoredGames = StoredGames.OrderByDescending(item => item.Modified).ToList();

            StoreChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
