using System;
using System.Collections.Generic;

namespace CardGame.Core
{
    public class Player
    {
        public string Name { get; set; } = string.Empty;
        public int HP { get; private set; } = 500;
        public int Energy { get; set; } = 0;

        // デッキ（山札）
        public List<Card> Deck { get; set; } = new List<Card>();
        // 手札
        public List<Card> Hand { get; set; } = new List<Card>();
        // 場（Board）
        public List<Card> Board { get; set; } = new List<Card>();

        public Player(string name)
        {
            Name = name;
        }

        /// <summary>
        /// ターン開始時の処理
        /// 仕様：EN+1 → 1ドロー
        /// </summary>
        public void StartTurn()
        {
            Energy++; // human.Energy++; // ターン開始時エネルギー増加
            DrawCard(1);
            Console.WriteLine($"--- {Name} のターン開始 ---");
            Console.WriteLine($"{Name}: HP {HP} | EN {Energy} | Hand {Hand.Count} | Board {Board.Count}");
        }

        /// <summary>
        /// 指定枚数をデッキから引く
        /// </summary>
        public void DrawCard(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (Deck.Count > 0)
                {
                    Card card = Deck[0];
                    Deck.RemoveAt(0);
                    Hand.Add(card);
                    Console.WriteLine($"{Name} は {card.Name} をドローした。");
                }
                else
                {
                    Console.WriteLine($"{Name} のデッキが空です！");
                }
            }
        }

        /// <summary>
        /// ダメージを受ける処理
        /// </summary>
        public void TakeDamage(int damage)
        {
            HP -= damage;
            Console.WriteLine($"{Name} は {damage} ダメージを受けた！ (残りHP: {HP})");
        }

        /// <summary>
        /// 敗北判定
        /// </summary>
        public bool IsDead => HP <= 0;
    }
}
