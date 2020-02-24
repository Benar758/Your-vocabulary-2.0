using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Your_vocabulary_2._0
{
    /// <summary>
    /// Слово
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Создание слова
        /// </summary>
        /// <param name="Word">Слово</param>
        /// <param name="Translation">Перевод</param>
        public Word (string Word, string Translation, string GroupName)
        {
            _Word = Word;
            this.Translation = Translation;
            LearningProgress = 0;
            this.GroupName = GroupName;
            WhenTheWordWasAdded = DateTime.Now;
            TheWordAskedUntil = WhenTheWordWasAdded.AddDays(3);
        }

        public Word () { }

        /// <summary>
        /// Слово
        /// </summary>
        public string _Word { get; set; }

        /// <summary>
        /// Перевод
        /// </summary>
        public string Translation { get; set; }

        public DateTime WhenTheWordWasAdded { get; set; }

        public DateTime TheWordAskedUntil { get; set; }

        /// <summary>
        /// Прогресс изучения слова
        /// </summary>
        public int LearningProgress { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Необходимый прогресс
        /// </summary>
        public const int NeededProgress = 10;

        /// <summary>
        /// Добавить слово
        /// </summary>
        /// <param name="_Word">Слово</param>
        /// <param name="GroupName">Группа для добавления</param>
        public static void Add(Word _Word)
        {
            var selectedGroup = from g in MainWindow.Dictionary.Groups where g.Name == _Word.GroupName select g;
            selectedGroup.FirstOrDefault().Words.Add(_Word);
        }
    }
}
