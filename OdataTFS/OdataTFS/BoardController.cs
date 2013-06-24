using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading;
using OdataTFS;
using RestSharp;
using System.Xml.Linq;
using MonoTouch.Dialog;

namespace OdataTFS
{
	public class BoardController:UIViewController 
	{
		string projectName="";
		UIImageView imageView;
		UIScrollView _scrollView;
		UITableView _tabTODO,_tabInProgress,_tabDone=null;
		public BoardController (string proName)
		{
			projectName = proName;
		}
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Title = projectName;

			imageView=new UIImageView(UIImage.FromFile("Images/sticky-note (1).png"));
			imageView.Frame = new RectangleF (10, 10, 100, 100);
			imageView.UserInteractionEnabled = true;

			_scrollView=new UIScrollView(new RectangleF(0,0, View.Frame.Width*2,
			                                            650));
			_scrollView.PagingEnabled = true;
			_scrollView.Bounces = true;
			_scrollView.DelaysContentTouches = true;
			_scrollView.ShowsHorizontalScrollIndicator = true;

			_scrollView.ContentSize = new System.Drawing.SizeF (2*View.Frame.Width/3+200,650);
			_scrollView.ScrollRectToVisible (new RectangleF (0, 0,View.Frame.Width, View.Frame.Height), true);


		    _tabTODO = new UITableView(new RectangleF(0,0,View.Frame.Width/3+100,View.Frame.Height), UITableViewStyle.Grouped);
			_tabTODO.Source = new TableSource("TODO");
			//Add(_tabTODO);
			//View.AddSubview (_tabTODO);
			//_tabTODO.AddSubview (imageView);

			_tabInProgress = new UITableView(new RectangleF(View.Frame.Width/3+100,0,View.Frame.Width/3+100,View.Frame.Height), UITableViewStyle.Grouped);
			_tabInProgress.Source = new TableSource("InProgress");
			//Add(_tabInProgress);
			//View.AddSubview(_tabInProgress);

			_tabDone = new UITableView(new RectangleF(2*View.Frame.Width/3+200,0,View.Frame.Width/3+100,View.Frame.Height), UITableViewStyle.Grouped);
			_tabDone.Source = new TableSource("Done");
			//Add(_tabDone);
			//View.AddSubview(_tabDone);
			_scrollView.AddSubviews (_tabTODO, _tabInProgress, _tabDone);
			_scrollView.AddSubview (imageView);
			View.AddSubview (_scrollView);
			AddGestureRecognizer ();
		}
		public void AddGestureRecognizer()
		{
			UIPanGestureRecognizer panGesture;
			PointF p1;
			float dx = 0;
			float dy = 0;

			/*var imageView=new UIImageView(UIImage.FromFile("Images/Post-it-note-transparent.png"));
			imageView.Frame = new RectangleF (10, 10, 200, 200);
			imageView.UserInteractionEnabled = true;*/

			panGesture = new UIPanGestureRecognizer ((pg) => {
				if ((pg.State == UIGestureRecognizerState.Began || pg.State == UIGestureRecognizerState.Changed) && (pg.NumberOfTouches == 1)) {

					var p0 = pg.LocationInView (View);

					if (dx == 0)
						dx = p0.X - imageView.Center.X;

					if (dy == 0)
						dy = p0.Y - imageView.Center.Y;

					 p1 = new PointF (p0.X - dx, p0.Y - dy);

					imageView.Center = p1;
				} 
				else if (pg.State == UIGestureRecognizerState.Ended) 
				{
					dx =0 ;
					dy = 0;
				}
			});
			panGesture.Delegate = new GestureDelegate ();
		    imageView.AddGestureRecognizer (panGesture);
		}

		public class GestureDelegate : UIGestureRecognizerDelegate {
			public override bool ShouldBegin (UIGestureRecognizer recognizer)
			{
				return true;
			}
			public override bool ShouldReceiveTouch (UIGestureRecognizer recognizer, UITouch touch)
			{
				return true;
			}
			public override bool ShouldRecognizeSimultaneously (UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
			{
				return true;
			}
		}
		public class TableSource : UITableViewSource
		{

			protected string _cellIdentifier = "TableCell";
			string _title="";
			public TableSource(string title)
			{
				this._title=title;
			}



			public override int RowsInSection(UITableView tableview, int section)
			{
				return 0;
			}

			public override UIView GetViewForHeader(UITableView tableView, int section)
			{

				return BuildSectionHeaderView(_title,tableView.Frame.Width);
			}

			public static UIView BuildSectionHeaderView(string caption,float width)
			{
				UIView view = new UIView(new System.Drawing.RectangleF(0, 0, 320, 25));
				UILabel label = new UILabel();
				//UIImage _headImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
				label.BackgroundColor = UIColor.LightGray;
				label.Opaque = false;
				label.TextColor = UIColor.Black;
				label.Font = UIFont.FromName("Helvetica-Bold", 14f);
				label.Frame = new System.Drawing.RectangleF(0, 0, width-10, 20);
				label.Text = caption;
				view.AddSubview(label);
				return view;
			}
			public override string TitleForHeader(UITableView tableView, int section)
			{
				return _title;
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
				cell.TextLabel.Text = _title;

				return cell;
			}


		}
	}
}

