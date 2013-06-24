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
using MonoTouch.CoreGraphics;

namespace OdataTFS
{
	public class BoardViewController:UIViewController
	{
		UINavigationController _navTODO,_navInProgress,_navDone;
		SubViewController vc1;
		ContainerViewController vc2;
		ContainerView1Controller vc3;
		UIImageView imageView;
		WorkItem wrkItem = new WorkItem ();
		UIScrollView _scrollView;
		UIPickerView _pickerView;
		UIPopoverController uipoc;
		RectangleF _bounds;
		int height=40;
		RectangleF _originalImageFrame = new RectangleF ();
		List<UIImageView> lstImageView = new List<UIImageView> ();
		RectangleF lRect,rRect,mRect;
		UIImage _image;
		public BoardViewController(WorkItem wrkItem)
		{
			this.wrkItem = wrkItem;
		}
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override void ViewDidLoad ()
		{
			float h = 0;
			float w = 0;
			if (this.InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || this.InterfaceOrientation == UIInterfaceOrientation.LandscapeRight) 
			{
				h = 150.0f;
				w = 150.0f;
			}
			else
			{

				h = 130f;
				w = 130f;
			}

			float padding = 0.0f;
			//float m=0;
			float x=0;
			int l=0;
			float y=60;
			_bounds = new RectangleF (50, 100,View.Frame.Width*2,View.Frame.Height);
			this.Title = wrkItem.Title;
			base.ViewDidLoad ();
			_scrollView=new UIScrollView(new RectangleF(0,0, View.Frame.Width*3,
			                                            View.Frame.Height));
			_scrollView.PagingEnabled = true;
			_scrollView.Bounces = true;
			_scrollView.DelaysContentTouches = true;
			_scrollView.ShowsHorizontalScrollIndicator = true;

			_scrollView.ContentSize = new System.Drawing.SizeF (View.Frame.Width,View.Frame.Height);
			_scrollView.ScrollRectToVisible (new RectangleF (0,0,View.Frame.Width, View.Frame.Height), true);
			/*
			_scrollView.MaximumZoomScale = 2.0f; 
			_scrollView.MinimumZoomScale = 0.5f; 
			_scrollView.ZoomScale = 1.0f; 
			_scrollView.BouncesZoom = true; 
			_scrollView.ViewForZoomingInScrollView = delegate(UIScrollView scrollView) {
				return imageView;
			};      
*/
			vc1 = new SubViewController (UIColor.White);
			_navTODO = new UINavigationController (vc1);
			//_navTODO.NavigationBar.BarStyle = UIBarStyle.Black;
			this.AddChildViewController(_navTODO);

			SubViewController tmpvc1 = new SubViewController (UIColor.White);
			vc2 = new ContainerViewController (tmpvc1);
			_navInProgress = new UINavigationController (vc2);
			//_navInProgress.NavigationBar.BarStyle = UIBarStyle.Black;
			this.AddChildViewController (_navInProgress);


			SubViewController tmpvc2 = new SubViewController (UIColor.White);
			vc3 = new ContainerView1Controller (tmpvc2);
			_navDone = new UINavigationController (vc3);
			//_navDone.NavigationBar.BarStyle = UIBarStyle.Black;
			this.AddChildViewController (_navDone);
			//this.View.AddSubview (_nav.View);
			_scrollView.AddSubviews (_navTODO.View, _navInProgress.View, _navDone.View);
			/*imageView=new UIImageView(UIImage.FromFile("Images/sticky-note (1).png"));
			imageView.Frame = new RectangleF (0, 50, 150, 150);
			imageView.UserInteractionEnabled = true;*/

			/*UILabel lblUserStory = new UILabel (new RectangleF(20,50,100,10));
			lblUserStory.Text = wrkItem.Title;
			lblUserStory.TextAlignment = UITextAlignment.Left;
			lblUserStory.LineBreakMode = UILineBreakMode.WordWrap;
			lblUserStory.Font = UIFont.SystemFontOfSize(10f);
			lblUserStory.TextColor = UIColor.Black;
			lblUserStory.BackgroundColor = UIColor.Clear;
			imageView.AddSubview (lblUserStory);*/
			if(wrkItem.lstTask.Count>0)
			{
				for(int i=0;i<wrkItem.lstTask.Count;i++)
				{
					_image = UIImage.FromFile ("Images/sticky-note (1).png");
					imageView=new UIImageView(_image);
					//imageView.Frame = new RectangleF (X, Y, 150, 150);
					imageView.UserInteractionEnabled = true;
					imageView.Tag = i;
					//height = height +(10 + i);
					UILabel lblTask = new UILabel (new RectangleF(5,height,120,10));
					lblTask.Text = wrkItem.lstTask[i];
					lblTask.TextColor = UIColor.Black;
					lblTask.Font = UIFont.BoldSystemFontOfSize (8f);
					lblTask.TextColor = UIColor.Black;
					lblTask.BackgroundColor = UIColor.Clear;
					lblTask.TextAlignment = UITextAlignment.Center;
					lblTask.LineBreakMode = UILineBreakMode.WordWrap;
					lblTask.Lines = 0;
					lblTask.SizeToFit ();
					imageView.AddSubview (lblTask);
					UILabel lblAssignedTo = new UILabel (new RectangleF(5,height+40,120,10));
					lblAssignedTo.Text = wrkItem.AssignedTo;
					lblAssignedTo.TextColor = UIColor.Black;
					lblAssignedTo.Font = UIFont.BoldSystemFontOfSize (8f);
					lblAssignedTo.TextColor = UIColor.Black;
					lblAssignedTo.BackgroundColor = UIColor.Clear;
					lblAssignedTo.TextAlignment = UITextAlignment.Center;
					lblAssignedTo.LineBreakMode = UILineBreakMode.WordWrap;
					lblAssignedTo.Lines = 0;
					lblAssignedTo.SizeToFit ();
					imageView.AddSubview (lblAssignedTo);
					/*UIButton button = new UIButton (new RectangleF(20,height+20,40,10));
					button.UserInteractionEnabled = true;
					button.SetTitle ("40h", UIControlState.Normal);
					button.TitleLabel.LineBreakMode=UILineBreakMode.WordWrap;
					button.TitleLabel.TextAlignment = UITextAlignment.Center;
					button.SetTitleColor(UIColor.Black,UIControlState.Normal);
					button.TouchDown += ShowPicker;
					imageView.AddSubview (button);*/
					if(l<=1)
					{

						x=padding*(l+1)+(l*w);
						imageView.Frame = new RectangleF (x,y, w, h); 
						imageView.Tag=i;
						l++;
					}
					else
					{
						l=0;
						x=padding*(l+1)+(l*w);
						y=y+2*padding+h;
						imageView.Frame=new RectangleF(x,y,w,h);
						imageView.Tag=i;
						l++;
					}
					_scrollView.AddSubview (imageView);
					lstImageView.Add (imageView);
					AddGestureRecognizer (imageView);


				}


			}
		
			//_scrollView.AddSubview (imageView);
			View.AddSubview (_scrollView);


		}
		void ScaleImage (UIPinchGestureRecognizer gestureRecognizer)
		{
			 AdjustAnchorPointForGestureRecognizer (gestureRecognizer);
			if (gestureRecognizer.State == UIGestureRecognizerState.Began || gestureRecognizer.State == UIGestureRecognizerState.Changed) {
				gestureRecognizer.View.Transform *= CGAffineTransform.MakeScale (gestureRecognizer.Scale, gestureRecognizer.Scale);
				// Reset the gesture recognizer's scale - the next callback will get a delta from the current scale.
				gestureRecognizer.Scale = 1;
			}
		}
		void AdjustAnchorPointForGestureRecognizer (UIGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer.State == UIGestureRecognizerState.Began) {
				var image = gestureRecognizer.View;
				var locationInView = gestureRecognizer.LocationInView (image);
				var locationInSuperview = gestureRecognizer.LocationInView (image.Superview);

				image.Layer.AnchorPoint = new PointF (locationInView.X / image.Bounds.Size.Width, locationInView.Y / image.Bounds.Size.Height);
				image.Center = locationInSuperview;
			}
		}
	
			

