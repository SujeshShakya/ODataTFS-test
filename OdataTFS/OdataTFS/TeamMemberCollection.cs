using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading;
using RestSharp;
using System.Xml.Linq;

namespace OdataTFS
{
	public  class TeamMemberCollection:UIViewController
	{
		ProjectCollectionController viewController;
		string _uname,_password,mtitle;
		UIImage _bckImg;
		UITableView tableView;
		UIScrollView scrollView;
		//List<TeamMember> lstTable = new List<TeamMember>();
		public TeamMemberCollection (string _uname,string _password,string mtitle)
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
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.FromPatternImage (_bckImg);
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			base.ViewDidLoad();

			// Perform any additional setup after loading the view
		}
		public List<TeamMember> GetWorkItemList(string ProjectName, string url)
		{


		    viewController = new ProjectCollectionController (_uname, _password);
			var request = new RestRequest("/"+mtitle+"/Projects" + "('" + ProjectName + "')" + "/TeamMembers", Method.GET);//get xml

			var content = viewController.Execute(request, url);

			XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";//namespace for element
			XNamespace m = "http://www.w3.org/2005/Atom";//namespace for descendants
			XNamespace p = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";//namespace for properties element

			List<TeamMember> teamList = new List<TeamMember>();
			XDocument feedDocument = XDocument.Parse(content);
			var entryElements = feedDocument.Root.Descendants(m + "entry");
			foreach (var element in entryElements)
			{
				var contentElement = element.Descendants(m + "content").FirstOrDefault();
				var properties = contentElement.Element(p + "properties");
				#region Workitems

				TeamMember wi = new TeamMember();
			
				wi.MemberOf = properties.Element(d + "MemberOf").Value;
				wi.MemberName= properties.Element(d + "MemberName").Value;
				wi.ProjectName = properties.Element(d + "ProjectName").Value;


				teamList.Add(wi);
				#endregion
			}
			return teamList;

		}

		public UIViewController GetDetailItem(string ProjectName, string url)
		{

			//List<TeamMemberShow> lstTeamMemberCollection = new List<TeamMemberShow> ();
			tableView = new UITableView(View.Bounds);
		//	string[] wrkList = null;
			List<TeamMember> workItemList=GetWorkItemList (ProjectName, url);
			//wrkList = workItemList.Select (x=>x.MemberOf).Distinct ().ToArray ();
			TeamMemberShow tmColl = new TeamMemberShow ();
			List<TeamMember> single=workItemList.Where (x=>x.MemberOf=="[Mobile TFS]\\Project Valid Users").Select (x=>x).ToList();
				if (single != null) {
					tmColl.lstTeamMember = single;
					//lstTeamMemberCollection.Add (tmColl);
				}
			


			tableView = new UITableView(new RectangleF(0,0,View.Frame.Width,View.Frame.Height), UITableViewStyle.Plain);
			tableView.Source = new TableSource("[Mobile TFS]\\Project Valid Users",tmColl);
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
		public class TableSource : UITableViewSource
		{

			protected string _cellIdentifier = "TableCell";
			string[] lstTeam=null;
			TeamMemberShow teamMem = new TeamMemberShow ();
			string team = "";
			//UIActivityIndicatorView spinner;
			//TeamMemberCollection _tmCollection;
			public TableSource(string team,TeamMemberShow teamMember)
			{
				this.team = team;
				this.teamMem=teamMember;
			}
			public override int NumberOfSections(UITableView tableView)
			{
				return 1;
			}


			public override int RowsInSection(UITableView tableview, int section)
			{
				if (teamMem != null)
				{
					return this.teamMem.lstTeamMember.Count;
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

				return BuildSectionHeaderView(SplitString(team),tableView.Frame.Width);
			}

			public static UIView BuildSectionHeaderView(string caption,float tblview)
			{
				UIView view = new UIView(new System.Drawing.RectangleF(0, 0, 320, 25));
				UILabel label = new UILabel();
				//UIImage _headImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
				label.BackgroundColor = UIColor.LightGray;
				label.Opaque = false;
				label.TextColor = UIColor.Black;
				label.Font = UIFont.FromName("Helvetica-Bold", 14f);
				label.Frame = new System.Drawing.RectangleF(0, 0, tblview, 25);
				label.Text = caption;
				view.AddSubview(label);
				return view;
			}
			public override string TitleForHeader(UITableView tableView, int section)
			{
				return SplitString (team);
			} 
			private string SplitString(string team)
			{
				lstTeam = team.Split('\\');
				team=lstTeam.LastOrDefault ().ToString ();
				return team;
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
				cell.TextLabel.Text = teamMem.lstTeamMember[indexPath.Row].MemberName.ToString ();
				cell.TextLabel.Lines = 0;
				cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

				return cell;
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				//present model view controller
			}
			public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
			{
				if (indexPath.Row % 2 == 0) {
					cell.BackgroundColor = UIColor.FromRGB (245, 245, 245);

				} else {
					cell.BackgroundColor = UIColor.White;
				}
			}

		}
	}
}

