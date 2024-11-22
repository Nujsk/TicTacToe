using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] board = new string [9];
        private bool playerTurn = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!playerTurn)
            {
                return;
            }
            Button? clickedButton = sender as Button;
            int index = GetButtonIndex(clickedButton!);
            if (index == -1 || board[index] != null)
            {
                return;
            }
            board[index] = "X";
            clickedButton!.Content = "X";
            clickedButton.Foreground = Brushes.Green;
            clickedButton.IsEnabled = false;
            if (CheckWinner("X"))
            {
                MessageBox.Show("You win!");
                ResetGame();
                return;
            }
            if (IsBoardFull())
            {
                MessageBox.Show("It's a draw!");
                ResetGame();
                return;
            }
            playerTurn = false;
            ComputerMove();
        }

        private int GetButtonIndex(Button button)
        {
            switch (button.Name)
            {
                case "Button0": return 0;
                case "Button1": return 1;
                case "Button2": return 2;
                case "Button3": return 3;
                case "Button4": return 4;
                case "Button5": return 5;
                case "Button6": return 6;
                case "Button7": return 7;
                case "Button8": return 8;
                default: return -1;
            }
        }

        // disgusting way to check winner
        private bool CheckWinner(string player)
        {
            int[][] winConditions =
            [
                [0, 1, 2], // top row
                [3, 4, 5], // mid row
                [6, 7, 8], // bot row
                [0, 3, 6], // left col
                [1, 4, 7], // mid col
                [2, 5, 8], // right col
                [0, 4, 8], // dia \
                [2, 4, 6]  // dia /
            ];
            
            // check each condition. if all 3 indexes of condition matches player, in this case "X", player wins
            foreach(var contition in winConditions)
            {
                if (board[contition[0]] == player &&
                    board[contition[1]] == player &&
                    board[contition[2]] == player)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsBoardFull()
        {
            return board.All(cell => cell != null);
        }

        private void ResetGame()
        {
            board = new string[9];
            if (Content is Grid grid)
            {
                foreach (var child in grid.Children)
                {
                    if (child is Button btn)
                    {
                        btn.Content = "";
                        btn.IsEnabled = true;
                    }
                }
            }
            playerTurn = true;
        }

        private void ComputerMove()
        {
            var availableIndex = board
                .Select((value, index) => new { value, index })
                .Where(x => x.value == null)
                .Select(x => x.index)
                .ToList();

            if(availableIndex.Count == 0)
            {
                return;
            }

            Random random = new Random();
            int computerIndex = availableIndex[random.Next(availableIndex.Count)];
            board[computerIndex] = "O";

            Button? computerButton = FindName($"Button{computerIndex}") as Button;
            if (computerButton != null)
            {
                computerButton.Content = "O";
                computerButton.Foreground = Brushes.Red;
                computerButton.IsEnabled = false;
            }
            if(CheckWinner("O"))
            {
                MessageBox.Show("Computer wins!");
                ResetGame();
                return;
            }
            if (IsBoardFull())
            {
                MessageBox.Show("It's a draw!");
                ResetGame();
                return;
            }
            playerTurn = true;
        }
    }
}