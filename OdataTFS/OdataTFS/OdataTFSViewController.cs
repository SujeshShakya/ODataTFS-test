using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using System.Net;
using System.IO;
using RestSharp;
using System.Xml;
using System.Xml.Linq;
using RestSharp.Deserializers;
using System.Text;
using System.Threading;
using MonoTouch.ObjCRuntime;
using ODataTFS;
using AlertView;

namespace OdataTFS
{
	public partial class OdataTFSViewController : UIViewController
	{

		#region Private Member Variables
		string mTitle="";
		ProjectCollectionController _projectColl;
		AddProjectController _popOverController;
		UIBarButtonItem _btnCancel;
		UIImageView _imgProject;
		List<UIButton> lstProject;
		UIScrollView _scrollView;
		UITableView tableView;
		//UIPopoverController uipoc;
		//RestClient client;
		string _uname;
		string _password;
		//UIAlertView alertView;
		UIImage _bckImg;
		string url="";
		float h = 180.0f;
		float w = 180.0f;
		float padding = 10.0f;
		//float m=0;
		float x=0;
		int l=0;
		float y=0;
		#endregion

		#region Static Method
		static bool UserInterfaceIdiomIsPhone
		{
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
		#endregion

		#region Public Method


		public OdataTFSViewController(string _uname, string _password,string mtitle)
			: base(UserInterfaceIdiomIsPhone ? "OdataTFSViewController_iPhone" : "OdataTFSViewController_iPad", null)
		{
			this._uname = _uname;
			this._password = _password;
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
			this.Title = "Projects";
			View.Frame = UIScreen.MainScreen.Bounds;
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.FromPatternImage (_bckImg);
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			base.ViewDidLoad();

		}

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			#region 
			// Return true for supported orientations

			if (UserInterfaceIdiomIsPhone)
			{
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			}
			else
			{
				return true;
			}

			#endregion



		}


