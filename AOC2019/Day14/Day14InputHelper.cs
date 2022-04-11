using System.Text.RegularExpressions;

namespace AOC2019.Day14
{
    internal class Day14InputHelper : InputHelper<List<Chemical>>
    {
        public Day14InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<Chemical> Parse()
        {
            var output = new List<Chemical>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                var chemicalRegex = new Regex(@"\d+\s[A-Z]+");
                while ((ln = sr.ReadLine()!) != null)
                {
                    var chemicalMatches = chemicalRegex.Matches(ln);

                    var childChemical = output.FirstOrDefault(x => x.Name == GetChemicalName(chemicalMatches.Last().Value));
                    if (childChemical == null)
                    {
                        childChemical = new Chemical(GetChemicalName(chemicalMatches.Last().Value));
                        output.Add(childChemical);
                    }
                    childChemical.SetNumberProduced(GetChemicalNumber(chemicalMatches.Last().Value));

                    for (var i = 0; i < chemicalMatches.Count - 1; i++)
                    {
                        var parentChemical = output.FirstOrDefault(x => x.Name == GetChemicalName(chemicalMatches[i].Value));
                        if (parentChemical == null)
                        {
                            parentChemical = new Chemical(GetChemicalName(chemicalMatches[i].Value));
                            output.Add(parentChemical);
                        }
                        childChemical.AddParent(parentChemical, GetChemicalNumber(chemicalMatches[i].Value));
                    }
                }
            }
            return output;
        }

        private string GetChemicalName(string chemicalInput)
        {
            var nameRegex = new Regex(@"[A-Z]+");
            return nameRegex.Match(chemicalInput).Value;
        }

        private long GetChemicalNumber(string chemicalInput)
        {
            var numberRegex = new Regex(@"\d+");
            return long.Parse(numberRegex.Match(chemicalInput).Value);
        }
    }
}
