using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleGame.Model;
using System.Diagnostics;

namespace WordleGame.Services
{
    public class WordleService
    {
        private readonly HttpClient httpClient;
        private List<Wordle> wordsList = new();
        private int currentWordIndex = -1;
        private readonly string[] fallbackWords =
        {
            "APPLE", "BRAVE", "CRANE", "DREAM", "EAGER",
            "FLAME", "GRAPE", "HOUSE", "INDEX", "JOKER"
        };

        public WordleService()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<Wordle>> GetWords()
        {
            if (wordsList.Count > 0)
                return wordsList;

            var wordUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt";
            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    var response = await httpClient.GetAsync(wordUrl);
                    response.EnsureSuccessStatusCode();
                    var text = await response.Content.ReadAsStringAsync();
                    var words = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    wordsList = words
                        .Select(word => word.Trim().ToUpperInvariant())
                        .Where(word => word.Length == 5 && word.All(char.IsLetter))
                        .Distinct()
                        .Select(word => new Wordle { Word = word })
                        .ToList();

                    if (wordsList.Count > 0)
                    {
                        RandomizeWords(wordsList);
                        return wordsList;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Word fetch attempt {attempt} failed: {ex.Message}");
                    if (attempt < maxAttempts)
                    {
                        await Task.Delay(300 * attempt);
                    }
                }
            }

            wordsList = fallbackWords.Select(word => new Wordle { Word = word }).ToList();
            RandomizeWords(wordsList);
            return wordsList;
        }

        internal Wordle GetNextWord()
        {
            if (wordsList.Count == 0)
                throw new InvalidOperationException("No words are loaded.");

            currentWordIndex = (currentWordIndex + 1) % wordsList.Count;
            return wordsList[currentWordIndex];
        }

        private void RandomizeWords(List<Wordle> wordsList)
        {
            var random = new Random();
            for (int i = 0; i < wordsList.Count; i++)
            {
                int randomIndex = random.Next(i, wordsList.Count);
                var order = wordsList[i];
                wordsList[i] = wordsList[randomIndex];
                wordsList[randomIndex] = order;
            }
        }
    }
}