		public bool GetConnection(string urls)
		{
			_projectColl = new ProjectCollectionController (_uname, _password);
			tableView = new UITableView(View.Bounds);
			var request = new RestRequest("/"+mTitle+"/Projects/", Method.GET);//get xml
			var content = _projectColl.Execute(request,urls);
			this.url = urls;

			try
			{
				XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";//namespace for element
				XNamespace m = "http://www.w3.org/2005/Atom";//namespace for descendants
				XNamespace p = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";//namespace for properties element



				XDocument feedDocument = XDocument.Parse(content);
				List<string> prlist = new List<string>();
				var entryElements = feedDocument.Root.Descendants(m + "entry");
				foreach (var element in entryElements)
				{
					var contentElement = element.Descendants(m + "content").FirstOrDefault();
					var properties = contentElement.Element(p + "properties");


					#region Projects
					Projects pr = new Projects();
					pr.Name = properties.Element(d + "Name").Value;
					pr.Collection = properties.Element(d + "Collection").Value;
					prlist.Add(pr.Name);

					#endregion
				}

				/*	tableView = new UITableView(this.View.Frame, UITableViewStyle.Grouped);
				tableView.Source = new TableSource(prlist,this,uname,password,url,mtitle);
				Add(tableView);
				View.AddSubview(tableView);*/


				//List Image Button Purpose
				int n=prlist.Count;
				lstProject=new List<UIButton>();

				_scrollView=new UIScrollView(new RectangleF(20,10, View.Frame.Width+170,
				                                            1000));
				/*_scrollView.PagingEnabled=true;
				_scrollView.AlwaysBounceHorizontal=true;
				_scrollView.ContentSize = new System.Drawing.SizeF ((w + padding) * n, View.Frame.Height);
				_scrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				_scrollView.ShowsHorizontalScrollIndicator = true;*/

				_scrollView.PagingEnabled = true;
				_scrollView.Bounces = true;
				_scrollView.DelaysContentTouches = true;
				_scrollView.ShowsHorizontalScrollIndicator = true;
				_scrollView.ShowsVerticalScrollIndicator=true;
				_scrollView.ContentSize = new System.Drawing.SizeF ((w+padding)*n+View.Frame.Width,950);
				_scrollView.ScrollRectToVisible (new RectangleF (0, 0,View.Frame.Width, View.Frame.Height), true);

				for (int i=0; i<n; i++) {
					var button=new UIButton();
					_imgProject=new UIImageView(UIImage.FromFile("Images/sticky-note (1).png"));
					button.SetBackgroundImage(_imgProject.Image,UIControlState.Normal);
					button.SetTitle (prlist[i].ToString(), UIControlState.Normal);
					button.TitleLabel.LineBreakMode=UILineBreakMode.WordWrap;
					button.TitleLabel.TextAlignment = UITextAlignment.Center;
					button.SetTitleColor(UIColor.Black,UIControlState.Normal);
					if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait
					    || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown)
					{
						if(l<=3)
						{

							x=padding*(l+1)+(l*w);
							button.Frame = new RectangleF (x,y, w, h); 
							button.Tag=i;
							l++;
						}
						else
						{
							l=0;
							x=padding*(l+1)+(l*w);
							y=y+2*padding+h;
							button.Frame=new RectangleF(x,y,w,h);
							button.Tag=i;
							l++;
						}
					}
					else
					{
						if(l<=4)
						{

							x=padding*(l+1)+(l*w);
							button.Frame = new RectangleF (x,y, w, h); 
							button.Tag=i;
							l++;
						}
						else
						{
							l=0;
							x=padding*(l+1)+(l*w);
							y=y+2*padding+h;
							button.Frame=new RectangleF(x,y,w,h);
							button.Tag=i;
							l++;
						}
					}


					button.TouchDown+= HandleData;
					_scrollView.AddSubview (button);
					//lstProject.Add (button);
				}
				/*	var addProject=new UIImageView(UIImage.FromFile("Images/Post-it-note-transparent.png"));
				var btnAdd=new UIButton();

				btnAdd.SetBackgroundImage(addProject.Image,UIControlState.Normal);
				btnAdd.SetTitle("Add Project",UIControlState.Normal);
				btnAdd.TitleLabel.LineBreakMode=UILineBreakMode.WordWrap;
				btnAdd.TitleLabel.TextAlignment = UITextAlignment.Center;
				btnAdd.SetTitleColor(UIColor.Black,UIControlState.Normal);
				btnAdd.Frame = new RectangleF (padding*(l+1)+(l*w),2*padding+h,w,h); 
				btnAdd.TouchDown+= AddProject;
				_scrollView.AddSubview(btnAdd);		*/
				View.AddSubview(_scrollView);
				return true;
			}
			catch (Exception ex)
			{
				return false;

			}

		}

		private void HandleData(object sender,EventArgs e)

		{

			ThreadPool.QueueUserWorkItem((f) =>
			                             {
				InvokeOnMainThread(delegate
				                   {
					/*spinner = new LoadingOverlay(new RectangleF(0,0,View.Frame.Width,View.Frame.Height));
					View.AddSubview(spinner);*/
					MBHUDView.HudWithBody (
						body: "Loading Data.....", 
						aType: MBAlertViewHUDType.ActivityIndicator, 
						delay: 1.0f, 
						showNow: true
						);

				});
				InvokeOnMainThread(delegate
				                   {
					if(UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Pad)
					{
						AppDelegate sd;
						UIButton btn = (UIButton)sender;
						string ptitle = btn.Title (UIControlState.Normal);
						sd = ((AppDelegate)UIApplication.SharedApplication.Delegate);
						sd._sp = new SplitViewContoller (mTitle,ptitle,_uname, _password,url);
						sd.window.RootViewController = sd._sp;

					}
					else
					{

					}
				});

				/*InvokeOnMainThread(delegate
				                   {
					spinner.Hide();

				});*/

			});

		}

