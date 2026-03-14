namespace CardGame.Core
{
    // カードの種類を定義
    public enum CardType
    {
        Unit,   // ユニット（場に出て戦う）
        Spell   // 呪文（即時発動）
    }

    public class Card
    {
        // 基本情報
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CardType Type { get; set; }
        public int Cost { get; set; }

        // ユニット専用パラメータ（Type == Unit の時使用）
        public int Attack { get; set; }
        public int Health { get; set; }

        // 呪文専用パラメータ（Type == Spell の時使用）
        public int Damage { get; set; }
        public bool TargetUnitFirst { get; set; } // true: ユニット優先, false: 本体優先

        // 画面表示用の文字列を返す（デバッグやコンソール表示用）
        public override string ToString()
        {
            if (Type == CardType.Unit)
            {
                return $"[U] {Name.PadRight(8)} (Cost:{Cost}) ATK:{Attack} / HP:{Health}";
            }
            else
            {
                string target = TargetUnitFirst ? "UnitFirst" : "Direct";
                return $"[S] {Name.PadRight(8)} (Cost:{Cost}) DMG:{Damage} ({target})";
            }
        }

        // カードを複製するメソッド（デッキから手札に入れる際などに使用）
        public Card Clone() => (Card)this.MemberwiseClone();
    }
}
