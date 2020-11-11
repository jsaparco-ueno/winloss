using System;

namespace winloss
{
    class Program
    {
        static void Main(string[] args)
        {
            // In Streamlabs OBS choose this file as the target for a text source to display the tally
            var winlossPath = "D:\\streamtools\\wldisp.txt";
            // Game history is stored here, for kicks.  It's not used by the app for tally or streak calculations, but could be used to recover an ongoing tally.
            var historyPath = "D:\\streamtools\\wlhist.txt";

            // initialization, also checks that we can write before we try tallying
            System.IO.File.WriteAllText(winlossPath, "0W 0L");
            var historyText = System.IO.File.ReadAllText(historyPath);
            if (!(historyText.Contains(GetTimestamp())))
            {
                System.IO.File.AppendAllText(historyPath, $"\n{GetTimestamp()}\n");                
            }

            var wins = 0;
            var losses = 0;
            var streak = 0;
            var previousResult = "";

            var keepLooping = true;
            Console.WriteLine("Wins/Losses Tracker");
            Console.WriteLine("Commands are:");
            Console.WriteLine("w - Records a win");
            Console.WriteLine("l - Records a loss");
            Console.WriteLine("reset - Resets the tally to zero");
            
            // begin main execution loop
            while (keepLooping)
            {
                var tally = $"{wins}W {losses}L";
                if (streak >= 3)
                {
                    tally += $"   {streak}{previousResult.ToUpper()} streak";
                }
                Console.WriteLine(tally);
                System.IO.File.WriteAllText(winlossPath, tally);

                var input = Console.ReadLine();

                switch(input)
                {
                    case "w": 
                    {
                        Console.WriteLine("Recording a win");
                        wins++;
                        System.IO.File.AppendAllText(historyPath, "w");
                        
                        if (previousResult == "l") streak = 1;
                        else streak++;

                        previousResult = "w";
                        break;
                    }
                    case "l":
                    {
                        Console.WriteLine("Recording a loss");
                        losses++;
                        System.IO.File.AppendAllText(historyPath, "l");

                        if (previousResult == "w") streak = 1;
                        else streak++;

                        previousResult = "l";
                        break;
                    }
                    case "reset":
                    {
                        Console.WriteLine("Are you sure you want to reset the tally? y/n");
                        var resetConfirm = Console.ReadLine();
                        switch (resetConfirm)
                        {
                            case "y":
                            {
                                Console.WriteLine("Resetting the tally!");
                                wins = 0;
                                losses = 0;
                                streak = 0;
                                previousResult = "";
                                break;
                            }
                            case "n":
                            {
                                Console.WriteLine("Sustaining sequence.");
                                break;
                            }
                        }
                        break;
                    }
                    case "quit":
                    {
                        Console.WriteLine("Goodbye!");
                        keepLooping = false;
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Command not found.  Nothing doing.");
                        break;
                    }
                }
            }
        }

        private static string GetTimestamp()
        {
            var now = DateTime.Now;
            return $"{now.Year}-{now.Month}-{now.Day}";
        }
    }
}
