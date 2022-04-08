namespace AOC2019.Day6
{
    internal class Day6InputHelper : InputHelper<List<Planet>>
    {
        public Day6InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Planet> Parse()
        {
            var output = new List<Planet>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    var planets = ln.Split(')');

                    var parentPlanet = output.FirstOrDefault(x => x.Name == planets[0]);
                    var childPlanet = output.FirstOrDefault(x => x.Name == planets[1]);

                    var addParent = false;
                    if (parentPlanet == null)
                    {
                        addParent = true;
                        parentPlanet = new Planet(planets[0]);
                    }

                    var addChild = false;
                    if (childPlanet == null)
                    {
                        addChild = true;
                        childPlanet = new Planet(planets[1]);
                    }

                    childPlanet.SetParent(parentPlanet);
                    parentPlanet.AddChild(childPlanet);

                    if (addParent)
                    {
                        output.Add(parentPlanet);
                    }
                    if (addChild)
                    {
                        output.Add(childPlanet);
                    }
                }
            }
            return output;
        }
    }
}