		private void ShowPicker(object sender,EventArgs e)
		{
			UIButton lbl = (UIButton)sender;
			_pickerView = new UIPickerView ();
			_pickerView.Frame = new RectangleF (0,0,250,250); 
			_pickerView.ShowSelectionIndicator = true;
			_pickerView.Model = new StatusPickerViewModel ();
			UIViewController vc = new UIViewController ();
			vc.View.AddSubview (_pickerView);
			uipoc = new UIPopoverController(vc);
			uipoc.PopoverContentSize = new SizeF(_pickerView.Frame.Size);
			uipoc.PresentFromRect (imageView.Frame,this.View, UIPopoverArrowDirection.Any, true);


		}

		public class StatusPickerViewModel : UIPickerViewModel
		{
			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return 40;
			}

			public override string GetTitle (UIPickerView picker, int row, int component)
			{

				return row.ToString();
			}
		}
		public override void ViewWillLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

		     lRect = this.View.Bounds;
			 rRect = lRect;
			 mRect = lRect;

			lRect.X = lRect.Y = lRect.Y = 0;
			lRect.Width =lRect.Width/3;

			rRect.X = lRect.Width+1;
			rRect.Width = (rRect.Width/3)-1;

			mRect.X =rRect.X+rRect.Width+1;
			mRect.Width = (mRect.Width/3)-1;

			this.View.Subviews[0].Subviews[0].Frame = lRect;
			this.View.Subviews[0].Subviews[1].Frame = rRect;
			this.View.Subviews [0].Subviews[2].Frame = mRect;
		}

		public void AddGestureRecognizer(UIImageView imageView)
		{

			//For Pinch Gesture


			UIPanGestureRecognizer panGesture;
			PointF offset;
			RectangleF newFrame;
			RectangleF previousFrame;
			previousFrame = new RectangleF (imageView.Frame.X, imageView.Frame.Y, imageView.Frame.Width, imageView.Frame.Height);

			//needs to change here for swapping ImageView Fixing Boundry Locations....
			panGesture = new UIPanGestureRecognizer ((recognizer) => {

				if(recognizer.State == UIGestureRecognizerState.Began)
				{ 
					if(_bounds.Contains(recognizer.LocationInView(imageView.Superview)))
					{
						this._originalImageFrame = imageView.Frame; 
					}
				}
				if(recognizer.State != (UIGestureRecognizerState.Cancelled | UIGestureRecognizerState.Failed
				                        | UIGestureRecognizerState.Possible))
				{
					//---- move the shape by adding the offset to the object's frame 
					if(_bounds.Contains(recognizer.LocationInView(imageView.Superview)))
					{
					offset= recognizer.TranslationInView(imageView); 
					newFrame = this._originalImageFrame; 
					newFrame.Offset(offset.X, offset.Y);
					imageView.Frame = newFrame;
					
				}
				if(recognizer.State==UIGestureRecognizerState.Ended)
				{

					//Check if it doesnot crosses boundry...

					float newframeX=newFrame.X;

					float mainX=View.Frame.Width/3;
					float boundry01=mainX-imageView.Frame.Width;
					float boundry02=mainX-imageView.Frame.Width/2;
					float boundry03=2*mainX-imageView.Frame.Width;
					float boundry04=2*mainX-imageView.Frame.Width/2;

					if(newframeX>=boundry01 && newframeX<boundry02)
					{
						//original frame
						//imageView.Frame=_originalImageFrame;
						imageView.Frame=previousFrame;

					}
					else if(newframeX>boundry02 && newframeX<boundry03)
					{
						RectangleF newRect=new RectangleF(mainX,imageView.Frame.Y,imageView.Frame.Width,imageView.Frame.Height);
						imageView.Frame=newRect;
					}
					else if(newframeX>=boundry03 && newframeX<boundry04)
					{
						//original frame
						RectangleF newRect=new RectangleF(mainX,imageView.Frame.Y,imageView.Frame.Width,imageView.Frame.Height);
						imageView.Frame=newRect;
					}
					else if(newframeX>boundry04)
					{
						RectangleF newRect=new RectangleF(2*mainX,imageView.Frame.Y,imageView.Frame.Width,imageView.Frame.Height);
						imageView.Frame=newRect;
					}
					else
					{
						imageView.Frame=newFrame;
					}
						//For Swapping Purpose.....
						foreach(UIImageView imageview in lstImageView)
						{
							if(imageview.Frame.IntersectsWith(newFrame))
							{
								Swap(imageview,imageView);
							}

						}
					}
				

				}

			});


			panGesture.Delegate = new GestureDelegate ();
			imageView.AddGestureRecognizer (panGesture);
			var pinchGesture = new UIPinchGestureRecognizer (ScaleImage);
			pinchGesture.Delegate = new GestureDelegate ();
			imageView.AddGestureRecognizer (pinchGesture);


		}
		private void Swap(UIImageView oldView,UIImageView currentView)
		{
			UIImageView temp = oldView;
			oldView = currentView;
			currentView = temp;
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




	}

	public class ContainerViewController : UIViewController
	{
		UIViewController _ctrl; 
		public ContainerViewController (UIViewController ctrl)
		{
			_ctrl = ctrl;   
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "InProgress";
			AddChildViewController (_ctrl);
			View.AddSubview (_ctrl.View);
		}


		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();
			RectangleF tmp = this.View.Frame;
			tmp.X = tmp.Y = 0;
			_ctrl.View.Frame = tmp;
		}
	} 
	public class ContainerView1Controller : UIViewController
	{
		UIViewController _ctrl; 
		public ContainerView1Controller (UIViewController ctrl)
		{
			_ctrl = ctrl;   
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "Done";
			AddChildViewController (_ctrl);
			View.AddSubview (_ctrl.View);
		}


		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();
			RectangleF tmp = this.View.Frame;
			tmp.X = tmp.Y = 0;
			_ctrl.View.Frame = tmp;
		}
	} 
	public class SubViewController : UIViewController
	{

		UIColor _back;

		public SubViewController (UIColor back)
		{
			_back = back;

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.View.BackgroundColor = _back;
			this.Title = "ToDo";
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}