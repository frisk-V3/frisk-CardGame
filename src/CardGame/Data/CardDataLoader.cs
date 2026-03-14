using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using CardGame.Core;

namespace CardGame.Data
{
    public static class CardDataLoader
    {
        // 埋め込みリソースのパス（プロジェクト名.フォルダ名.ファイル名）
        private const string ResourcePath = "CardGame.Data.MasterData.json";

        /// <summary>
        /// EXE内に埋め込まれたJSONから100種類のカードリストを読み込む
        /// </summary>
        public static List<Card> LoadMasterData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            // リソースストリームを開く
            using (Stream? stream = assembly.GetManifestResourceStream(ResourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"埋め込みリソースが見つかりません: {ResourcePath}");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    
                    // JSONオプション（大文字小文字を区別しないなど）
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var cardList = JsonSerializer.Deserialize<List<Card>>(json, options);
                    
                    if (cardList == null)
                    {
                        return new List<Card>();
                    }

                    Console.WriteLine($"MasterData.json から {cardList.Count} 種類のカードをロードしました。");
                    return cardList;
                }
            }
        }
    }
}
