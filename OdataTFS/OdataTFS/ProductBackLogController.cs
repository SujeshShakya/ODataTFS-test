using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading;
using OdataTFS;
using RestSharp;
using System.Xml.Linq;
using MonoTouch.Dialog;
using AlertView;


namespace OdataTFS
{
	public class ProductBackLogController:UIViewController
	{
		UITableView tableView;
		UIImage _bckImg;
		string _uName,_password,mtitle="";
		List<WorkItem> lst;
		UIScrollView scrollView;
		//OdataTFSViewController viewController;
		WorkItemViewController _wrkItemView;

	//	UIAlertView alert;
		public ProductBackLogController (string _uname,string password,string mtitle)
		{

			this._uName = _uname;
			this._password = password;
			this.mtitle = mtitle;
		}
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{

			View.Frame = UIScreen.MainScreen.Bounds;
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.Black;
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			base.ViewDidLoad();


			// Perform any additional setup after loading the view
		}


		public UIViewController GetProductBackLogItem(string ProjectName,string url)
		{
			try
			{
			_wrkItemView = new WorkItemViewController (_uName, _password,mtitle);
			List<WorkItem> workItem = _wrkItemView.GetWorkItemList(ProjectName,url);

				var query = from s in workItem
				where s.Type == "Product Backlog Item"  
					select s;
				lst = query.ToList ();
				tableView = new UITableView (new RectangleF(0,0,View.Frame.Width+View.Frame.Width/2,View.Frame.Height), UITableViewStyle.Plain);
				tableView.Source = new TableSource (lst, this,ProjectName,url);
				scrollView = new UIScrollView (new RectangleF (0, 0,tableView.Frame.Width,tableView.Frame.Height));
				scrollView.AlwaysBounceHorizontal = true;
				scrollView.ShowsHorizontalScrollIndicator = true;
				scrollView.ShowsVerticalScrollIndicator = false;
				scrollView.AlwaysBounceVertical = false;
				scrollView.SizeToFit ();
				scrollView.ContentSize = new System.Drawing.SizeF (tableView.Frame.Width*2, tableView.Frame.Height);
				scrollView.AddSubview (tableView);
				View.AddSubview (scrollView);


				//Add(tableView);
				//View.AddSubview(tableView);
				return this;
			}
			catch(Exception ex) {
				return null;
			}

		}

		public class TableSource : UITableViewSource
		{
			protected List<WorkItem> _tableItems;
			protected string _cellIdentifier = "TableCell";
			ProductBackLogController vc;
			UIBarButtonItem _btnCancel;
			WorkItem[] lstSprint = null;
			string projectName="";
		    BoardViewController _brdController;
			UIBarButtonItem _btnDetails;
			UINavigationController _nav;
			string url="";
			ProductBackLogDetail _popOverController;
			public TableSource(List<WorkItem> product,ProductBackLogController vcon,string ProjectName,string url)
			{
				this.lstSprint = product.ToArray();
				this.vc=vcon;
				this.projectName=ProjectName;
				this.url=url;
			}



			public override int RowsInSection(UITableView tableview, int section)
			{
				if (lstSprint != null)
				{
					return this.lstSprint.Length;
				}
				else
				{
					return 0;
				}
			}
			public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				return 35;
			}
			public override UIView GetViewForHeader(UITableView tableView, int section)
			{

				return BuildSectionHeaderView("ProductBackLog Items",tableView.Frame.Width);
			}

			public static UIView BuildSectionHeaderView(string caption,float width)
			{
				UIView view = new UIView(new System.Drawing.RectangleF(0, 0, 320, 20));
				UILabel label = new UILabel();
				//UIImage _headImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
				label.BackgroundColor = UIColor.LightGray;
				label.Opaque = false;
				label.TextColor = UIColor.Black;
				label.Font = UIFont.FromName("Helvetica-Bold", 14f);
				label.Frame = new System.Drawing.RectangleF(0, 0, width, 20);
				label.Text = caption;
				view.AddSubview(label);
				return view;
			}
			public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
			{
				if (indexPath.Row % 2 == 0) {

					cell.BackgroundColor = UIColor.FromRGB (245, 245, 245);
				} else {
					cell.BackgroundColor = UIColor.White;
				}
			}
			public override string TitleForHeader(UITableView tableView, int section)
			{
				return "ProductBackLog Items";
			}


			public static void ClearEmptyTableViewSpacer(UITableView oTableView)
			{
				UIView oViewSpacerClear = new UIView(new RectangleF(0, 0, oTableView.Frame.Width, 10));
				oViewSpacerClear.BackgroundColor = UIColor.Clear;
				oTableView.TableFooterView = oViewSpacerClear;

			}

