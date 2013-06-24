
using System;
using MonoTouch.UIKit;
using MonoTouch.CoreFoundation;
using System.Drawing;
using System.Threading;
using AlertView;

namespace OdataTFS
{
	public class SplitViewContoller : UISplitViewController
	{

		 MasterViewController masterView;
		 DetailViewController detailView;
		 LoadingOverlay lodOv;
		UINavigationController _nav,_navDetail,_nav1;
	
		public SplitViewContoller (string mTitle,string projectName, string username, string password,string url) : base()
		{
			// create our master and detail views
			masterView = new MasterViewController ();
			detailView = new DetailViewController (projectName,username,password,url,mTitle);

		   _nav=new UINavigationController(masterView);
			var _btn = new UIBarButtonItem ("Back", UIBarButtonItemStyle.Bordered, delegate {
				Back (username,password,url,mTitle);
			});

			masterView.NavigationItem.LeftBarButtonItem=_btn;
			_nav.NavigationBar.BarStyle = UIBarStyle.Black;
			_navDetail = new UINavigationController (detailView);
			var _btnLogOut = new UIBarButtonItem ("Log Out", UIBarButtonItemStyle.Bordered, delegate {
				Close ();
			});
			_btnLogOut.TintColor = UIColor.Red;
			detailView.NavigationItem.RightBarButtonItem=_btnLogOut;
			_navDetail.NavigationBar.BarStyle = UIBarStyle.Black;



			masterView.RowClicked += (object sender, MasterViewController.RowClickedEventArgs e) => 
			{
				
				ThreadPool.QueueUserWorkItem((f) =>
				                             {
					InvokeOnMainThread(delegate
					                   {
						/*lodOv = new LoadingOverlay(new RectangleF(320,0,View.Frame.Width,View.Frame.Height));
						View.AddSubview(lodOv);*/
						MBHUDView.HudWithBody (
							body: "Loading Data.....", 
							aType: MBAlertViewHUDType.ActivityIndicator, 
							delay: 4.0f, 
							showNow: true
							);

					});
					InvokeOnMainThread(delegate
					                   {
					    
						detailView.Update (e.Item, projectName , username , password,url);
					});

					/*InvokeOnMainThread(delegate
					                   {
						lodOv.Hide();

					});*/

				});



					
			};
			ViewControllers = new UIViewController[] 
			{ _nav, _navDetail };
			Delegate = new SplitViewDelegate();
	
		}

		private void Back(string username,string password,string url,string mTitle)
		{
			 
			ThreadPool.QueueUserWorkItem((f) =>
			                             {
				InvokeOnMainThread(delegate
				                   {
				/*	lodOv = new LoadingOverlay(new RectangleF(320,0,View.Frame.Width,View.Frame.Height));
					View.AddSubview(lodOv);*/
					MBHUDView.HudWithBody (
						body: "Loading Data.....", 
						aType: MBAlertViewHUDType.ActivityIndicator, 
						delay: 1.0f, 
						showNow: true
						);

				});
				InvokeOnMainThread(delegate
				                   {
					AppDelegate sd;
					sd = ((AppDelegate)UIApplication.SharedApplication.Delegate);
					sd.viewController=new OdataTFSViewController(username,password,mTitle);
					sd.viewController.GetConnection(url);

					_nav1=new UINavigationController(sd.viewController);
					UIBarButtonItem _btnCancel = new UIBarButtonItem("Login Credential", UIBarButtonItemStyle.Bordered, delegate { Close(); });
					sd.viewController.NavigationItem.LeftBarButtonItem = _btnCancel;

					_nav1.NavigationBar.BarStyle = UIBarStyle.Black;
					//this.NavigationController.PushViewController(_nav1,true);
					 sd.window.RootViewController = _nav1;
					//this.NavigationController.PopViewControllerAnimated(true);
				});

				/*InvokeOnMainThread(delegate
				                   {
					lodOv.Hide();

				});*/

			});
		}

		public void Close()
		{
			AppDelegate sd;
			sd = ((AppDelegate)UIApplication.SharedApplication.Delegate);
		    _nav = new UINavigationController ();
			_nav = sd.RootLoginMethod ();
			sd.window.RootViewController = sd._navController;
		}
	
		public class SplitViewDelegate : UISplitViewControllerDelegate {
			public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
			{

				return false;
			}

			public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
			{
				DetailViewController detailView = svc.ViewControllers[1] as DetailViewController;
				detailView.Popover = pc;
				detailView.AddContentsButton (barButtonItem,svc.ViewControllers[0]);
			}

			public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
			{
				DetailViewController detailView = svc.ViewControllers[1] as DetailViewController;
				detailView.Popover = null;
				detailView.RemoveContentsButton ();
			}
		}
	}	
}
