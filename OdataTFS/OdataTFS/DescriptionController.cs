using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading;

namespace OdataTFS
{
    public class DescriptionController : UIViewController
    {
          
        #region Private Member Variables
         OdataTFSViewController viewController;
        UITableView tableView;
        List<WorkItem> lst;
		UIImage _bckImg;
#endregion

        #region Public Methods
        public DescriptionController()
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
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.FromPatternImage (_bckImg);
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

        public void GetDetail(List<WorkItem> wrkItem, string types)
        {
            tableView = new UITableView(View.Bounds);
            string[] lstSprint = null;
            List<string> finalLstSprint = new List<string>();

            List<WorkItem> lsttable = new List<WorkItem>();
            var query = from s in wrkItem
                        where s.Type ==types  
                        select s;
            lst = query.ToList();

            foreach (var item in lst)
            {

                if (item.IterationPath != null)
                {
                    lstSprint = item.IterationPath.Split('\\');
                    finalLstSprint.Add(lstSprint.LastOrDefault().ToString());
                }

                lsttable.Add(item);
            }

            tableView = new UITableView(this.View.Frame, UITableViewStyle.Grouped);
            tableView.Source = new TableSource(lsttable, this, finalLstSprint.Distinct().ToList(),wrkItem);
            Add(tableView);
            View.AddSubview(tableView);
        }

        public class TableSource : UITableViewSource
        {
            protected List<WorkItem> _tableItems;
            protected string _cellIdentifier = "TableCell";
            DescriptionView viewDescription;
            UIViewController vc;
            string[] lstSprint = null;
            UIActivityIndicatorView spinner;
            protected List<WorkItem> _totalList;
            public TableSource(List<WorkItem> tblItemGroups, UIViewController DesController, List<string> sprint,List<WorkItem>totalList)
            {
                this._tableItems = tblItemGroups;
                this.vc = DesController;
                this.lstSprint = sprint.ToArray();
                this._totalList = totalList;
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
                return 30;
            }
            public override UIView GetViewForHeader(UITableView tableView, int section)
            {

                return BuildSectionHeaderView("Sprint List");
            }

            public static UIView BuildSectionHeaderView(string caption)
            {
                UIView view = new UIView(new System.Drawing.RectangleF(0, 0, 320, 20));
                UILabel label = new UILabel();
                label.BackgroundColor = UIColor.Clear;
                label.Opaque = false;
                label.TextColor = UIColor.Black;
                label.Font = UIFont.FromName("Helvetica-Bold", 16f);
                label.Frame = new System.Drawing.RectangleF(15, 0, 290, 20);
                label.Text = caption;
                view.AddSubview(label);
                return view;
            }
            public override string TitleForHeader(UITableView tableView, int section)
            {
                return "Sprint List";
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
                cell.TextLabel.Text = lstSprint[indexPath.Row];
                cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;

                return cell;
            }
			public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
			{
				if (indexPath.Row % 2 == 0) {
					cell.BackgroundColor = UIColor.White;
				} else {
					cell.BackgroundColor = UIColor.LightGray;
				}
			}
            public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
            {
                ThreadPool.QueueUserWorkItem((f) =>
                {
                    InvokeOnMainThread(delegate
                    {
                        spinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
                        spinner.StartAnimating();
                        vc.NavigationItem.RightBarButtonItem = new UIBarButtonItem(spinner);


                    });
                    InvokeOnMainThread(delegate
                    {
                        viewDescription = new DescriptionView(lstSprint);
                        viewDescription.GetDetail(_tableItems, lstSprint[indexPath.Row],_totalList);
                        // chgSetViewController = new ChangeSetViewController(userName, password);//for changeset
                     
                        vc.NavigationController.PushViewController(viewDescription, true);
                    });

                    InvokeOnMainThread(delegate
                    {

                        spinner.StopAnimating();

                    });

                });




            }
        }
        #endregion
       
       
    }
}