			public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				{
					tableView.BackgroundView = null;
				}
				else
				{
					tableView.BackgroundColor = null;
				}
				//---- declare vars
				UITableViewCell cell = tableView.DequeueReusableCell(this._cellIdentifier);

				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Value1, this._cellIdentifier);

				}
			
				cell.ImageView.Image = UIImage.FromFile ("Images/Clipboard.png");
				cell.TextLabel.Text = lstSprint[indexPath.Row].Title;
				cell.DetailTextLabel.Text = lstSprint [indexPath.Row].State;
				NSIndexPath path = NSIndexPath.FromRowSection (0, 0);
				tableView.SelectRow (path, true, UITableViewScrollPosition.Bottom);
				ClearEmptyTableViewSpacer(tableView);
				return cell;
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{

			/*	_brdController = new BoardViewController (lstSprint[indexPath.Row]);
			     _nav = new UINavigationController (_brdController)
				{ 
					ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
					ModalPresentationStyle = UIModalPresentationStyle.FullScreen,

				};

				_btnCancel= new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {Back();});
				_brdController.NavigationItem.LeftBarButtonItem = _btnCancel;
				_btnCancel.TintColor=UIColor.Red;
				_btnDetails=new UIBarButtonItem("Details",UIBarButtonItemStyle.Bordered,delegate{Details(lstSprint[indexPath.Row]);});
				 vc.ToolbarItems = new UIBarButtonItem[] { _btnDetails};
				 
				_brdController.NavigationItem.RightBarButtonItems = vc.ToolbarItems;
				vc.View.Subviews[0].RemoveFromSuperview ();
				vc.PresentViewController(_nav, true,null);*/

			/*popOverController = new ProductBackLogDetail(lstSprint[indexPath.Row]);
				UINavigationController _nav = new UINavigationController (_popOverController)
				{ 
					ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
					ModalPresentationStyle = UIModalPresentationStyle.FormSheet,

				};
				_btnCancel = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {_popOverController.Close();});
				_popOverController.NavigationItem.LeftBarButtonItem = _btnCancel;
				_nav.NavigationBar.BarStyle = UIBarStyle.Black;
				this.vc.PresentModalViewController (_nav, true);*/
			}
			/*private void Details(WorkItem userStory)
			{
				_popOverController = new ProductBackLogDetail(userStory);
				UINavigationController _nav = new UINavigationController (_popOverController)
				{ 
					ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
					ModalPresentationStyle = UIModalPresentationStyle.FormSheet,

				};
				_btnCancel = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {_popOverController.Close();});
				_popOverController.NavigationItem.LeftBarButtonItem = _btnCancel;
				_nav.NavigationBar.BarStyle = UIBarStyle.Black;
				_brdController.PresentModalViewController(_nav, true);


}
			private void Back()
			{
				ThreadPool.QueueUserWorkItem((f) =>
				                             {
					InvokeOnMainThread(delegate
					                   {

						MBHUDView.HudWithBody (
							body: "Loading Data.....", 
							aType: MBAlertViewHUDType.ActivityIndicator, 
							delay: 1.0f, 
							showNow: true
							);

					});
					InvokeOnMainThread(delegate
					                   {
						vc.GetProductBackLogItem(projectName,url);
						this.vc.DismissViewController (true,null);

					});


				});
			}*/

		}


	}

	public class ProductBackLogDetail:UIViewController
	{
		RootElement rootElement;
	
		StringElement Titles,Type,Detail,Area,Iteration,State,Reason,AssignedTo;
		WorkItem _wrkItem;

		public ProductBackLogDetail(WorkItem wrkItem)
		{
			this._wrkItem = wrkItem;
		
		}
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}
			public void Close()
			{
				DismissViewController(true,null);
			}


		public override void ViewDidLoad()
		{
			this.Title = "Product Backlog Detail";

			rootElement = new RootElement("Product BackLog Detail") {

				new Section () {
					(Type= new StringElement ("Type",_wrkItem.Type )),
					(Titles=new StringElement("Title",_wrkItem.Title)),
					(Detail= new StringElement ("Detail",_wrkItem.Description)),
					(Area=new StringElement("Area",_wrkItem.AreaPath)),

					(Iteration= new StringElement ("Iteration",_wrkItem.IterationPath)),
					(State=new StringElement("State",_wrkItem.State)),

					(Reason= new StringElement ("Reason",_wrkItem.Reason)),
					(AssignedTo=new StringElement("Assigned To",_wrkItem.AssignedTo)),


				}


			};
			var _dvController = new DialogViewController(rootElement);
			_dvController.TableView.BackgroundView = null;  
			var _bckImg = UIImage.FromFile ("Images/images.jpeg");
			_dvController.View.BackgroundColor = UIColor.FromPatternImage(_bckImg);
			View.AddSubview (_dvController.View);
			base.ViewDidLoad();

			// Perform any additional setup after loading the view
		}
		private void FullDescription(string element)
		{

			UILabel lbl = new UILabel (new RectangleF(20,40,1000,300));
			UIViewController popoverContent = new UIViewController();
			UIView popoverView = new UIView();
			popoverView.BackgroundColor = UIColor.Black;
			lbl.Text = element;
			popoverView.AddSubview(lbl);
			popoverContent.View = popoverView;
			UIPopoverController popovercontroller = new UIPopoverController(popoverContent);
			popovercontroller.PopoverContentSize = new SizeF(320,420);
			popovercontroller.PresentFromRect(new RectangleF(0,10, View.Bounds.Width/3, View.Bounds.Height/3),View, UIPopoverArrowDirection.Any, true);

		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

	}

	
}

