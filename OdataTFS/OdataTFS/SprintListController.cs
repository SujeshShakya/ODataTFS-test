using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading;
using AlertView;

namespace OdataTFS
{
	public  class SprintListController:UIViewController
	{
		UIScrollView scrollView;
		UITableView tableView;

		string _uname,_password,mtitle;
		WorkItemViewController wrkViewController;
		List<WorkItem> lst = new List<WorkItem> ();
		public SprintListController (string _uname,string _password,string mtitle)
		{
			this._uname = _uname;
			this._password = _password;
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
			View.BackgroundColor = UIColor.Black;
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			base.ViewDidLoad();

			// Perform any additional setup after loading the view
		}

		public UIViewController GetSprintList(string ProjectName,string url)
		{

			wrkViewController = new WorkItemViewController (_uname, _password,mtitle);
			List<WorkItem>workItem=wrkViewController.GetWorkItemList (ProjectName, url);
			string[] lstSprint = null;
			List<string> finalLstSprint = new List<string>();
			var query = from s in workItem
				where s.Type == "Product Backlog Item"  
					select s;
			lst = query.ToList ();
			foreach(var item in lst)
			{
				if (item.IterationPath.Contains("Sprint"))
				{
					lstSprint = item.IterationPath.Split('\\');
					finalLstSprint.Add(lstSprint.LastOrDefault().ToString());
				}

			}
			tableView = new UITableView(new RectangleF(0,0,View.Frame.Width,View.Frame.Height), UITableViewStyle.Plain);
			tableView.Source = new TableSource(lst,this,finalLstSprint.Distinct().ToList(),workItem);
			/*_toolBar = new UIToolbar (new RectangleF(0,0,tableView.Frame.Width,30));
			_btnDetails=new UIBarButtonItem("BoardView",UIBarButtonItemStyle.Bordered,delegate{Board(lst,finalLstSprint.LastOrDefault(),workItem,ProjectName,url);});
			_toolBar.SetItems(new [] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), _btnDetails }, false);
			//_toolBar.SetItems(new UIBarButtonItem[] { _btnDetails }, true);
			_toolBar.BarStyle = UIBarStyle.Black;*/
			scrollView = new UIScrollView (new RectangleF (0, 0,tableView.Frame.Width,tableView.Frame.Height));
			scrollView.AlwaysBounceHorizontal = true;
			scrollView.ShowsHorizontalScrollIndicator = true;
			scrollView.ShowsVerticalScrollIndicator = false;
			scrollView.AlwaysBounceVertical = false;
			scrollView.SizeToFit ();
			scrollView.ContentSize = new System.Drawing.SizeF (tableView.Frame.Width*2, tableView.Frame.Height);
			scrollView.AddSubview(tableView);
			View.AddSubview (scrollView);
			return this;
		}
	/*	private void Board(List<WorkItem> productWorkItem,string sprint,List<WorkItem> totalList,string projectName,string url)
		{
			viewDescription = new DescriptionView();
			List<WorkItem>lstTable=	viewDescription.GetDetail(productWorkItem, sprint,totalList);

			_brdController = new BoardViewController (lstTable[0]);
			_nav = new UINavigationController (_brdController)
			{ 
				ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
				ModalPresentationStyle = UIModalPresentationStyle.FullScreen,

			};

			_btnCancel= new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {BoardBack(projectName,url);});
			_brdController.NavigationItem.LeftBarButtonItem = _btnCancel;
			_btnCancel.TintColor=UIColor.Red;
			_btnDetails=new UIBarButtonItem("Details",UIBarButtonItemStyle.Bordered,delegate{Details(lstTable[0]);});
			 ToolbarItems = new UIBarButtonItem[] { _btnDetails};

			_brdController.NavigationItem.RightBarButtonItems = ToolbarItems;
			View.Subviews[0].RemoveFromSuperview ();
			PresentViewController(_nav, true,null);


		}
		private void Details(WorkItem userStory)
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
		private void BoardBack(string ProjectName,string url)
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
					GetSprintList(ProjectName,url);
					DismissViewController (true,null);

				});


			});
		}*/

