using System.Reflection;
using System.Text.Json;
using CardGame.Core;

namespace CardGame.Data {
    public static class CardDataLoader {
        public static List<Card> Load() {
            var assembly = Assembly.GetExecutingAssembly();
            // LogicalNameで指定した名前で直接取得
            using var stream = assembly.GetManifestResourceStream("MasterData.json");
            
            if (stream == null) {
                // デバッグ用：リソースが見つからない場合に全リソース名を表示
                var names = string.Join(", ", assembly.GetManifestResourceNames());
                throw new Exception($"Resource not found. Available: {names}");
            }

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<List<Card>>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }
}
