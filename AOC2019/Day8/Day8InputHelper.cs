namespace AOC2019.Day8
{
    internal class Day8InputHelper : InputHelper<List<List<List<int>>>>
    {
        public Day8InputHelper(string fileName) : base(fileName)
        {
        }

        public override List<List<List<int>>> Parse()
        {
            int pixelWidth = 25;
            int pixelHeight = 6;
            int numberOfLayers;
            bool endOfRow = false;
            bool endOfLayer = false;

            var input = new List<List<List<int>>>();
            using (var sr = new StreamReader(InputPath))
            {
                string ln;
                while ((ln = sr.ReadLine()!) != null)
                {
                    int totalLength = ln.Count<char>();
                    numberOfLayers = totalLength / (pixelWidth * pixelHeight);

                    List<List<int>> pixelLayer = new List<List<int>>();
                    List<int> pixelRow = new List<int>();
                    int counter = 0;

                    foreach (char digit in ln)
                    {
                        int digitAsInt = int.Parse(digit.ToString());
                        pixelRow.Add(digitAsInt);

                        counter++;

                        if (counter % pixelWidth == 0)
                        {
                            endOfRow = true;
                        }

                        if (endOfRow == true)
                        {
                            pixelLayer.Add(pixelRow);
                            pixelRow = new List<int>();
                            endOfRow = false;
                        }

                        if (counter % (pixelWidth * pixelHeight) == 0)
                        {
                            endOfLayer = true;
                        }

                        if (endOfLayer == true)
                        {
                            input.Add(pixelLayer);
                            pixelLayer = new List<List<int>>();
                            endOfLayer = false;
                        }

                    }
                }
            }
            return input;
        }
    }
}