		public class TableSource : UITableViewSource
		{
			DescriptionView viewDescription;
			protected List<WorkItem> _tableItems;
			//BoardViewController _brdController;
			protected string _cellIdentifier = "TableCell";
			UIViewController vcon;
			protected List<WorkItem> _totalList;
			string[] lstSprint = null;
			UINavigationController  _nav;
			int row=0;
			UIBarButtonItem _btnCancel,_btnDetails;
			WorkItem wrk;
			public TableSource(List<WorkItem> typeWorkList,UIViewController vc,List<string> sprint,List<WorkItem>totalList)
			{
					this.lstSprint = sprint.ToArray();
				    this._tableItems=typeWorkList;
				    this.vcon=vc;
				    this._totalList=totalList;

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

				return BuildSectionHeaderView("Project Sprint List",tableView.Frame.Width);
			}

			public static UIView BuildSectionHeaderView(string caption,float width)
			{
				UIView view = new UIView(new System.Drawing.RectangleF(0, 0, 320,25));
				UILabel label = new UILabel();
				//UIImage _headImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
				label.BackgroundColor = UIColor.LightGray;
				label.Opaque = false;
				label.TextColor = UIColor.Black;
				label.Font = UIFont.FromName("Helvetica-Bold", 14f);
				label.Frame = new System.Drawing.RectangleF(0, 0, width, 25);
				label.Text = caption;
				view.AddSubview(label);
				return view;
			}
			public override string TitleForHeader(UITableView tableView, int section)
			{
				return "Project Sprint List";
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
					cell = new UITableViewCell(UITableViewCellStyle.Default, this._cellIdentifier);
				}


				ClearEmptyTableViewSpacer(tableView);
			    row = lstSprint.Length;
				NSIndexPath path = NSIndexPath.FromRowSection (row-1, 0);
				tableView.SelectRow (path, true, UITableViewScrollPosition.Top);
				cell.TextLabel.Text = lstSprint[indexPath.Row];
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

				return cell;
			}
			public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
			{
				if (indexPath.Row % 2 == 0) {
					cell.BackgroundColor = UIColor.FromRGB (245, 245, 245);
				} else {
					cell.BackgroundColor = UIColor.White;

				}
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				
				viewDescription = new DescriptionView(lstSprint);
			List<WorkItem>lstTable=	viewDescription.GetDetail(_tableItems, lstSprint[indexPath.Row],_totalList);
				_nav = new UINavigationController (viewDescription)
				{ 
					ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
					ModalPresentationStyle = UIModalPresentationStyle.CurrentContext,

				};

				_btnCancel= new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {Back();});
				if(indexPath.Row==row-1)
				{
					wrk = new WorkItem ();
					foreach (var item in lstTable) 
					{
						if (item.lstTask.Count > 0) {
							wrk = item;
							_btnDetails = new UIBarButtonItem ("BoardView", UIBarButtonItemStyle.Bordered, delegate {
								viewDescription.Board (wrk, lstTable);
							});
						} else {
							wrk = lstTable [0];
							_btnDetails=new UIBarButtonItem("BoardView",UIBarButtonItemStyle.Bordered,delegate{viewDescription.Board(wrk,lstTable);});
						}
					}

			         viewDescription.NavigationItem.RightBarButtonItems=new UIBarButtonItem[]{_btnCancel,_btnDetails};

			
				
				}
				else
				{
					//_btnDetails=new UIBarButtonItem("BoardView",UIBarButtonItemStyle.Bordered,delegate{viewDescription.Board(lstTable[0],lstTable);});
					viewDescription.NavigationItem.RightBarButtonItems=new UIBarButtonItem[]{_btnCancel};

				}
				//viewDescription.NavigationItem.RightBarButtonItem = _btnDetails;
				_btnCancel.TintColor=UIColor.Black;
				this.vcon.PresentViewController(_nav, true,null);
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

						this.vcon.DismissViewController (true,null);

					});


				});
			}

		}



	}
}

