namespace AOC2019.Day6
{
    internal class Planet
    {
        private List<Planet> _children = new List<Planet>();

        public string Name { get; private set; }
        public Planet? Parent { get; private set; } = null;
        public IReadOnlyCollection<Planet> Children => _children.AsReadOnly();

        public Planet(string name)
        {
            Name = name;
        }

        public void SetParent(Planet parent)
        {
            Parent = parent;
        }
        public void AddChild(Planet child)
        {
            _children.Add(child);
        }
        public int CountParents()
        {
            if (Parent == null)
            {
                return 0;
            }
            return 1 + Parent.CountParents();
        }

        private void ReturnAllParents(List<Planet> parents)
        {
            if (Parent == null)
            {
                return;
            }
            parents.Add(Parent);
            Parent.ReturnAllParents(parents);
        }

        internal int FindOrbitalTransfersBetween(Planet otherPlanet)
        {
            var thisParents = new List<Planet>();
            ReturnAllParents(thisParents);


            var otherParents = new List<Planet>();
            otherPlanet.ReturnAllParents(otherParents);

            var notIntersection = thisParents.ToArray().Except(otherParents.ToArray()).Union(otherParents.ToArray().Except(thisParents.ToArray()));

            return notIntersection.Count();
        }
    }
}
