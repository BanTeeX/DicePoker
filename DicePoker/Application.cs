namespace DicePoker;

public static class Application
{
    public static void Loop()
    {
        while (true)
        {
            Console.Clear();

            Console.Write("""
                1. Start game
                2. Start learning
                3. Test AI against random player
                4. Exit
                Choose option: 
                """);

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Game();
                    break;
                case "2":
                    Learn();
                    break;
                case "3":
                    Test();
                    break;
                case "4":
                    return;
            }
        }
    }

    private static void Game()
    {
        Console.Clear();

        Console.Write("Specify Q-Table file for AI (default QTable.json): ");
        var input = Console.ReadLine();
        var path = string.IsNullOrEmpty(input) ? "QTable.json" : input;

        Console.Clear();

        var game = new DicePokerGame(path);
        game.Run();

        Console.Read();
    }

    private static void Learn()
    {
        Console.Clear();

        Console.Write("How many iterations (default 1 000 000): ");
        var input = Console.ReadLine();
        var iterations = string.IsNullOrEmpty(input) ? 1_000_000 : int.Parse(input);

        Console.Clear();

        Console.Write("Do you want to generate new Q-Table (y/n default n): ");
        input = Console.ReadLine();
        var generate = !string.IsNullOrEmpty(input) && input == "y";

        Console.Clear();

        var path = "QTable.json";
        if (generate)
        {
            Console.Write("Specify file for new Q-Table (or override default QTable.json): ");
            input = Console.ReadLine();
            path = string.IsNullOrEmpty(input) ? path : input;
        }

        Console.Clear();

        Console.Write("Specify learning rate (range 0-1 default 0.1): ");
        input = Console.ReadLine();
        var learningRate = string.IsNullOrEmpty(input) ? 0.1 : double.Parse(input);

        Console.Clear();

        Console.Write("Specify exploration rate (range 0-1 default 0.1): ");
        input = Console.ReadLine();
        var explorationRate = string.IsNullOrEmpty(input) ? 0.1 : double.Parse(input);

        Console.Clear();

        var teacher = new AITeacher(generate, path, learningRate, explorationRate);
        teacher.Learn(iterations);
        teacher.SaveQTable();

        Console.Read();
    }

    private static void Test()
    {
        Console.Clear();

        Console.Write("How many iterations (default 1 000 000): ");
        var input = Console.ReadLine();
        var iterations = string.IsNullOrEmpty(input) ? 1_000_000 : int.Parse(input);

        Console.Clear();

        Console.Write("Specify Q-Table file for AI (default QTable.json): ");
        input = Console.ReadLine();
        var path = string.IsNullOrEmpty(input) ? "QTable.json" : input;

        Console.Clear();

        var teacher = new AITeacher(false, path);
        teacher.CheckAgaistRandom(iterations);

        Console.Read();
    }
}
