using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HearthDb.Enums;
using HsProtoSim.Config;
using HsProtoSim.Model;
using HsProtoSim.Tasks;
using log4net;
using SimpleAi.Nodes;

namespace SimpleAi
{
    class Program
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
            var game = new Game(MageMirrorConfig(false));
            game.StartGame();
            game.Player1.BaseMana = 5;
            while (game.State != State.COMPLETE)
            {
                Log.Info($"Player1: {game.Player1.PlayState} / Player2: {game.Player2.PlayState} " +
                         $"----- TURN {game.Turn}");
                Log.Info($"Hero[P1]: {game.Player1.Hero.Health} / Hero[P2]: {game.Player2.Hero.Health}");

                while (game.CurrentPlayer == game.Player1)
                {
                    Log.Info($"* Calculating solutions *** Player 1 ***");
                    var solutions = OptionNode.GetSolutions(game, game.Player1.Id, 10, 1000);
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
                    var options = game.Options(game.CurrentPlayer);
                    var option = options[Rnd.Next(options.Count)];
                    Log.Info($"[{option.FullPrint()}]");
                    game.Process(option);
                }
            }
            Log.Info($"Game: {game.State}, Player1: {game.Player1.PlayState} / Player2: {game.Player2.PlayState}");
        }

        public static GameConfig MageMirrorConfig(bool shuffle)
        {
            return new GameConfig
            {
                StartPlayer = 1,
                Player1Name = "FitzVonGerald",
                Player1HeroClass = CardClass.MAGE,
                DeckPlayer1 = new List<Card>()
                {
                    Cards.FromName("Mirror Image"),
                    Cards.FromName("Sorcerer's Apprentice"),
                    Cards.FromName("Ice Lance"),
                    Cards.FromName("Frostbolt"),
                    Cards.FromName("Ironforge Rifleman"),
                    Cards.FromName("Mirror Image"),
                    Cards.FromName("Ironforge Rifleman"),
                    Cards.FromName("Gnomish Inventor"),
                    Cards.FromName("Fireball"),
                    Cards.FromName("Fireball"),
                    Cards.FromName("Gnomish Inventor"),
                    Cards.FromName("Ice Lance"),
                    Cards.FromName("Sorcerer's Apprentice"),
                    Cards.FromName("Frostwolf Grunt"),
                    Cards.FromName("Raid Leader"),
                    Cards.FromName("Kobold Geomancer"),
                    Cards.FromName("Stormpike Commando"),
                    Cards.FromName("Sen'jin Shieldmasta"),
                    Cards.FromName("Elven Archer"),
                    Cards.FromName("Frostwolf Grunt"),
                    Cards.FromName("Stormpike Commando"),
                    Cards.FromName("Elven Archer"),
                    Cards.FromName("Stormwind Champion"),
                    Cards.FromName("Frostwolf Warlord"),
                    Cards.FromName("Voodoo Doctor"),
                    Cards.FromName("Raid Leader"),
                    Cards.FromName("Kobold Geomancer"),
                    Cards.FromName("Sen'jin Shieldmasta"),
                    Cards.FromName("Voodoo Doctor"),
                    Cards.FromName("Frostwolf Warlord")
                },
                Player2Name = "RehHausZuckFuchs",
                Player2HeroClass = CardClass.MAGE,
                DeckPlayer2 = new List<Card>()
                {
                    Cards.FromName("Mirror Image"),
                    Cards.FromName("Sorcerer's Apprentice"),
                    Cards.FromName("Ice Lance"),
                    Cards.FromName("Frostbolt"),
                    Cards.FromName("Ironforge Rifleman"),
                    Cards.FromName("Mirror Image"),
                    Cards.FromName("Ironforge Rifleman"),
                    Cards.FromName("Gnomish Inventor"),
                    Cards.FromName("Fireball"),
                    Cards.FromName("Fireball"),
                    Cards.FromName("Gnomish Inventor"),
                    Cards.FromName("Ice Lance"),
                    Cards.FromName("Sorcerer's Apprentice"),
                    Cards.FromName("Frostwolf Grunt"),
                    Cards.FromName("Raid Leader"),
                    Cards.FromName("Kobold Geomancer"),
                    Cards.FromName("Stormpike Commando"),
                    Cards.FromName("Sen'jin Shieldmasta"),
                    Cards.FromName("Elven Archer"),
                    Cards.FromName("Frostwolf Grunt"),
                    Cards.FromName("Stormpike Commando"),
                    Cards.FromName("Elven Archer"),
                    Cards.FromName("Stormwind Champion"),
                    Cards.FromName("Frostwolf Warlord"),
                    Cards.FromName("Voodoo Doctor"),
                    Cards.FromName("Raid Leader"),
                    Cards.FromName("Kobold Geomancer"),
                    Cards.FromName("Sen'jin Shieldmasta"),
                    Cards.FromName("Voodoo Doctor"),
                    Cards.FromName("Frostwolf Warlord")
                },
                FillDecks = false,
                Shuffle = shuffle,
            };
        }
    }
}