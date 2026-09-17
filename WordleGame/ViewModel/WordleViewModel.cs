using System.Collections.ObjectModel;
using System.Diagnostics;
using WordleGame.Services;

namespace WordleGame.ViewModel
{
    public class WordleViewModel : BaseViewModel
    {
        private const int WordLength = 5;
        private const int TotalAttempts = 6;

        private readonly WordleService wordleService;
        private string selectedWord = string.Empty;
        private string playerAnswer = string.Empty;
        private string playerName;
        private int maxAttempts = TotalAttempts;
        private bool isGameOver;
        private string statusMessage = string.Empty;

        public ObservableCollection<GuessResultRow> GuessResult { get; } = new();

        public bool IsGameOver
        {
            get => isGameOver;
            set
            {
                if (isGameOver != value)
                {
                    isGameOver = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSubmit));
                    SubmitAnswerCommand.ChangeCanExecute();
                }
            }
        }

        public int MaxAttempts
        {
            get => maxAttempts;
            set
            {
                if (maxAttempts != value)
                {
                    maxAttempts = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSubmit));
                    SubmitAnswerCommand.ChangeCanExecute();
                }
            }
        }

        public string SelectWord
        {
            get => selectedWord;
            private set
            {
                if (selectedWord != value)
                {
                    selectedWord = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PlayerAnswer
        {
            get => playerAnswer;
            set
            {
                var sanitized = SanitizeGuess(value);
                if (playerAnswer != sanitized)
                {
                    playerAnswer = sanitized;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSubmit));
                    SubmitAnswerCommand.ChangeCanExecute();
                }
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                if (statusMessage != value)
                {
                    statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSubmit => !IsBusy && !IsGameOver && MaxAttempts > 0 && PlayerAnswer.Length == WordLength;

        public Command SubmitAnswerCommand { get; }
        public Command NewGameCommand { get; }

        public WordleViewModel(WordleService wordleService)
        {
            Title = "Wordle";
            this.wordleService = wordleService;
            SubmitAnswerCommand = new Command(async () => await SubmitAnswerAsync(), () => CanSubmit);
            NewGameCommand = new Command(async () => await NewGameAsync(), () => !IsBusy);
            playerName = Preferences.Get("PlayerName", "Player");

            _ = NewGameAsync();
        }

        private async Task NewGameAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = string.Empty;
                MaxAttempts = TotalAttempts;
                PlayerAnswer = string.Empty;
                GuessResult.Clear();
                IsGameOver = false;

                await wordleService.GetWords();
                SelectWord = wordleService.GetNextWord().Word;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                StatusMessage = "Unable to start a new game.";
                await Shell.Current.DisplayAlert("Error", "Unable to start a new game.", "OK");
            }
            finally
            {
                IsBusy = false;
                SubmitAnswerCommand.ChangeCanExecute();
                NewGameCommand.ChangeCanExecute();
            }
        }

        private async Task SubmitAnswerAsync()
        {
            if (!CanSubmit)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = string.Empty;

                var guess = PlayerAnswer.ToUpperInvariant();
                if (guess.Length != WordLength || !guess.All(char.IsLetter))
                {
                    StatusMessage = "Enter exactly 5 letters.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectWord))
                {
                    StatusMessage = "No game word is loaded. Start a new game.";
                    return;
                }

                var feedback = EvaluateGuess(guess, SelectWord);
                GuessResult.Insert(0, new GuessResultRow
                {
                    Guess = guess,
                    Pattern = feedback.Pattern,
                    Message = feedback.Message
                });

                MaxAttempts--;

                if (feedback.IsWin)
                {
                    IsGameOver = true;
                    StatusMessage = "You guessed the word!";
                    EndGame();
                    await Shell.Current.DisplayAlert("You Win!", "Congratulations! You've guessed the word correctly.", "OK");
                    return;
                }

                if (MaxAttempts == 0)
                {
                    IsGameOver = true;
                    StatusMessage = $"Game over! The word was {SelectWord}.";
                    EndGame();
                    await Shell.Current.DisplayAlert("Game Over", $"You've run out of attempts. The word was: {SelectWord}.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                StatusMessage = "Unable to submit answer.";
                await Shell.Current.DisplayAlert("Error", "Unable to submit answer.", "OK");
            }
            finally
            {
                PlayerAnswer = string.Empty;
                IsBusy = false;
                SubmitAnswerCommand.ChangeCanExecute();
                NewGameCommand.ChangeCanExecute();
            }
        }

        private (string Pattern, string Message, bool IsWin) EvaluateGuess(string guess, string word)
        {
            var statuses = new char[WordLength];
            var remaining = new Dictionary<char, int>();

            for (int i = 0; i < WordLength; i++)
            {
                if (guess[i] == word[i])
                {
                    statuses[i] = 'G';
                }
                else
                {
                    statuses[i] = 'X';
                    remaining[word[i]] = remaining.GetValueOrDefault(word[i]) + 1;
                }
            }

            for (int i = 0; i < WordLength; i++)
            {
                if (statuses[i] == 'G')
                    continue;

                var letter = guess[i];
                if (remaining.TryGetValue(letter, out var count) && count > 0)
                {
                    statuses[i] = 'Y';
                    remaining[letter] = count - 1;
                }
            }

            var isWin = statuses.All(c => c == 'G');
            var message = isWin ? "Correct!" : "G=correct spot, Y=wrong spot, X=not in word";
            return (new string(statuses), message, isWin);
        }

        private string SanitizeGuess(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return new string(value.Where(char.IsLetter).Take(WordLength).ToArray()).ToUpperInvariant();
        }

        private void EndGame()
        {
            var attemptsUsed = TotalAttempts - MaxAttempts;
            var scoreboardViewModel = new ScoreboardViewModel();
            scoreboardViewModel.AddScore(playerName, SelectWord, attemptsUsed);
        }
    }

    public class GuessResultRow
    {
        public string Guess { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
