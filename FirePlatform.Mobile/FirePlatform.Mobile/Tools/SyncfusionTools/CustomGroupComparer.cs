using System;
using System.Collections.Generic;
using Syncfusion.DataSource;
using Syncfusion.DataSource.Extensions;

namespace FirePlatform.Mobile.Tools.SyncfusionTools
{
    public class CustomGroupComparer : IComparer<GroupResult>, ISortDirection
    {
        public CustomGroupComparer()
        {
            this.SortDirection = ListSortDirection.Ascending;
        }
        public ListSortDirection SortDirection
        {
            get;
            set;
        }
        public int Compare(GroupResult x, GroupResult y)
        {
            int groupX = x.Count;
            int groupY = y.Count;

            if (groupX.CompareTo(groupY) > 0)
                return SortDirection == ListSortDirection.Ascending ? 1 : -1;
            else if (groupX.CompareTo(groupY) == -1)
                return SortDirection == ListSortDirection.Ascending ? -1 : 1;
            else
                return 0;
        }
    }
}
