namespace AOC2019.Day14
{
    internal class Chemical
    {
        public string Name { get; private set; }
        public long NumberProduced { get; set; } = 0;
        public Dictionary<Chemical, long> Parents { get; set; } = new Dictionary<Chemical, long>();

        public Chemical(string name)
        {
            Name = name;
        }

        public void SetNumberProduced(long numberProduced)
        {
            NumberProduced = numberProduced;
        }

        public void AddParent(Chemical parent, long amount)
        {
            Parents.Add(parent, amount);
        }
        public long CountParents(long numberRequired, Dictionary<Chemical, long> chemicalStockpile)
        {
            if (!Parents.Any())
            {
                return numberRequired;
            }

            if (!chemicalStockpile.ContainsKey(this))
            {
                chemicalStockpile[this] = 0;
            }

            if (chemicalStockpile[this] >= numberRequired)
            {
                chemicalStockpile[this] -= numberRequired;
                return 0;
            }

            numberRequired -= chemicalStockpile[this];
            chemicalStockpile[this] = 0;
            long multiplesRequired = 1;
            if (NumberProduced < numberRequired)
            {
                multiplesRequired = numberRequired / NumberProduced;
                multiplesRequired += numberRequired % NumberProduced == 0 ? 0 : 1;
            }
            chemicalStockpile[this] = NumberProduced * multiplesRequired - numberRequired;
            long sum = 0;
            foreach (var keyValuePair in Parents)
            {
                sum += keyValuePair.Key.CountParents(multiplesRequired * keyValuePair.Value, chemicalStockpile);
            }
            return sum;
        }
    }
}
