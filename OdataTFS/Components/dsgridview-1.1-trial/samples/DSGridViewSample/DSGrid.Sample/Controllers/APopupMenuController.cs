
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace DSGrid.Sample.Controllers
{
	public delegate void PopItemSelectedHandler(object sender, object item);
	
	public class APopupMenuController : UITableViewController
	{
		public event PopItemSelectedHandler OnItemSelected;
		
		private APopupMenuControllerSource mSource;
		
		public UIPopoverController PopUpController;
		
		public APopupMenuController () : base (UITableViewStyle.Grouped)
		{
		}
		
		public APopupMenuController (Dictionary<String,object> Items) : base (UITableViewStyle.Plain)
		{
			mSource = new APopupMenuControllerSource(Items, this);
		}
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Register the TableView's data source
			TableView.Source = mSource;

		}

		public void DidSelectItem(object item)
		{
			if (OnItemSelected != null)
			{
				
				OnItemSelected(this, item);
			}
		}
	}
}

