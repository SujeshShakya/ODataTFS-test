using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace OdataTFS
{
	public class AddProjectController:UIViewController
	{
		UIButton buttonRect;
		UITableView tableView;
		UINavigationController _navController;
	    public AddProjectController ()
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

			AddGestureRecognizer ();

			/*buttonRect = UIButton.FromType(UIButtonType.RoundedRect);
			buttonRect.SetTitle ("Cancel", UIControlState.Normal);
			buttonRect.Frame = new RectangleF (30, 30, 50, 50);
			buttonRect.TouchDown += Close;
			Add (buttonRect);
			View.AddSubview(buttonRect);*/

		
			base.ViewDidLoad();

			// Perform any additional setup after loading the view
		}
		public void AddGestureRecognizer()
		{
			UIPanGestureRecognizer panGesture;

			float dx = 0;
			float dy = 0;
			this.Title = "Add Project";
			View.Frame = UIScreen.MainScreen.Bounds;
			View.BackgroundColor = UIColor.Gray;
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			var imageView=new UIImageView(UIImage.FromFile("Images/Post-it-note-transparent.png"));
			imageView.Frame = new RectangleF (10, 10, 200, 200);
			imageView.UserInteractionEnabled = true;
			View.AddSubview (imageView);
			panGesture = new UIPanGestureRecognizer ((pg) => {
				if ((pg.State == UIGestureRecognizerState.Began || pg.State == UIGestureRecognizerState.Changed) && (pg.NumberOfTouches == 1)) {

					var p0 = pg.LocationInView (View);

					if (dx == 0)
						dx = p0.X - imageView.Center.X;

					if (dy == 0)
						dy = p0.Y - imageView.Center.Y;

					var p1 = new PointF (p0.X - dx, p0.Y - dy);

					imageView.Center = p1;
				} else if (pg.State == UIGestureRecognizerState.Ended) {
					dx = 0;
					dy = 0;
				}
			});


			imageView.AddGestureRecognizer (panGesture);
		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	
	}
}

