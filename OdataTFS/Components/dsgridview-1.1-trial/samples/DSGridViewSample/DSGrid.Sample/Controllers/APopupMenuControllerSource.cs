
using System;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace DSGrid.Sample.Controllers
{
	public class APopupMenuControllerSource : UITableViewSource
	{
		private Dictionary<String,object> mItems;
		private APopupMenuController mOwner;
		
		public APopupMenuControllerSource ()
		{
		}
		
		public APopupMenuControllerSource (Dictionary<String,object> Items, APopupMenuController Owner)
		{
			mItems = Items;
			mOwner = Owner;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			// TODO: return the actual number of items in the section
			return mItems.Keys.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (APopupMenuControllerCell.Key) as APopupMenuControllerCell;
			if (cell == null)
				cell = new APopupMenuControllerCell ();
			
			String aKey = mItems.Keys.ElementAt(indexPath.Row);
			cell.TextLabel.Text = mItems[aKey].ToString();
			
			return cell;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (mOwner.PopUpController != null)
			{
				mOwner.PopUpController.Dismiss(true);
			}			
			
			String aKey = mItems.Keys.ElementAt(indexPath.Row);
			
			mOwner.DidSelectItem(mItems[aKey]);
			
		}
	}
}

