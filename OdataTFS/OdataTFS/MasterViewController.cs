using System;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.CoreFoundation;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.Threading;
using System.Drawing;

namespace OdataTFS {
	public class MasterViewController : UITableViewController {
		TableSource tableSource;
		UIImage _bckImg;
		public event EventHandler<RowClickedEventArgs> RowClicked;
		public class RowClickedEventArgs : EventArgs
		{
			public string Item { get; set; }

			public RowClickedEventArgs(string item) : base()
			{ this.Item = item; }
		}

		public MasterViewController ()
		{
		}
		public override void ViewDidLoad()
		{

			View.Frame = UIScreen.MainScreen.Bounds;
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.LightGray;
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			List<string> lst = new List<string> ();
			lst.Add ("Product Backlogs");
			lst.Add ("Sprints");
			lst.Add ("Members");
			//lst.Add ("BoardView");
			tableSource = new TableSource (lst, this);

			// add the data source to the table
			this.TableView.Source = tableSource;

		}
		public class TableSource : UITableViewSource
		{

			protected string _cellIdentifier = "TableCell";

			List<string> lstMasterMenu = new List<string> ();
			MasterViewController vc;
			public TableSource(List<string> lstMaster,MasterViewController vint)
			{
				lstMasterMenu=lstMaster;
				vc=vint;
			}
		

			public override int NumberOfSections (UITableView tableView)
			{ 
				return 1; 
			}
			public override int RowsInSection(UITableView tableview, int section)
			{
				return lstMasterMenu.Count;
			}
			public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				return 40;
			}


			public static void ClearEmptyTableViewSpacer(UITableView oTableView)
			{
				UIView oViewSpacerClear = new UIView(new RectangleF(0, 0, oTableView.Frame.Width, 10));
				oViewSpacerClear.BackgroundColor = UIColor.Clear;
				oTableView.TableFooterView = oViewSpacerClear;

			}

		  /*  public override string TitleForHeader (UITableView tableView, int section)
			{
				return "Ma";
			}
			public override UIView GetViewForHeader (UITableView tableView, int section)
			{
				return null;
			}*/
			public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{

				//---- declare vars
				UITableViewCell cell = tableView.DequeueReusableCell(this._cellIdentifier);
				string Menu = lstMasterMenu[indexPath.Row];
				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, this._cellIdentifier);
				}


				ClearEmptyTableViewSpacer(tableView);
				cell.TextLabel.Text = Menu;
				if (Menu == "Product Backlogs") {
					cell.ImageView.Image = UIImage.FromFile ("Images/images (1).jpeg");

				}
				if (Menu == "Sprints") {
					cell.ImageView.Image = UIImage.FromFile ("Images/sprintList.png");
				}
				if (Menu == "Members") {
					cell.ImageView.Image = UIImage.FromFile ("Images/teamCollection.jpeg");
				}
				//if (Menu == "BoardView") {
				//	cell.ImageView.Image=UIImage.FromFile ("Images/boardimg.png");
				//}
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				cell.TextLabel.Lines = 0;
				cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

				return cell;
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{

				if (vc != null)
					vc.RowClicked (this, new RowClickedEventArgs(lstMasterMenu[indexPath.Row]));
			}


		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	
	}
}