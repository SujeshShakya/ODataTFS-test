using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RestSharp;
using System.Xml.Linq;
using System.Drawing;
using System.Threading;

namespace OdataTFS
{

    public class WorkItemViewController : UIViewController
    {
        #region Private Member Variables
        ProjectCollectionController viewController;
        string _uname;
        string _password;
		string mtitle="";
        UITableView tableView;
		UIImage _bckImg;
	
        #endregion

        #region Public Method

        public WorkItemViewController(string userName, string password,string mtitle)
        {

            _uname = userName;
            _password = password;
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
            // this.Title = "WorkItemCollection";
            View.Frame = UIScreen.MainScreen.Bounds;
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.FromPatternImage (_bckImg);            
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

		public List<WorkItem> GetWorkItemList(string ProjectName, string url)
		{

			
			viewController = new ProjectCollectionController(_uname, _password);
			var request = new RestRequest("/"+mtitle+"/Projects" + "('" + ProjectName + "')" + "/WorkItems", Method.GET);//get xml

			var content = viewController.Execute(request, url);
			if (content != "") 
			{
				XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";//namespace for element
				XNamespace m = "http://www.w3.org/2005/Atom";//namespace for descendants
				XNamespace p = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";//namespace for properties element

				List<WorkItem> workItemList = new List<WorkItem> ();
				XDocument feedDocument = XDocument.Parse (content);
				var entryElements = feedDocument.Root.Descendants (m + "entry");
				foreach (var element in entryElements) {
					var contentElement = element.Descendants (m + "content").FirstOrDefault ();
					var properties = contentElement.Element (p + "properties");
					#region Workitems

					WorkItem wi = new WorkItem ();
					wi.Id = int.Parse (properties.Element(d + "Id").Value);
					wi.AreaPath = properties.Element (d + "AreaPath").Value;
					wi.IterationPath = properties.Element (d + "IterationPath").Value;
					wi.Revision = int.Parse (properties.Element(d + "Revision").Value);
					wi.Priority = properties.Element (d + "Priority").Value;
					wi.Severity = properties.Element (d + "Severity").Value;
					wi.StackRank = properties.Element (d + "StackRank").Value;
					wi.Project = properties.Element (d + "Project").Value;
					wi.AssignedTo = properties.Element (d + "AssignedTo").Value;
					wi.CreatedDate = DateTime.Parse (properties.Element(d + "CreatedDate").Value);
					wi.CreatedBy = properties.Element (d + "CreatedBy").Value;
					wi.ChangedDate = DateTime.Parse (properties.Element(d + "ChangedDate").Value);
					wi.ChangedBy = properties.Element (d + "ChangedBy").Value;
					wi.ResolvedBy = properties.Element (d + "ResolvedBy").Value;
					wi.Title = properties.Element (d + "Title").Value;
					wi.State = properties.Element (d + "State").Value;
					wi.Type = properties.Element (d + "Type").Value;
					wi.Reason = properties.Element (d + "Reason").Value;
					wi.Description = properties.Element (d + "Description").Value;
					wi.ReproSteps = properties.Element (d + "ReproSteps").Value;
					wi.FoundInBuild = properties.Element (d + "FoundInBuild").Value;
					wi.IntegratedInBuild = properties.Element (d + "IntegratedInBuild").Value;
					wi.WebEditorUrl = properties.Element (d + "WebEditorUrl").Value;
					if (Convert.ToInt32 (properties.Element(d + "ParentId").Value) != 0) {
						wi.ParentId = Convert.ToInt32 (properties.Element(d + "ParentId").Value);
					}
					workItemList.Add (wi);
					#endregion
				}
				return workItemList;
			} 
			else 
			{

				return null;
			}

		}
        public void GetDetailItem(string ProjectName, string url)
        {
            
			tableView = new UITableView(View.Bounds);
			string[] wrkList = null;
		    List<WorkItem> workItemList=GetWorkItemList(ProjectName, url);
            wrkList = workItemList.Select(x => x.Type).Distinct().ToArray();
            tableView = new UITableView(this.View.Frame, UITableViewStyle.Grouped);
            tableView.Source = new TableSource(wrkList, workItemList.ToArray(), this, _uname, _password);
            Add(tableView);
            View.AddSubview(tableView);


        }

        public class TableSource : UITableViewSource
        {
            UIActivityIndicatorView spinner;

            WorkItem[] tableItems;
            string[] dis_Types;
            private string _cellId;
          
            UIViewController vc;
//List<string> Types = new List<string>();
            DescriptionController descriptionController = new DescriptionController();

            public TableSource(string[] types, WorkItem[] items, UIViewController typevc, string uname, string password)
            {

                tableItems = items;
                vc = typevc;
                dis_Types = types;
                _cellId = "Section1";
            }
            public override int RowsInSection(UITableView tableview, int section)
            {
                if (dis_Types!=null)
                {
                    return dis_Types.Length;
                }
                else
                {
                    return 0;
                }
            }
            public override UIView GetViewForHeader(UITableView tableView, int section)
            {

                return BuildSectionHeaderView("WorkItems Types");
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
            public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                string cellIdentifier = _cellId;
                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    tableView.BackgroundView = null;
                }
                else
                {
                    tableView.BackgroundColor = null;
                }

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                }


                ClearEmptyTableViewSpacer(tableView);
                cell.TextLabel.Text = dis_Types[indexPath.Row];
                cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton; // implement AccessoryButtonTapped
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

                return cell;
            }
            public static void ClearEmptyTableViewSpacer(UITableView oTableView)
            {
                UIView oViewSpacerClear = new UIView(new RectangleF(0, 0, oTableView.Frame.Width, 10));
                oViewSpacerClear.BackgroundColor = UIColor.Clear;
                oTableView.TableFooterView = oViewSpacerClear;

            }
            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 30;
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
                        descriptionController = new DescriptionController();
                       
                        //method to extract details according to type;
                        descriptionController.GetDetail(tableItems.ToList(), dis_Types[indexPath.Row]);
                        vc.NavigationController.PushViewController(descriptionController, true);
                    });

                    InvokeOnMainThread(delegate
                    {
                        spinner.StopAnimating();

                    });

                });


            }
            public override string TitleForHeader(UITableView tableView, int section)
            {

                return "WorkItems Types";//Default header
            }


        }
        #endregion
       
    }
}