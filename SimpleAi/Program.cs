using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HearthDb.Enums;
using HsProtoSim.Config;
using HsProtoSim.Model;
using HsProtoSim.Tasks;
using HsProtoSim.Tasks.PlayerTasks;
using log4net;
using SimpleAi.Meta;
using SimpleAi.Nodes;
using SimpleAi.Score;

namespace SimpleAi
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Random Rnd = new Random();

        private static void Main(string[] args)
        {
            Log.Info("Starting test setup.");

            OptionNodeTest();

            Log.Info("Test end!");
            Console.ReadLine();
        }

        public static void OptionNodeTest()
        {
            var game = new Game(
                new GameConfig()
                {
                    StartPlayer = 1,
                    Player1Name = "FitzVonGerald",
                    Player1HeroClass = CardClass.WARRIOR,
                    DeckPlayer1 = Decks.WarriorPirate,
                    Player2Name = "RehHausZuckFuchs",
                    Player2HeroClass = CardClass.MAGE,
                    DeckPlayer2 = Decks.SimpleMage,
                    FillDecks = false,
                    Shuffle = true
                });
            game.StartGame();
            while (game.State != State.COMPLETE)
            {
                Log.Info("");
                Log.Info($"Player1: {game.Player1.PlayState} / Player2: {game.Player2.PlayState} - " +
                         $"ROUND {(game.Turn + 1) / 2} - {game.CurrentPlayer.Name}");
                Log.Info($"Hero[P1]: {game.Player1.Hero.Health} / Hero[P2]: {game.Player2.Hero.Health}");
                Log.Info("");
                while (game.CurrentPlayer == game.Player1)
                {
                    Log.Info($"* Calculating solutions *** Player 1 ***");
                    var solutions = OptionNode.GetSolutions(game, game.Player1.Id, 10, 500);
                    var solution = new List<PlayerTask>();
                    solutions.OrderByDescending(p => p.Score).First().PlayerTasks(ref solution);
                    Log.Info($"- Player 1 - <{game.CurrentPlayer.Name}> ---------------------------");
                    solution.ForEach(p =>
                    {
                        Log.Info(p.FullPrint());
                        game.Process(p);
                    });
                }

                // Random mode for Player 2
                Log.Info($"- Player 2 - <{game.CurrentPlayer.Name}> ---------------------------");
                while (game.CurrentPlayer == game.Player2)
                {
                    //var options = game.Options(game.CurrentPlayer);
                    //var option = options[Rnd.Next(options.Count)];
                    //Log.Info($"[{option.FullPrint()}]");
                    //game.Process(option);
                    Log.Info($"* Calculating solutions *** Player 1 ***");
                    var solutions = OptionNode.GetSolutions(game, game.Player2.Id, 10, 500);
                    var solution = new List<PlayerTask>();
                    solutions.OrderByDescending(p => p.Score).First().PlayerTasks(ref solution);
                    Log.Info($"- Player 1 - <{game.CurrentPlayer.Name}> ---------------------------");
                    solution.ForEach(p =>
                    {
                        Log.Info(p.FullPrint());
                        game.Process(p);
                    });
                }
            }
            Log.Info($"Game: {game.State}, Player1: {game.Player1.PlayState} / Player2: {game.Player2.PlayState}");
        }

    }
}