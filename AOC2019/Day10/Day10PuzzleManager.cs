namespace AOC2019.Day10
{
    internal class Day10PuzzleManager : PuzzleManager
    {
        public (int, int) Dimensions { get; set; }
        public List<(int, int)> Asteroids { get; private set; }

        public Day10PuzzleManager()
        {
            var inputHelper = new Day10InputHelper(INPUT_FILE_NAME);
            (Dimensions, Asteroids) = inputHelper.Parse();
        }

        public override Task SolveBothParts()
        {
            SolvePartOne();
            Console.WriteLine();
            SolvePartTwo();
            return Task.CompletedTask;
        }

        public override Task SolvePartOne()
        {
            var maxAsteroidsSeen = 0;
            foreach (var currentAsteroid in Asteroids)
            {
                IEnumerable<(int, int)> visibleAsteroids = FindVisibleAsteroids(currentAsteroid);
                maxAsteroidsSeen = Math.Max(maxAsteroidsSeen, visibleAsteroids.Count());
            }
            Console.WriteLine($"The solution to part one is '{maxAsteroidsSeen}'.");
            return Task.CompletedTask;
        }

        private IEnumerable<(int, int)> FindVisibleAsteroids((int, int) currentAsteroid)
        {
            var hiddenPoints = new List<(int, int)>();
            foreach (var asteroid in Asteroids)
            {
                if (asteroid == currentAsteroid)
                {
                    continue;
                }

                var relativeLocation = new int[] { asteroid.Item1 - currentAsteroid.Item1, asteroid.Item2 - currentAsteroid.Item2 };
                Simplify(relativeLocation);

                var outOfBounds = false;
                var counter = 1;
                while (!outOfBounds)
                {
                    if (asteroid.Item1 + relativeLocation[0] * counter < 0 ||
                        asteroid.Item1 + relativeLocation[0] * counter > Dimensions.Item1 ||
                        asteroid.Item2 + relativeLocation[1] * counter < 0 ||
                        asteroid.Item2 + relativeLocation[1] * counter > Dimensions.Item2)
                    {
                        outOfBounds = true;
                        break;
                    }
                    var hiddenPoint = (asteroid.Item1 + relativeLocation[0] * counter, asteroid.Item2 + relativeLocation[1] * counter);
                    if (!hiddenPoints.Contains(hiddenPoint))
                    {
                        hiddenPoints.Add(hiddenPoint);
                    }
                    counter++;
                }
            }
            var visibleAsteroids = Asteroids.Where(x => !hiddenPoints.Contains(x) && x != currentAsteroid);
            return visibleAsteroids;
        }

        public override Task SolvePartTwo()
        {
            var maxAsteroidsSeen = 0;
            var asteroidLaserBase = (0, 0);
            foreach (var currentAsteroid in Asteroids)
            {
                var visibleAsteroids = FindVisibleAsteroids(currentAsteroid);
                if (visibleAsteroids.Count() > maxAsteroidsSeen)
                {
                    maxAsteroidsSeen = visibleAsteroids.Count();
                    asteroidLaserBase = currentAsteroid;
                }
            }

            var asteroidsDestoyed = 0;
            while (asteroidsDestoyed < 200)
            {
                var visibleAsteroids = FindVisibleAsteroids(asteroidLaserBase).OrderBy(x => GetAngle((x.Item1 - asteroidLaserBase.Item1, x.Item2 - asteroidLaserBase.Item2)));
                foreach (var asteroid in visibleAsteroids)
                {
                    Asteroids.Remove(asteroid);
                    if (++asteroidsDestoyed == 200)
                    {
                        Console.WriteLine($"The solution to part two is '{asteroid.Item1 * 100 + asteroid.Item2}'.");
                        return Task.CompletedTask;
                    }
                }
            }            
            return Task.CompletedTask;
        }

        public override void Reset()
        {
            var inputHelper = new Day10InputHelper(INPUT_FILE_NAME);
            (Dimensions, Asteroids) = inputHelper.Parse();
        }

        private void Simplify(int[] numbers)
        {
            var isNumeratorNegative = numbers[0] < 0;
            numbers[0] = Math.Abs(numbers[0]);
            var isDenominatorNegative = numbers[1] < 0;
            numbers[1] = Math.Abs(numbers[1]);
            int gcd = GCD(numbers);
            for (int i = 0; i < numbers.Length; i++)
                numbers[i] /= gcd;
            if (isNumeratorNegative) numbers[0] *= -1;
            if (isDenominatorNegative) numbers[1] *= -1;
        }

        private int GCD(int a, int b)
        {
            while (b > 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }
            return a;
        }

        private int GCD(int[] args)
        {
            return args.Aggregate((gcd, arg) => GCD(gcd, arg));
        }

        private double GetAngle((int, int) relativePosition)
        {
            (int x, int y) = relativePosition;
            if (y == 0)
            {
                if (x > 0)
                {
                    return Math.PI / 2;
                }
                return 3 * Math.PI / 2;
            }
            if (x == 0)
            {
                if (y < 0)
                {
                    return 0;
                }
                return Math.PI;
            }
            var xDouble = (double)x;
            var yDouble = (double)y;
            var arcTan = Math.Atan(xDouble / yDouble);
            if (xDouble > 0 && yDouble < 0)
            {
                return Math.Abs(arcTan);
            }
            if ((xDouble > 0 && yDouble > 0) || (xDouble < 0 && yDouble > 0))
            {
                return Math.PI - arcTan;
            }
            if (xDouble < 0 && yDouble < 0)
            {
                return 2 * Math.PI - arcTan;
            }
            throw new ArithmeticException("Maths broke.");
        }
    }
}
