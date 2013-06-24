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
   
    public class ChangeSetViewController : UIViewController
    {
        #region Private Member Variables
        string _uname;
        string _password,mTitle;
		ProjectCollectionController viewController;
        UITableView tableView;
        #endregion

        #region Public Methods
        public ChangeSetViewController(string userName, string password,string mtitle)
        {

            this._uname = userName;
            this._password = password;
			this.mTitle = mtitle;
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {

            this.Title = "ChangeSetCollection";
            View.Frame = UIScreen.MainScreen.Bounds;
            View.BackgroundColor = UIColor.DarkGray;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

        public void GetChangeSet(string Url)
        {
            tableView = new UITableView(View.Bounds);
            string[] wrkList = null;
            viewController = new ProjectCollectionController(_uname, _password);
            var request = new RestRequest("/DefaultCollection/Changesets", Method.GET);//get xml

            var content = viewController.Execute(request, Url);

            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";//namespace for element
            XNamespace m = "http://www.w3.org/2005/Atom";//namespace for descendants
            XNamespace p = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";//namespace for properties element

            List<ChangeSets> ChangeSetList = new List<ChangeSets>();
            XDocument feedDocument = XDocument.Parse(content);
            var entryElements = feedDocument.Root.Descendants(m + "entry");
            foreach (var element in entryElements)
            {
                var contentElement = element.Descendants(m + "content").FirstOrDefault();
                var properties = contentElement.Element(p + "properties");
                #region ChangeSets

                ChangeSets wi = new ChangeSets();
              
                wi.Id = Convert.ToInt32(properties.Element(d + "Id").Value);
                wi.CreationDate = DateTime.Parse(properties.Element(d + "ChangedBy").Value);
                wi.Comment = properties.Element(d + "Comment").Value;               
                wi.Branch = properties.Element(d + "Branch").Value;
                wi.ArtifactUri = properties.Element(d + "ArtifactUri").Value;

                ChangeSetList.Add(wi);

                #endregion
                wrkList = ChangeSetList.Select(x => x.ArtifactUri).Distinct().ToArray();

            }

            tableView.Source = new TableSource(wrkList, ChangeSetList.ToArray(), this);
            Add(tableView);
            View.AddSubview(tableView);


        }

        public class TableSource : UITableViewSource
        {
            LoadingOverlay lodOv;
            ChangeSets[] tableItems;
            string[] dis_Types=null;
            private string _cellId;
            UIViewController vc;
      
            DescriptionController descriptionController = new DescriptionController();

            public TableSource(string[] types, ChangeSets[] items, UIViewController typevc)
            {

                tableItems = items;
                vc = typevc;
                this.dis_Types = types;
                _cellId = "Section1";
            }
            public override int RowsInSection(UITableView tableview, int section)
            {
                return tableItems.Length;
            }

            public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {

                string cellIdentifier = _cellId;
                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                }
                ClearEmptyTableViewSpacer(tableView);

                cell.TextLabel.Text = tableItems[indexPath.Row].ArtifactUri + "                 " + tableItems[indexPath.Row].CreationDate.ToShortDateString() + "           " + tableItems[indexPath.Row].Comment.ToString();

                cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton; // implement AccessoryButtonTapped
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

                return cell;
            }
            public override UIView GetViewForHeader(UITableView tableView, int section)
            {

                return BuildSectionHeaderView("ChangeSet Types");
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
						lodOv = new LoadingOverlay(new RectangleF(0,0,tableView.Frame.Width,tableView.Frame.Height));
                        tableView.AddSubview(lodOv);


                    });
                    InvokeOnMainThread(delegate
                    {
                        descriptionController = new DescriptionController();

                        //method to extract details according to type;
                        //  descriptionController.GetDetail(tableItems.ToList(),dis_Types[indexPath.Row]);
                        vc.NavigationController.PushViewController(descriptionController, true);
                    });

                    InvokeOnMainThread(delegate
                    {
                        lodOv.Hide();

                    });

                });


            }
            public override string TitleForHeader(UITableView tableView, int section)
            {

                return "ChangeSet Types";//Default header
            }


        }
        #endregion
       
    }
}