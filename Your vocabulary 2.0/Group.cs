using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Your_vocabulary_2._0
{
    public class Group
    {
        public Group(string Name)
        {
            this.Name = Name;
            Words = new List<Word>();
        }
        
        public Group() { }

        public Word this[string Word]
        {
            get
            {
                var selectedWord = from w in Words where w._Word.Trim() == Word.Trim() select w;
                return selectedWord.FirstOrDefault();
            }
        }

        public string Name { get; set; }

        public List<Word> Words { get; set; }

        public void Add(Group _Group)
        {
            MainWindow.Dictionary.Groups.Add(_Group);
        }
    }
}
