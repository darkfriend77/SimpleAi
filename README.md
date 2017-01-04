# SimpleAi
Simple Hearthstone A.I.

... create a Game ... 

            var game = new Game(new GameConfig
            {
                StartPlayer = 1,
                Player1HeroClass = CardClass.SHAMAN,
                Player2HeroClass = CardClass.SHAMAN,
                FillDecks = true
            });

... start ...

            game.StartGame();

... play ...          

            for(var i = 0; i < turns; i++)
            {
                var options = game.Options(game.CurrentPlayer);
                var option = options[rnd.Next(options.Count)];
                game.Process(option);
            }
            
... there is a lot more ... but no time need to code ^^

thx to this project which inspired me a lot ... and was a great base of solutions ...

https://github.com/HearthSim/Brimstone
