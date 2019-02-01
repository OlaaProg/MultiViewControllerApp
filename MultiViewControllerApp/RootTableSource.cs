using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace MultiViewControllerApp
{
    class RootTableSource : UITableViewSource
    {
        Contact[] tableItems;
        string cellIdentifier = "contactCellId";

        public RootTableSource(Contact[] items)
        {           
            tableItems = items;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifier);
            cell.TextLabel.Text = tableItems[indexPath.Row].FirstName +", " + tableItems[indexPath.Row].SecondName;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Length;
        }

        public Contact GetItem(int id)
        {
            return tableItems[id];
        }
    }
}