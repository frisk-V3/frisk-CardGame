using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Core;
using CardGame.Data;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 本格カード対戦ゲーム 起動 ===");

            // 1. データロード
            var masterCards = CardDataLoader.LoadMasterData();
            if (masterCards.Count == 0) {
                Console.WriteLine("カードデータのロードに失敗しました。");
                return;
            }

            // 2. プレイヤーとCPUの初期化 (HP 500)
            var human = new Player("Player");
            var cpu = new Player("CPU");
            var engine = new GameEngine();

            // 3. デッキ構築 (100種類からランダムに40枚ずつ配布)
            var rnd = new Random();
            human.Deck = Enumerable.Range(0, 40).Select(_ => masterCards[rnd.Next(masterCards.Count)].Clone()).ToList();
            cpu.Deck = Enumerable.Range(0, 40).Select(_ => masterCards[rnd.Next(masterCards.Count)].Clone()).ToList();

            // 初期ドロー (5枚)
            human.DrawCard(5);
            cpu.DrawCard(5);

            // 4. ゲームループ
            while (true)
            {
                // --- プレイヤーのターン ---
                human.StartTurn();
                PlayerAction(human, cpu, engine);
                if (cpu.IsDead) { Console.WriteLine("あなたの勝利！"); break; }

                // --- CPUのターン ---
                cpu.StartTurn();
                CpuAction(cpu, human, engine);
                if (human.IsDead) { Console.WriteLine("CPUの勝利..."); break; }

                Console.WriteLine("\n--- ターン終了。Enterで次へ ---");
                Console.ReadLine();
            }
        }

        // プレイヤーの行動選択 (簡易入力)
        static void PlayerAction(Player p, Player o, GameEngine engine)
        {
            while (true)
            {
                Console.WriteLine($"\n[手札]");
                for (int i = 0; i < p.Hand.Count; i++) Console.WriteLine($"{i}: {p.Hand[i]}");
                Console.WriteLine("使用するカード番号を入力 (sで攻撃フェイズへ):");

                string input = Console.ReadLine() ?? "";
                if (input == "s") break;

                if (int.TryParse(input, out int idx) && idx >= 0 && idx < p.Hand.Count)
                {
                    var card = p.Hand[idx];
                    if (p.Energy >= card.Cost)
                    {
                        p.Hand.RemoveAt(idx);
                        if (card.Type == CardType.Unit) engine.SummonUnit(card, p);
                        else engine.CastSpell(card, p, o);
                    }
                    else Console.WriteLine("ENが足りません！");
                }
            }
            engine.ExecuteBattle(p, o);
        }

        // CPUの行動ルール (ENの限りコストの高い順に使う)
        static void CpuAction(Player p, Player o, GameEngine engine)
        {
            var playable = p.Hand.Where(c => c.Cost <= p.Energy).OrderByDescending(c => c.Cost).ToList();
            foreach (var card in playable)
            {
                p.Hand.Remove(card);
                if (card.Type == CardType.Unit) engine.SummonUnit(card, p);
                else engine.CastSpell(card, p, o);
            }
            engine.ExecuteBattle(p, o);
        }
    }
}
