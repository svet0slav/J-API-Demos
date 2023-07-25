namespace GameModel.Abstract
{
    /// <summary>
    /// Random number generator for different games.
    /// </summary>
    public interface IGameRandomNumberGenerator
    {
        /// <summary>
        /// Calculates random value between 0.00 and 1.00.
        /// </summary>
        /// <returns>Double random value between 0.00 and 1.00</returns>
        double GetRandom();
    }
}
