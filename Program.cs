class Program
{

    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Diffuse the Bomb! Here is your question!");
        var asyncTask = RunTheGame();
        asyncTask.Wait();

        static async Task<string> ActivateBombAsync(CancellationToken cancellationToken)
        {
            for (int a = 1000; a >= 0; a--)
            {
                await Task.Delay(1, CancellationToken.None);
                Console.Write("\r {0}:{1} <- Answer quick! = ", a / 100, a % 100);
            }
            return "Timer run out! Game Over!";
        }

        CancellationTokenSource cancellation = new CancellationTokenSource();
        var result = ActivateBombAsync(cancellation.Token);
        static string PuzzleApp()
        {
            static Puzzle GenerateAPuzzle()
            {

                static (int Left, Operators Operator, int Right) generateAnEquation()
                {
                    var rnd = new Random();
                    var left = rnd.Next(1, 10);
                    var right = rnd.Next(1, 10);
                    var operation = (Operators)rnd.Next(3);
                    return (left, operation, right);
                }

                var (Left, Operator, Right) = generateAnEquation();

                static Puzzle createPuzzle(int Left, Operators Operator, int Right) => Operator switch
                {
                    Operators.Sum => new Puzzle($"{Left} + ? = {Left + Right}", $"{Right}"),
                    Operators.Minus => new Puzzle($"{Left} - ? = {Left - Right}", $"{Right}"),
                    Operators.Multiply => new Puzzle($"{Left} * ? = {Left * Right}", $"{Right}"),
                    Operators.Divide => new Puzzle($"{Left} / ? = {Left / Right}", $"{Right}"),
                    _ => throw new ArgumentOutOfRangeException($"{nameof(Operator)}: {Operator}")
                };
                return createPuzzle(Left, Operator, Right);

            }
            Puzzle puzzle = GenerateAPuzzle();
            Console.WriteLine($"\r{puzzle.Question}");
            var usersAnswer = Console.ReadLine();
            string message = puzzle.Evaluate(usersAnswer);
            return message;
        }

        static async Task RunTheGame()
        {
            CancellationTokenSource cancellation = new CancellationTokenSource();
            string finalMessage = await await Task.WhenAny(ActivateBombAsync(cancellation.Token), Task.Run(PuzzleApp));
            cancellation.Cancel();
            Console.Clear();
            Console.WriteLine($"\r{finalMessage}");
        }

    }

    enum Operators : int
    {
        Sum = 0, Minus = 1, Multiply = 2, Divide = 3
    }

    record Puzzle(string Question, string Answer)
    {
        public string Evaluate(string usersAnswer)
        => usersAnswer switch
        {
            "" => "You didn't answer! Disqualified!",
            _ when usersAnswer == Answer => $"You Won! Your answer of {usersAnswer} was correct!",
            _ => $"The correct answer is {Answer}, but you entered: {usersAnswer}! Disqualified!"
        };
    }

}
