using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Your_vocabulary_2._0
{
    /// <summary>
    /// Словарь
    /// </summary>
    public class Dictionary
    {
        /// <summary>
        /// Создание словаря
        /// </summary>
        public Dictionary()
        {
            Groups = new List<Group>();
        }

        public Group this[string groupName]
        {
            get 
            {
                var selectedGroup = from g in Groups where g.Name == groupName select g;
                return selectedGroup.FirstOrDefault();
            }
        }

        /// <summary>
        /// Группы
        /// </summary>
        public List<Group> Groups { get; set; }
    }
}
