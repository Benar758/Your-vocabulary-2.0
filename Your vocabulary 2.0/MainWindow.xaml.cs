using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Your_vocabulary_2._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Словарь
        /// </summary>
        public static Dictionary Dictionary { get; set; }

        /// <summary>
        /// Окно тренировки
        /// </summary>
        public static Training Tr { get; set; }

        public static Group CurrentGroup { get; set; }

        bool JustStarted { get; set; } = true;

        public static MainWindow MW;

        public InstructionsPage InstFrame;

        public MainWindow()
        {
            MW = this;
            Dictionary = new Dictionary();

            Data.Load();

            InitializeComponent();

            InstFrame = new InstructionsPage();

            foreach (Group group in Dictionary.Groups)
            {
                ListOfGroups.Items.Add(group.Name);
            }

            var selectedGroup = from g in Dictionary.Groups where g.Name == "Разное" select g;
            if (selectedGroup.FirstOrDefault() == null)
            {
                Dictionary.Groups.Add(new Group("Разное"));
                ListOfGroups.Items.Add("Разное");
                Data.Save();
            }

            string groupName = ListOfGroups.Items[0].ToString();
            ListOfGroups.SelectedItem = ListOfGroups.Items[0];

            CurrentGroup = Dictionary[groupName];

            var sortedWords = from g in Dictionary.Groups where g.Name == groupName from word 
                              in g.Words orderby word.LearningProgress descending select word;

            int count = 1;

            foreach (Word word in sortedWords)
            {
                ListOfWords.Items.Add($"{count++}. {word._Word.TrimEnd(' ')} - {word.Translation.TrimStart(' ')}");
            }

            SetNumberOfWords();
        }

        /// <summary>
        /// Кнопка добавления слова нажата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewWordButton_Click(object sender, RoutedEventArgs e)
        {
            string word = NewWordTextBox.Text.Trim();
            string translation = NewTranslationTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(translation))
            {
                MessageBox.Show("Не все поля заполнены!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedWord = from g in Dictionary.Groups from w in g.Words where w._Word.ToLower() == word.ToLower() select w._Word;
            string foundWord = selectedWord.FirstOrDefault();

            if (!string.IsNullOrEmpty(foundWord))
            {
                MessageBox.Show("Вы уже добавляли такое слово!", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string groupName = ListOfGroups.SelectedItem.ToString();

            Word.Add(new Word(word.TrimEnd(' '), translation.TrimStart(' '), groupName));

            Data.Save();

            NewWordTextBox.Text = string.Empty;
            NewTranslationTextBox.Text = string.Empty;

            ListOfWords.Items.Add($"{Dictionary[groupName].Words.Count}. {word.TrimStart(' ')} - {translation.TrimStart(' ')}");
            SetNumberOfWords();
        }

        /// <summary>
        /// Кнопка начала тренировки нажата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTraining_Click(object sender, RoutedEventArgs e)
        {
            Tr = new Training();

            if (!Tr.noWords)
            {
                Tr.Show();

                ListOfWords.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Кнопка Enter нажата в рамках поля перевода слова
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewTranslationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) NewWordButton_Click(new object(), new RoutedEventArgs());
        }

        private void NewGroupButton_Click(object sender, RoutedEventArgs e)
        {
            string groupName = NewGroupTextBox.Text;
            var selectedGroup = from g in Dictionary.Groups where g.Name == groupName select g;

            if (selectedGroup.FirstOrDefault() != null)
            {
                MessageBox.Show("Такая группа уже существует!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Группа добавлена", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            Dictionary.Groups.Add(new Group(groupName));
            ListOfGroups.Items.Add(groupName);
            Data.Save();
        }

        private void ListOfGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListOfWords.Items.Clear();
            Group group = Dictionary[ListOfGroups.SelectedItem.ToString()];
            CurrentGroup = Dictionary[group.Name];

            var sortedWords = from w in CurrentGroup.Words
                              orderby w.LearningProgress descending
                              select w;

            if (!JustStarted)
            {
                int count = 1;
                foreach (Word word in sortedWords)
                {
                    ListOfWords.Items.Add($"{count++}. {word._Word.TrimEnd(' ')} - {word.Translation.TrimStart(' ')}");
                }
            }
            JustStarted = false;

            SetNumberOfWords();
        }

        /// <summary>
        /// Окно закрыто
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch
            {
                return;
            }
        }

        private void NewGroupTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) NewGroupButton_Click(new object(), new RoutedEventArgs());
            SetNumberOfWords();
        }

        private void ListOfWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                string[] words = ListOfWords.SelectedItem.ToString().Split('.', '-');
                string word = words[1];

                Word wordObject = CurrentGroup[word];

                ListOfWords.Items.Remove(ListOfWords.SelectedItem);
                CurrentGroup.Words.Remove(wordObject);

                Data.Save();

                ListOfWords.Items.Clear();

                var sortedWords = from g in Dictionary.Groups
                                  where g.Name == CurrentGroup.Name
                                  from w
                                  in g.Words
                                  orderby w.LearningProgress descending
                                  select w;

                int count = 1;

                foreach (Word w in sortedWords)
                {
                    ListOfWords.Items.Add($"{count++}. {w._Word.TrimEnd(' ')} - {w.Translation.TrimStart(' ')}");
                }

                SetNumberOfWords();
            }
            else if (e.Key == Key.Space)
            {
                
            }
        }

        /// <summary>
        /// Показывает пользователю, сколько всего у него есть слов, и сколько слов он уже выучил
        /// </summary>
        public void SetNumberOfWords()
        {
            NumberOfWords.Text = $"Всего у вас слов: {CurrentGroup.Words.Count}";
            LearntWords.Text = $"Изученных слов: {(from w in CurrentGroup.Words where w.LearningProgress == 10 select w).ToList().Count}";
        }

        private void InstBtn_Click(object sender, RoutedEventArgs e)
        {
            InstBtn.Visibility = Visibility.Hidden;

            if (InstructionFrame.Visibility == Visibility.Hidden)
            {
                InstructionFrame.Visibility = Visibility.Visible;
            }

            InstructionFrame.Content = InstFrame;
        }
    }
}