		private void AddProject(object sender,EventArgs e)
		{
			/*UIButton btn = (UIButton)sender;
			string title = btn.Title (UIControlState.Normal);
		/*_popOverController = new AddProjectController ();
			uipoc = new UIPopoverController(_popOverController);
			uipoc.PopoverContentSize = new SizeF(View.Bounds.Width/2, View.Bounds.Height/2);
			uipoc.PresentFromRect (new RectangleF(0,10, View.Bounds.Width/2, View.Bounds.Height/2),View, UIPopoverArrowDirection.Left, true);
*/
			//PresentModalViewController (_popOverController, true);
			_popOverController = new AddProjectController();
			UINavigationController _nav = new UINavigationController (_popOverController)
 				{ 
				ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
				ModalPresentationStyle = UIModalPresentationStyle.FormSheet,
	
				};
			_btnCancel = new UIBarButtonItem("X", UIBarButtonItemStyle.Bordered, delegate { Close(); });
			_popOverController.NavigationItem.LeftBarButtonItem = _btnCancel;
			_nav.NavigationBar.BarStyle = UIBarStyle.Black;
			this.NavigationController.PresentViewController(_nav,true,null);

		}

		public void Close()
		{
			DismissViewController(true,null);
			//DismissModalViewControllerAnimated (true);
		}


        public class TableSource : UITableViewSource
        {
            protected OdataTFSViewController Container;
            string[] tableItems;
            UIViewController vc;
			//TeamMemberCollection _team;
            WorkItemViewController wrkItemViewController;
            //ChangeSetViewController chgSetViewController;//showing changeset after click of project
            string _Url = "";
            UIActivityIndicatorView spinner;
           // UIScrollView _scrollView;
			//SprintListController _sprintList;
            string userName, password,mtitle;
            string cellIdentifier = "TableCell";
          
            public TableSource(string[] items, UIViewController vcon, string _uname, string _password,string url,string mtitle)
            {
                tableItems = items;
                this.vc = vcon;
                this.userName = _uname;
                this.password = _password;
                this._Url = url;
				this.mtitle=mtitle;
            }
            public override int RowsInSection(UITableView tableview, int section)
            {
                if (tableItems != null)
                {
                    return tableItems.Length;
                }
                else
                {
                    return 0;
                }
            }
            public override UIView GetViewForHeader(UITableView tableView, int section)
            {

                return BuildSectionHeaderView("Project Collections");
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
               
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    tableView.BackgroundView = null;
                }
                else
                {
                    tableView.BackgroundColor = null;
                }

                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                }

                ClearEmptyTableViewSpacer(tableView);
                cell.TextLabel.Text = tableItems[indexPath.Row];
               
                cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton; // implement AccessoryButtonTapped
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;


                return cell;
            }
            public override string TitleForHeader(UITableView tableView, int section)
            {
                return "Project Collections";//Default header
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
						// chgSetViewController = new ChangeSetViewController(userName, password);//for changeset
						// chgSetViewController.GetChangeSet(_Url);

                        wrkItemViewController = new WorkItemViewController(userName, password,mtitle);
                       wrkItemViewController.GetDetailItem(tableItems[indexPath.Row],_Url);
                        vc.NavigationController.PushViewController(wrkItemViewController, true);

						//_team = new TeamMemberCollection(userName, password);
						//_team.GetDetailItem(tableItems[indexPath.Row],_Url);
						//vc.NavigationController.PushViewController(_team, true);
						//_sprintList = new SprintListController(userName, password);
						// _sprintList.GetSprintList(tableItems[indexPath.Row],_Url);
						//vc.NavigationController.PushViewController(_sprintList, true);
                    });

                    InvokeOnMainThread(delegate
                    {
                        spinner.StopAnimating();

                    });

                });
            

            }
            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 30;
            }
            public static void ClearEmptyTableViewSpacer(UITableView oTableView)
            {
                UIView oViewSpacerClear = new UIView(new RectangleF(0, 0, oTableView.Frame.Width, 10));
                oViewSpacerClear.BackgroundColor = UIColor.Clear;
                oTableView.TableFooterView = oViewSpacerClear;

            }

        }

       
        #endregion

    }
}
