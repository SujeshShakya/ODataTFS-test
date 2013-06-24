using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.CoreFoundation;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.Threading;
using AlertView;
using DSoft.UI.Grid.Themes;
using DSoft.UI.Grid.Views;
using DSoft.Datatypes.Grid.Data;
using DSoft.UI.Grid;


namespace OdataTFS
{
	public class DetailViewController : UIViewController
	{
		UINavigationController _nav;
		public UIPopoverController Popover {get;set;}
		TeamMemberCollection TeamMember;
		SprintListController _spController;
		BoardViewController _brdController;
		ProductBackController _prBackController;
		SplitViewContoller _sp;
		string projectName,uname,password,url,mtitle="";
		UIPopoverController uipoc;
		UIBarButtonItem _btnCancel;//,_btnSprints,btnTm;
		UIAlertView alert;
		OdataTFSViewController viewcontroller;


		public DetailViewController (string proName, string username, string password,string url,string mtitle) : base()
		{

			this.projectName = proName;
			this.uname = username;
			this.password = password;
			this.url = url;
			this.mtitle = mtitle;
		}

		public override void ViewDidLoad()
		{
			//create the datatable object an

			View.BackgroundColor = UIColor.White;
			this.Title = projectName;
			Update ("Product Backlogs",projectName,uname,password,url); // defaults to "1"


		}
		public void Update (string row,string projectName, string _uname , string _password,string url) {


		
			if (row == "Product Backlogs") {
				if(View.Subviews.Length>0)
				{
					int length = View.Subviews.Length;
				   View.Subviews[length-1].RemoveFromSuperview ();
				}

				_prBackController = new ProductBackController (uname, password,mtitle);
				UIViewController prView = _prBackController.GetProductBackLogItem (projectName, url);
				if (prView != null) {
					View.AddSubview (prView.View);

				} 
				

			} 
			else if (row == "Members") {
				View.Subviews[0].RemoveFromSuperview ();
				TeamMember = new TeamMemberCollection (uname, password,mtitle);
				UIViewController tmView = TeamMember.GetDetailItem (projectName, url);
				View.AddSubview (tmView.View);

			}
			else if (row == "Sprints") {
				View.Subviews[0].RemoveFromSuperview ();
				UIViewController spView = GetSprintList ();
				View.AddSubview (spView.View);
			} 

			/*else if (row == "BoardView") {
			 * 
				_brdController = new BoardViewController (projectName);
				UINavigationController _nav = new UINavigationController (_brdController)
				{ 
					ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
					ModalPresentationStyle = UIModalPresentationStyle.FullScreen,

				};
				_btnCancel= new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {Back();});
				_brdController.NavigationItem.LeftBarButtonItem = _btnCancel;
				_btnSprints=new UIBarButtonItem("Sp",UIBarButtonItemStyle.Bordered,SprintList);
				btnTm=new UIBarButtonItem("Tm",UIBarButtonItemStyle.Bordered,TeamList);
				ToolbarItems = new UIBarButtonItem[] { _btnSprints,btnTm };
				_brdController.NavigationItem.RightBarButtonItems = ToolbarItems;
				_nav.NavigationBar.BarStyle = UIBarStyle.Black;
				 PresentModalViewController (_nav, true);
			}*/
		
		
			if (Popover != null) 
				Popover.Dismiss (true);
		}
		private UIViewController GetSprintList()
		{
			_spController = new SprintListController (uname, password,mtitle);
			UIViewController spView = _spController.GetSprintList (projectName, url); 
			return spView;
		}

/*		private void TeamList(object sender,EventArgs e)
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

					UIBarButtonItem _btn = (UIBarButtonItem)sender;
					TeamMember = new TeamMemberCollection (uname, password,mtitle);
					UIViewController tmView = TeamMember.GetDetailItem (projectName, url);
					_nav = new UINavigationController (tmView)
					{ 
						ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
						ModalPresentationStyle = UIModalPresentationStyle.FormSheet,

					};
					_btnCancel = new UIBarButtonItem("Close", UIBarButtonItemStyle.Bordered, delegate { TmClose(); });
					_btnCancel.TintColor=UIColor.Red;
					tmView.NavigationItem.LeftBarButtonItem = _btnCancel;
					_nav.NavigationBar.BarStyle = UIBarStyle.Black;
					_brdController.PresentViewController (_nav, true, null);
				});


			});

		
		}
		
		public void TmClose()
		{
			_nav.DismissViewController (true, null);
		}
		private void SprintList(object sender,EventArgs e)
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

					UIBarButtonItem _btn = (UIBarButtonItem)sender;
					UIViewController spView = GetSprintList ();
					uipoc = new UIPopoverController(spView);
					uipoc.PopoverContentSize = new SizeF(_brdController.View.Bounds.Width/3, _brdController.View.Bounds.Height/3);
					uipoc.PresentFromBarButtonItem (_btn, UIPopoverArrowDirection.Any, true);
					uipoc.DidDismiss += Dismiss;
				});


			});

		}
		private void Dismiss(object sender,EventArgs e)
		{
			UIPopoverController _pop = (UIPopoverController)sender;
			_pop.Dismiss (true);
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

						DismissViewController (true,null);
				});


			});
		}*/
		public void AddContentsButton (UIBarButtonItem button,UIViewController _nav)
		{
			button.Title = "Master View";
			_nav.NavigationController.NavigationBar.TopItem.SetLeftBarButtonItem (button, true);

		}

		public void RemoveContentsButton ()
		{
			_nav.NavigationController.NavigationBar.TopItem.SetLeftBarButtonItem (null, false);
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

}
}
