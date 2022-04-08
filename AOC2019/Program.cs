// See https://aka.ms/new-console-template for more information

using AOC2019;

await RunProgramAsync();

async Task RunProgramAsync()
{
    ShowGraphics(lineDelay: 25);
    await RunSolverAsync();
    
}

void ShowGraphics(int lineDelay)
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*************************************************************************************************************************************************");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                                                                                                               *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                                                                                                               *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*      $$$$$$\        $$\                                 $$\                      $$$$$$\         $$$$$$\                  $$\                 *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*     $$  __$$\       $$ |                                $$ |                    $$  __$$\       $$  __$$\                 $$ |                *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*     $$ /  $$ | $$$$$$$ |$$\    $$\  $$$$$$\  $$$$$$$\ $$$$$$\          $$$$$$\  $$ /  \__ |      $$ /  \__| $$$$$$\   $$$$$$$ | $$$$$$\       *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*     $$$$$$$$ |$$  __$$ |\$$\  $$  |$$  __$$\ $$  __$$\\_$$  _|        $$  __$$\ $$$$\           $$ |      $$  __$$\ $$  __$$ |$$  __$$\       *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*     $$  __$$ |$$ /  $$ | \$$\$$  / $$$$$$$$ |$$ |  $$ | $$ |          $$ /  $$ |$$  _|          $$ |      $$ /  $$ |$$ /  $$ |$$$$$$$$ |      *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*     $$ |  $$ |$$ |  $$ |  \$$$  /  $$   ____|$$ |  $$ | $$ |$$\       $$ |  $$ |$$ |            $$ |  $$\ $$ |  $$ |$$ |  $$ |$$   ____|      *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*     $$ |  $$ |\$$$$$$$ |   \$  /   \$$$$$$$\ $$ |  $$ | \$$$$  |      \$$$$$$  |$$ |            \$$$$$$  |\$$$$$$  |\$$$$$$$ |\$$$$$$$\       *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*     \__ |  \__| \_______|    \_/     \_______|\__|  \__|  \____/        \______/ \__|             \______/  \______/  \_______| \_______|     *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                                                                                                               *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                                                                                                               *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                                                                                                               *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                $$$$$$\   $$$$$$\    $$\   $$$$$$\                                                             *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                               $$  __$$\ $$$ __$$\ $$$$ | $$  __$$\                                                            *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                               \__ /  $$ |$$$$\ $$ |\_$$ | $$ /  $$ |                                                          *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                $$$$$$  |$$\$$\$$ |  $$ | \$$$$$$$ |                                                           *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                               $$  ____ / $$ \$$$$ |  $$ |  \____$$ |                                                          *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                               $$ |      $$ |\$$$ |  $$ | $$\   $$ |                                                           *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                               $$$$$$$$\ \$$$$$$  /$$$$$$\\$$$$$$  |                                                           *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                               \________ | \______/ \______|\______/                                                           *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                                                                                                               *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*                                                                                                                                               *");
    Thread.Sleep(lineDelay);
    Console.WriteLine(@"*************************************************************************************************************************************************");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
}

async Task RunSolverAsync()
{
    Console.WriteLine("Select a day (or choose 'x' to exit).");
    var day = Console.ReadLine();
    if (day == "x")
    {
        Environment.Exit(0);
    }
    try
    {
        var puzzleManagerFactory = new PuzzleManagerFactory();
        var puzzleManager = puzzleManagerFactory.CreatePuzzleManager(day!);
        try
        {
            await RunPartSelecterAsync(puzzleManager);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
            await RunPartSelecterAsync(puzzleManager);
        }
    }
    catch (ArgumentException e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine();
        await RunSolverAsync();
    }
    
}

async Task RunPartSelecterAsync(IPuzzleManager puzzleManager)
{
    Console.WriteLine("Select a part. 1, 2 or 3 for both (or choose 'b' to go back or 'x' to exit).");
    var part = Console.ReadKey();
    switch (part.KeyChar)
    {
        case 'x':
            Environment.Exit(0);
            break;
        case 'b':
            Console.WriteLine();
            await RunSolverAsync();
            break;
        case '1':
            Console.WriteLine();
            await puzzleManager.SolvePartOne();
            Console.WriteLine();
            await puzzleManager.Reset();
            await RunPartSelecterAsync(puzzleManager);
            break;
        case '2':
            Console.WriteLine();
            await puzzleManager.SolvePartTwo();
            Console.WriteLine();
            await puzzleManager.Reset();
            await RunPartSelecterAsync(puzzleManager);
            break;
        case '3':
            Console.WriteLine();
            await puzzleManager.SolveBothParts();
            Console.WriteLine();
            await puzzleManager.Reset();
            await RunPartSelecterAsync(puzzleManager);
            break;
        default:
            throw new ArgumentException("Part not recognised.");
    }
}