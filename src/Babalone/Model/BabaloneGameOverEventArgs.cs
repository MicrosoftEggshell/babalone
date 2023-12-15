using EVAL.Babalone.Persistence;

namespace EVAL.Babalone.Model
{
    /// <summary>
    /// Provides data for the <see cref="BabaloneModel.GameOver"/> event handler.
    /// </summary>
    public class BabaloneGameOverEventArgs : EventArgs
    {
        /// <summary>
        /// Points achieved by each player.
        /// </summary>
        public IReadOnlyDictionary<Player, int> Points { get; private set; }

        /// <summary>
        /// Player who won the game, or null if game was a tie.
        /// </summary>
        public Player? Winner { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneGameOverEventArgs"/> class.
        /// </summary>
        /// <param name="points">Points achieved by each player.</param>
        public BabaloneGameOverEventArgs(IReadOnlyDictionary<Player, int> points)
        {
            Points = points;
            Winner = null;

            List<KeyValuePair<Player, int>> pointsList = Points.ToList();
            pointsList.Sort((p1, p2) => p2.Value - p1.Value);
            if (pointsList[0].Value != pointsList[1].Value)
            {
                // Unambiguous winner exists
                Winner = pointsList[0].Key;
            }
        }
    }
}
