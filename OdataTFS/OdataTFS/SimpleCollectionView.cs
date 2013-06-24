using System;
using System.Collections.Generic;
using System.Drawing;
using OdataTFS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;

namespace ODataTFS
{
	public class SimpleCollectionView : UICollectionViewController
	{
		static NSString proCellId = new NSString ("proCell");
		List<IProject> proList;
		UIImage _bckImg;
		public SimpleCollectionView(UICollectionViewLayout layout,List<IProject> lstProject) : base (layout)
		{
			proList = lstProject;
			CollectionView.ContentInset = new UIEdgeInsets (0, 0, 200, 200);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.FromPatternImage (_bckImg);
			CollectionView.RegisterClassForCell (typeof (proCell), proCellId);
		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			return proList.Count;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var animalCell = (proCell) collectionView.DequeueReusableCell (proCellId, indexPath);

			var animal = proList [indexPath.Row];
			animalCell.OK = animal._img;
			return animalCell;
		}
	}

	public class proCell : UICollectionViewCell
	{
	

		UIImageView imageView;

		[Export ("initWithFrame:")]
		public proCell (System.Drawing.RectangleF frame) : base (frame)
		{
		
			SelectedBackgroundView = new UIView{BackgroundColor = UIColor.Blue};
			SelectedBackgroundView.Layer.CornerRadius = 10f;
			ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
			ContentView.Layer.BorderWidth = 1.5f;
			ContentView.Layer.CornerRadius = 10f;
			ContentView.BackgroundColor = UIColor.White;
			ContentView.Frame = new System.Drawing.RectangleF(6,6,226,226);
			ContentView.Transform = CGAffineTransform.MakeScale (0.8f, 0.8f);
		    imageView = new UIImageView (UIImage.FromFile ("Images/images.jpg"));
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			imageView.Center = ContentView.Center;
			imageView.Transform = CGAffineTransform.MakeScale (0.7f, 0.7f);
			ContentView.AddSubview (imageView);

		

		}


		public UIImage OK {
			set {
				this.imageView.Image = value;
			}

		}


	}
}
