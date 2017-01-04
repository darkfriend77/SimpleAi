using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HearthDb.Enums;
using HsProtoSim.Model;
using HsProtoSim.Tasks;
using log4net;
using SimpleAi.Score;

namespace SimpleAi.Nodes
{
    internal class OptionNode
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly OptionNode _parent;

        private readonly Game _game;

        private readonly int _playerId;

        public PlayerTask PlayerTask { get; }

        public string Hash;

        private int _gameState = 0;
        public bool IsWon => _gameState > 0;

        public bool IsLost => _gameState < 0;

        public bool IsRunning => _gameState == 0;

        private int _endTurn = 0;
        public bool IsEndTurn => _endTurn > 0;

        public bool IsRoot => PlayerTask == null;

        public int Score { get; private set; } = 0;

        public OptionNode(OptionNode parent, Game game, int playerId, PlayerTask playerTask)
        {
            _parent = parent;
            _game = game.Clone(); // create clone
            _playerId = playerId;
            PlayerTask = playerTask;

            if (!IsRoot)
                Execute();
        }

        public void Execute()
        {
            _game.Process(PlayerTask);

            var controller = _game.ControllerById(_playerId);

            _gameState = _game.State == State.RUNNING ? 0
                : (controller.PlayState == PlayState.WON ? 1 : -1);

            _endTurn = _game.CurrentPlayer.Id != _playerId ? 1 : 0;

            Hash = _game.Hash(GameTag.LAST_CARD_PLAYED);

            var aggroScore = new AggroScore(controller);
            Score = aggroScore.Rate();
        }

        public void PlayerTasks(ref List<PlayerTask> list)
        {
            if (_parent == null)
                return;

            _parent.PlayerTasks(ref list);
            list.Add(PlayerTask);
        }

        public void Options(ref Dictionary<string, OptionNode> optionNodes)
        {
            var options = _game.Options(_game.ControllerById(_playerId));
            foreach (var option in options)
            {
                var optionNode = new OptionNode(this, _game, _playerId, option);
                if (!optionNodes.ContainsKey(optionNode.Hash))
                    optionNodes.Add(optionNode.Hash, optionNode);
            }
        }

        public static List<OptionNode> GetSolutions(Game game, int playerId, int maxDepth = 10, int maxWidth = 10000)
        {
            var depthNodes = new Dictionary<string, OptionNode> { ["root"] = new OptionNode(null, game, playerId, null) };
            var endTurnNodes = new List<OptionNode>();
            for (var i = 0; depthNodes.Count > 0 && i < maxDepth; i++)
            {
                var nextDepthNodes = new Dictionary<string, OptionNode>();
                foreach (var option in depthNodes.Values)
                {
                    option.Options(ref nextDepthNodes);
                }
                
                endTurnNodes.AddRange(nextDepthNodes.Values.Where(p => p.IsEndTurn || p.IsWon));

                depthNodes = nextDepthNodes
                    .Where(p => !p.Value.IsEndTurn && p.Value.IsRunning)
                    .OrderByDescending(p => p.Value.Score)
                    .Take(maxWidth)
                    .ToDictionary(p => p.Key, p => p.Value);

                Log.Info($"Depth: {i + 1} --> {depthNodes.Count}/{nextDepthNodes.Count} options! [SOLUTIONS:{endTurnNodes.Count}]");
            }
            return endTurnNodes;
        }

    }
}