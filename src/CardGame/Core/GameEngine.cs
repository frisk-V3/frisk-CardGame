using System;
using System.Linq;

namespace CardGame.Core
{
    public class GameEngine
    {
        /// <summary>
        /// 呪文カードの効果処理
        /// 仕様：TargetUnitFirst = true なら攻撃力最大のユニット、いなければ本体。
        /// </summary>
        public void CastSpell(Card spell, Player user, Player opponent)
        {
            if (user.Energy < spell.Cost) return;
            user.Energy -= spell.Cost;

            int dmg = spell.Damage;

            // ターゲット判定
            if (spell.TargetUnitFirst && opponent.Board.Any())
            {
                // 相手の場にユニットがいれば 攻撃力最大のユニットにダメージ
                var target = opponent.Board.OrderByDescending(u => u.Attack).First();
                target.Health -= dmg;
                Console.WriteLine($"呪文 {spell.Name} が {target.Name} に {dmg} ダメージ！");
            }
            else
            {
                // TargetUnitFirst = false または 敵ユニットがいない場合
                opponent.TakeDamage(dmg);
                Console.WriteLine($"呪文 {spell.Name} が本体 {opponent.Name} に {dmg} ダメージ！");
            }

            CleanBoard(user, opponent);
        }

        /// <summary>
        /// ユニット召喚
        /// </summary>
        public void SummonUnit(Card unit, Player user)
        {
            if (user.Energy < unit.Cost) return;
            user.Energy -= unit.Cost;
            user.Board.Add(unit);
            Console.WriteLine($"{user.Name} が {unit.Name} を召喚！");
        }

        /// <summary>
        /// 戦闘フェイズ（ユニット攻撃）
        /// 仕様：HPが最も低いユニットを攻撃。いなければ本体。相打ちあり。
        /// </summary>
        public void ExecuteBattle(Player attacker, Player defender)
        {
            Console.WriteLine($"--- {attacker.Name} の攻撃フェイズ ---");
            
            // 攻撃側ユニット全員が1回ずつ攻撃
            foreach (var unit in attacker.Board.ToList())
            {
                if (defender.Board.Any())
                {
                    // 相手の場にユニットがいる -> HPが最も低いユニットを攻撃
                    var target = defender.Board.OrderBy(u => u.Health).First();
                    
                    Console.WriteLine($"{unit.Name} (ATK:{unit.Attack}) が {target.Name} (HP:{target.Health}) を攻撃！");
                    
                    // 戦闘処理：お互いのATK分HPを減らす（相打ちあり）
                    target.Health -= unit.Attack;
                    unit.Health -= target.Attack;
                }
                else
                {
                    // いない -> 本体を攻撃
                    defender.TakeDamage(unit.Attack);
                    Console.WriteLine($"{unit.Name} が {defender.Name} にダイレクトアタック！");
                }
            }

            CleanBoard(attacker, defender);
        }

        /// <summary>
        /// ボードの掃除
        /// 仕様：HP <= 0 のユニットを除去
        /// </summary>
        private void CleanBoard(params Player[] players)
        {
            foreach (var p in players)
            {
                int removedCount = p.Board.RemoveAll(u => u.Health <= 0);
                if (removedCount > 0)
                {
                    Console.WriteLine($"{p.Name} のユニットが倒れた。");
                }
            }
        }
    }
}
