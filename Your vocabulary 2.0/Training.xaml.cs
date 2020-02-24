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
using System.Windows.Shapes;

namespace Your_vocabulary_2._0
{
    /// <summary>
    /// Логика взаимодействия для Training.xaml
    /// </summary>
    public partial class Training : Window
    {
        public List<Word> words;

        string word;
        string translation;

        Word wordObject;

        public bool noWords = false;

        public static Training TR;

        public Training()
        {
            TR = MainWindow.Tr;

            var selectedWords = from w in MainWindow.CurrentGroup.Words where w.LearningProgress < 10 select w;
            var allWords = selectedWords.ToList();

            words = new List<Word>();
            Random r = new Random();

            if (allWords.Count == 0)
            {
                noWords = true;
                MessageBox.Show("Вы уже выучили все слова! Добавьте новые :)", Title, MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return;
            }
            else if (allWords.Count <= 10)
            {
                words = allWords;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int index = r.Next(allWords.Count);
                    words.Add(allWords[index]);
                    allWords.RemoveAt(index);
                }
            }

            InitializeComponent();

            Answer.Focus();
            GetWord();
        }

        /// <summary>
        /// Получение следующего слова для тренировки
        /// </summary>
        private void GetWord()
        {
            if (words.Count == 0)
            {
                MessageBox.Show($"Тренировка окончена. Приходите ещё!",
                                Title, MessageBoxButton.OK, MessageBoxImage.Information);
                word = string.Empty;
                translation = string.Empty;

                Visibility = Visibility.Hidden;

                MainWindow.MW.ListOfWords.Items.Clear();
                var selectedWords = from w in MainWindow.CurrentGroup.Words orderby w.LearningProgress descending select w;

                int count = 1;
                foreach (Word word in selectedWords)
                {
                    MainWindow.MW.ListOfWords.Items.Add($"{count++}. {word._Word.TrimEnd(' ')} - {word.Translation.TrimStart(' ')}");
                }

                MainWindow.MW.ListOfWords.Visibility = Visibility.Visible;
                MainWindow.MW.SetNumberOfWords();
                return;
            }

            Random r = new Random();

            int index = r.Next(words.Count);
            wordObject = words[index];

            word = wordObject._Word;
            translation = wordObject.Translation;

            words.RemoveAt(index);

            WordToAsk.Content += word;
        }

        /// <summary>
        /// Кнопка ответа нажата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(translation)) return;

            string[] answers = translation.Split(',');

            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = answers[i].ToLower().Trim();
            }

            string givenAnswer = Answer.Text.ToLower();

            if (givenAnswer == translation.ToLower().TrimStart(' ') || answers.Contains(givenAnswer))
            {
                int wordIndex = MainWindow.CurrentGroup.Words.IndexOf(wordObject);

                Word word = MainWindow.CurrentGroup.Words[wordIndex];

                if (word.LearningProgress <= 9)
                {
                    if (word.LearningProgress < 9)
                    {
                        MainWindow.CurrentGroup.Words[wordIndex].LearningProgress++;
                        Data.Save();
                    }
                    else if (word.LearningProgress == 9 && word.TheWordAskedUntil <= DateTime.Now)
                    {
                        MainWindow.CurrentGroup.Words[wordIndex].LearningProgress++;
                        Data.Save();
                    }
                }

                Answer.Text = string.Empty;
                WordToAsk.Content = "Переведите: ";
                GetWord();
            }
            else
            {
                int wordIndex = MainWindow.CurrentGroup.Words.IndexOf(wordObject);
                MainWindow.CurrentGroup.Words[wordIndex].LearningProgress -= 3;
                Data.Save();

                MessageBox.Show($"Ошибка!\nПравильный ответ: {translation}\nПопробуйте снова", 
                                Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Нажата кнопка Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) AnswerButton_Click(new object(), new RoutedEventArgs());
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.MW.ListOfWords.Visibility = Visibility.Visible;
        }
    }
}
