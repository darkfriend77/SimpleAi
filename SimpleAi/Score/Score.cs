using System;
using System.Linq;
using HsProtoSim.Model;

namespace SimpleAi.Score
{
    internal interface IScore
    {
        int Rate();
    }

    public class Score : IScore
    {
        private readonly Controller _controller;

        public int HeroHp => _controller.Hero.Health;

        public int OpHeroHp => _controller.Opponent.Hero.Health;

        public int HeroAtk => _controller.Hero.TotalAttackDamage;

        public int OpHeroAtk => _controller.Opponent.Hero.TotalAttackDamage;

        public Zone<IPlayable> Hand => _controller.Hand;

        public int HandTotCost => Hand.Sum(p => p.Cost);

        public int HandCnt => _controller.Hand.Count;

        public int OpHandCnt => _controller.Opponent.Hand.Count;

        public Zone<Minion> Board => _controller.Board;

        public Zone<Minion> OpBoard => _controller.Opponent.Board;

        public int MinionTotAtk => Board.Sum(p => p.AttackDamage);

        public int OpMinionTotAtk => OpBoard.Sum(p => p.AttackDamage);

        public int MinionTotHealth => Board.Sum(p => p.Health);

        public int OpMinionTotHealth => OpBoard.Sum(p => p.Health);

        public int MinionTotHealthTaunt => Board.Where(p => p.HasTaunt).Sum(p => p.Health);

        public int OpMinionTotHealthTaunt => OpBoard.Where(p => p.HasTaunt).Sum(p => p.Health);

        public Score(Controller controller)
        {
            _controller = controller;
        }
        public virtual int Rate()
        {
            return 0;
        }

        static Func<T> Remeber<T>(Func<T> getValue)
        {
            var isCached = false;
            var cachedResult = default(T);

            return () =>
            {
                if (isCached) return cachedResult;
                cachedResult = getValue();
                isCached = true;
                return cachedResult;
            };
        }

    }

    public class AggroScore : Score
    {
        public AggroScore(Controller controller) : base(controller)
        {
        }

        public override int Rate()
        {
            if (OpHeroHp < 1)
                return int.MaxValue;

            if (HeroHp < 1)
                return int.MinValue;

            var result = 0;

            if (OpBoard.Count == 0 && Board.Count > 0)
                result += 1000;

            if (OpMinionTotHealthTaunt > 0)
                result += MinionTotHealthTaunt * -1000;

            result += MinionTotAtk;

            result += (HeroHp - OpHeroHp) * 1000;

            return result;
        }
    }

    public class SimpleScore : Score
    {
        public SimpleScore(Controller controller) : base(controller)
        {
        }

        public override int Rate()
        {
            if (OpHeroHp < 1)
                return int.MaxValue;

            if (HeroHp < 1)
                return int.MinValue;

            var result = (HeroHp - OpHeroHp) * 100000;

            if (OpMinionTotAtk + OpHeroAtk >= HeroHp)
                result -= 100000;

            if (OpMinionTotAtk + OpHeroAtk < MinionTotHealthTaunt)
                result += 50000;

            if (MinionTotAtk + HeroAtk >= OpHeroHp)
                result += 10000;

            if (MinionTotAtk + HeroAtk < OpMinionTotHealthTaunt)
                result -= 5000;

            if (MinionTotAtk > OpMinionTotAtk)
                result += 1000;

            if (OpMinionTotAtk > MinionTotHealth)
                result -= 1000;

            if (HeroHp > OpHeroHp)
                result += 100;

            if (HeroHp < OpHeroHp)
                result -= 100;

            if (HeroAtk > OpHeroAtk)
                result += 50;

            if (HeroAtk < OpHeroAtk)
                result -= 50;

            result -= OpHeroHp;

            result += HeroHp;

            result -= HandTotCost;

            return result;
        }
    }
}
