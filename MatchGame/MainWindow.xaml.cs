using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int counter;
        int matchesFound;
        // Best time
        float timeRecord = 120.0F;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            counter--;
            timeTextBlock.Text = counter.ToString("0s");
            // If all matches found or timer expires
            if (matchesFound == 8 || counter == 0)
            {
                timer.Stop();
                // Remove seconds from time
                string timeStr = timeTextBlock.Text.Remove(timeTextBlock.Text.Length - 1);
                // Parse time to float
                float currTime = 100.0F - float.Parse(timeStr);

                // New time record
                if (timeRecord > currTime)
                {
                    timeRecord = currTime;
                    bestTimeTextBlock.Text = "Best time: " + timeRecord + "s";
                }
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🦥", "🦥",
                "🐘", "🐘",
                "🐆", "🐆",
                "🦒", "🦒",
                "🦧", "🦧",
                "🦌", "🦌",
                "🐫", "🐫",
                "🦔", "🦔",
                "🦊", "🦊",
                "🦛", "🦛",
                "🐶", "🐶",
                "🐱", "🐱",
                "🐷", "🐷",
                "🐭", "🐭",
                "🐰", "🐰",
                "🐢", "🐢",
                "🦘", "🦘",
                "🐧", "🐧",
                "🦩", "🦩"

            };

            Random random = new Random();
            List<string> subsetAnimals = new List<string>();

            // Get a subset of animalEmoji
            for (int i = 0; i < 8; i++)
            {
                // Generate an even number
                int index = random.Next(0, (animalEmoji.Count/2) - 1) * 2;
                // Add pair
                subsetAnimals.Add(animalEmoji[index]);
                subsetAnimals.Add(animalEmoji[index + 1]);
                // Remove pair
                animalEmoji.RemoveAt(index);
                animalEmoji.RemoveAt(index);
            }

            // Display subset
            foreach (TextBlock txtBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (txtBlock.Name != "timeTextBlock" && txtBlock.Name != "bestTimeTextBlock")
                {
                    txtBlock.Visibility = Visibility.Visible;
                    // Get a random number between 0 and the no of emojis left
                    int index = random.Next(subsetAnimals.Count);
                    string nextEmoji = subsetAnimals[index];
                    // Display emoji at TextBlock
                    txtBlock.Text = nextEmoji;
                    subsetAnimals.RemoveAt(index);
                }
                
            }

            timer.Start();
            counter = 120;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            // If only one TextBlock is clicked
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            // If there is a match with last TextBlock clicked
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            // If pair isn't a match
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8 || counter == 0)
            {
                SetUpGame();
            }
        }
    }
}
