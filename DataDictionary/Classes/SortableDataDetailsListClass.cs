// http://stackoverflow.com/questions/8011481/how-to-sort-a-column-in-datagridview-that-is-bound-to-a-list-in-winform
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataDictionary.Classes
{
    class SortableDataDetailsListClass : IComparer<DataDetailsKeyCriteriaClass>
    {
        private readonly string _memberName = string.Empty; // the member name to be sorted
        private readonly SortOrder _sortOrder = SortOrder.None;

        public SortableDataDetailsListClass(string memberName, SortOrder sortingOrder)
        {
            _memberName = memberName;
            _sortOrder = sortingOrder;
        }

        public int Compare(DataDetailsKeyCriteriaClass Details1, DataDetailsKeyCriteriaClass Details2)
        {
            if (_sortOrder != SortOrder.Ascending)
            {
                var tmp = Details1;
                Details1 = Details2;
                Details2 = tmp;
            }

            switch (_memberName)
            {
                case "ColumnName":
                    return Details1.ColumnName.CompareTo(Details2.ColumnName);
                case "ColumnSize":
                    return Details1.ColumnSize.CompareTo(Details2.ColumnSize);
                case "DataType":
                    return Details1.DataType.CompareTo(Details2.DataType);
                case "SqlDataType":
                    return Details1.SqlDataType.CompareTo(Details2.SqlDataType);
                case "Mandatory":
                    return Details1.Mandatory.CompareTo(Details2.Mandatory);
                case "PrimaryKey":
                    return Details1.PrimaryKey.CompareTo(Details2.PrimaryKey);
                case "UniqueKey":
                    return Details1.UniqueKey.CompareTo(Details2.UniqueKey);
                case "ForeignKey":
                    return Details1.ForeignKey.CompareTo(Details2.ForeignKey);
                case "Description":
                    if (Details1.Description == null || Details2.Description == null) return -1;
                    return Details1.Description.CompareTo(Details2.Description);
                case "Example":
                    if (Details1.Example == null || Details2.Example == null) return -1;
                    return Details1.Example.CompareTo(Details2.Example);
                case "RangeFrom":
                    if (Details1.RangeFrom == null || Details2.RangeFrom == null) return -1;
                    return Details1.RangeFrom.CompareTo(Details2.RangeFrom);
                case "RangeTo":
                    if (Details1.RangeTo == null || Details2.RangeTo == null) return -1;
                    return Details1.RangeTo.CompareTo(Details2.RangeTo);
                case "Notes":
                    if (Details1.Notes == null || Details2.Notes == null) return -1;
                    return Details1.Notes.CompareTo(Details2.Notes);
                case "Computed":
                    if (Details1.Computed == null || Details2.Computed == null) return -1;
                    return Details1.Computed.CompareTo(Details2.Computed);
                default:
                    return -1;
            }
        }
    }
}