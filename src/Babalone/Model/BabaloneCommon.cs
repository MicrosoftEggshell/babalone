namespace EVAL.Babalone.Model
{
    /// <summary>
    /// Generic algorithms used in Babalone.
    /// </summary>
    static internal class BabaloneCommon
    {
        /// <summary>
        /// Generates a shuffled array of numbers from 1 up to
        /// <paramref name="size"/> using the built-in <see cref="Random"/> class.
        /// </summary>
        /// <param name="size">Length of range and its largest number.</param>
        /// <returns><paramref name="size"/> length array of shuffled <see cref="int"/>s.</returns>
        /// <exception cref="ArgumentException">If <paramref name="size"/> is negative.</exception>
        /// 
        /// Implements the Fischer-Yater algorithm for shuffling
        /// a range of numbers based off of the Wikipedia article:
        /// <see href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_%22inside-out%22_algorithm"/>
        public static int[] ShuffledRange(int size)
        {
            if (size < 0)
                throw new ArgumentException($"{nameof(size)} must be 0 or greater.");

            int[] ret = new int[size];
            Random rng = new();
            for (int i = 0; i < size; ++i)
            {
                int j = rng.Next(i + 1);
                ret[i] = ret[j];
                ret[j] = i;
            }
            return ret;
        }
    }
}
