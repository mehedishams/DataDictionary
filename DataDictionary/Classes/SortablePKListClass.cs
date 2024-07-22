// http://stackoverflow.com/questions/8011481/how-to-sort-a-column-in-datagridview-that-is-bound-to-a-list-in-winform
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataDictionary.Classes
{
    class SortablePKListClass : IComparer<PKKeyCriteria>
    {
        private readonly string _memberName = string.Empty; // the member name to be sorted
        private readonly SortOrder _sortOrder = SortOrder.None;

        public SortablePKListClass(string memberName, SortOrder sortingOrder)
        {
            _memberName = memberName;
            _sortOrder = sortingOrder;
        }

        public int Compare(PKKeyCriteria Details1, PKKeyCriteria Details2)
        {
            if (_sortOrder != SortOrder.Ascending)
            {
                var tmp = Details1;
                Details1 = Details2;
                Details2 = tmp;
            }

            switch (_memberName)
            {                
                case "PrimaryKeyName":
                    if (Details1.PrimaryKeyName == null || Details2.PrimaryKeyName == null) return -1;
                    return Details1.PrimaryKeyName.CompareTo(Details2.PrimaryKeyName);
                case "ForeignKeyTable":
                    if (Details1.ForeignKeyTable == null || Details2.ForeignKeyTable == null) return -1;
                    return Details1.ForeignKeyTable.CompareTo(Details2.ForeignKeyTable);
                case "NameInForeignKeyTable":
                    if (Details1.NameInForeignKeyTable == null || Details2.NameInForeignKeyTable == null) return -1;
                    return Details1.NameInForeignKeyTable.CompareTo(Details2.NameInForeignKeyTable);
                default:
                    return -1;
            }
        }
    }
}