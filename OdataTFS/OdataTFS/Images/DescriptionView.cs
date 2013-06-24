using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.ObjCRuntime;
using System;
using System.Threading;
using AlertView;


namespace OdataTFS
{
   
    public class DescriptionView : UIViewController
    {
        #region Private  Member Variables

        string[] sprintCol = null;
        string IterationPath = "";
       // List<string> lstsprint = new List<string>();
        List<WorkItem> lstTable = new List<WorkItem>();
       // List<WorkItem> _task=new List<WorkItem>();
        UITableView tableView;
		//UIImage _bckImg;
		BoardViewController _brdController;
		UINavigationController _nav;
		UIBarButtonItem _btnCancel,_btnDetails;
		ProductBackLogDetail _popOverController;
		ProductBackDetailController _prBackDetailController;
        #endregion

        #region Public Methods
        public DescriptionView()
        {

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
			//_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.Black;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            base.ViewDidLoad();
        }
		public void Board(WorkItem lstTable,List<WorkItem>lstTotalList)
		{

			_brdController = new BoardViewController (lstTable);
			_nav = new UINavigationController (_brdController)
			{ 
				ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
				ModalPresentationStyle = UIModalPresentationStyle.FullScreen,

			};

			_btnCancel= new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {BoardBack(lstTotalList);});
			_brdController.NavigationItem.LeftBarButtonItem = _btnCancel;
			_btnCancel.TintColor=UIColor.Red;
			_btnDetails=new UIBarButtonItem("Details",UIBarButtonItemStyle.Bordered,delegate{Details(lstTable);});
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
		private void BoardBack(List<WorkItem> lstTable)
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
					tableLoad(lstTable);
					DismissViewController (true,null);

				});


			});
		}
      
        public List<WorkItem> GetDetail(List<WorkItem> tblItemGrp, string sprint,List<WorkItem> _totalList)
        {
            
            foreach (var item in tblItemGrp)
            {
                item.lstTask = new List<string>();

                foreach (var tsk in _totalList)
                {
                    if (item.Id == tsk.ParentId)
                    {
                        item.lstTask.Add(tsk.Title);
                    }

                }
                    sprintCol = item.IterationPath.Split('\\');
                    IterationPath = sprintCol.LastOrDefault().ToString();
                    if (IterationPath == sprint)
                    {

                        lstTable.Add(item);
                    }

                
            }
			tableLoad (lstTable);
         
           /* tableView = new UITableView(this.View.Frame, UITableViewStyle.Plain);
            tableView.Source = new TableSource(lstTable, sprint);
            Add(tableView);
            View.AddSubview(tableView);*/
			return lstTable;

        }
       private void tableLoad(List<WorkItem>lstTable)
		{
			_prBackDetailController = new ProductBackDetailController ();
			UIViewController prView = _prBackDetailController.GetProductBackLogDetail(lstTable);
			if (prView != null) {
				View.AddSubview (prView.View);

			} 

			/*tableView = new UITableView(this.View.Frame, UITableViewStyle.Plain);
			tableView.Source = new TableSource(lstTable,this);
			Add(tableView);
			View.AddSubview(tableView);*/
		}
        public class TableSource : UITableViewSource
        {
			string lstTask="";
            protected WorkItem[] _tableItems;
            protected string _cellIdentifier = "TableCell";
           // string vc="";
			WorkItem wrkItem;
            protected UIImageView img;  
			DescriptionView  viewController;
            public TableSource(List<WorkItem> tblItemGroups,DescriptionView vcon)
            {
                this._tableItems = tblItemGroups.ToArray();
				this.viewController=vcon;
               // this.vController = vCon;
                
            }
            public override int NumberOfSections(UITableView tableView)
            {
                return this._tableItems.Length;
            }
            public override int RowsInSection(UITableView tableview, int section)
            {
               
                if (_tableItems != null)
                {
                    return this._tableItems[section].lstTask.Count;
                }
                else
                {
                    return 0;
                }
           }
            public override UIView GetViewForHeader(UITableView tableView, int section)
            {
               
                return BuildSectionHeaderView(_tableItems[section].Title.ToString(),tableView.Frame.Width);
            }
          
            public UIView BuildSectionHeaderView(string caption,float tblview)
            {
                UIView view = new UIView(new System.Drawing.RectangleF(0, 10, tblview, 35));
                /*
				img = new UIImageView(UIImage.FromFile("Images/UIButtonTypeContactAdd.jpg"));
                img.UserInteractionEnabled = true;
                img.Frame = new RectangleF(10, 10, 10, 10);
                img.MultipleTouchEnabled = true;    */
                UILabel label = new UILabel();
                label.BackgroundColor = UIColor.Clear;
                label.Opaque = false;
                label.TextColor = UIColor.Black;
                label.Font = UIFont.FromName("Helvetica-Bold", 14f);
                label.Frame = new System.Drawing.RectangleF(10, 10, tblview-10, 35);
                label.Text = caption;
                label.Lines = 0;
                label.LineBreakMode = UILineBreakMode.WordWrap;
                label.SizeToFit();
                         
                view.AddSubview(label);
                return view;
            }
           
            public override float GetHeightForHeader(UITableView tableView, int section)
            {
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				{
					return 35;
				}
				else
				{
					return 68;
				}
                
            }
            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 25f;
            }
            public override string TitleForHeader(UITableView tableView, int section)
            {

                return _tableItems[section].Title.ToString();
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
                wrkItem = _tableItems[indexPath.Section];
			   lstTask = wrkItem.lstTask[indexPath.Row];
             
                //---- declare vars
                UITableViewCell cell = tableView.DequeueReusableCell(this._cellIdentifier);
                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Subtitle, this._cellIdentifier);
                }

                cell.TextLabel.Text = lstTask;
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

                ClearEmptyTableViewSpacer(tableView);

               
                return cell;
            }
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (lstTask != "") {
					viewController.Board (_tableItems[indexPath.Section],_tableItems.ToList());
				}
			}
           
        }
    
        #endregion

   


    }
}