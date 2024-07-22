// http://stackoverflow.com/questions/8011481/how-to-sort-a-column-in-datagridview-that-is-bound-to-a-list-in-winform
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataDictionary.Classes
{
    class SortableFKListClass : IComparer<FKKeyCriteria>
    {
        private readonly string _memberName = string.Empty; // the member name to be sorted
        private readonly SortOrder _sortOrder = SortOrder.None;

        public SortableFKListClass(string memberName, SortOrder sortingOrder)
        {
            _memberName = memberName;
            _sortOrder = sortingOrder;
        }

        public int Compare(FKKeyCriteria Details1, FKKeyCriteria Details2)
        {
            if (_sortOrder != SortOrder.Ascending)
            {
                var tmp = Details1;
                Details1 = Details2;
                Details2 = tmp;
            }

            switch (_memberName)
            {
                case "ForeignKeyName":
                    if (Details1.ForeignKeyName == null || Details2.ForeignKeyName == null) return -1;
                    return Details1.ForeignKeyName.CompareTo(Details2.ForeignKeyName);
                case "PrimaryKeyTable":
                    if (Details1.PrimaryKeyTable == null || Details2.PrimaryKeyTable == null) return -1;
                    return Details1.PrimaryKeyTable.CompareTo(Details2.PrimaryKeyTable);
                case "NameInPrimaryKeyTable":
                    if (Details1.NameInPrimaryKeyTable == null || Details2.NameInPrimaryKeyTable == null) return -1;
                    return Details1.NameInPrimaryKeyTable.CompareTo(Details2.NameInPrimaryKeyTable);
                default:
                    return -1;
            }
        }
    }
}