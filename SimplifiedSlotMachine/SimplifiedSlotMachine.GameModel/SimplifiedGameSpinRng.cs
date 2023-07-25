using GameModel.Abstract;

namespace SimplifiedSlotMachine.GameModel
{
    // Class for random values generator.
    public class SimplifiedGameSpinRng: IGameRandomNumberGenerator
    {
        protected Random random;

        public SimplifiedGameSpinRng() {
            random = new Random();
            Initialize();
        }

        protected void Initialize()
        {
            // To rely on random numbers, must rotate random generator first.
            var rotations = 5 + 2 * (DateTime.Now.Second / 3);

            for (int i = 0; i < rotations; i++)
            {
                random.Next();
            }
        }

        /// <summary>
        /// Calculates random value between 0.00 and 1.00
        /// </summary>
        /// <returns>Double random value between 0.00 and 1.00</returns>
        public double GetRandom()
        {
            return random.NextDouble();
        }
    }
}
