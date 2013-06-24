using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using DSoft.UI.Grid;
using DSGrid.Sample.Data;
using DSoft.UI.Grid.Themes;
using DSoft.UI.Grid.Views;

namespace DSGrid.Sample
{
	public partial class ADSGridViewController : DSGridViewController
	{
		public ADSGridViewController () : base()
		{
			this.Title = NSBundle.MainBundle.LocalizedString ("View Controller", "View Controller");
			this.TabBarItem.Image = UIImage.FromBundle ("first");
			
			ShowSelection = true;
			EnableBounce = true;
			
			DataSource = new ExampleDataTable("Items");
		}

		public override void OnDoubleCellTap (DSGridCellView sender)
		{
			Console.WriteLine (String.Format ("{0}:{1}", sender.Y.ToString (), sender.X.ToString ()));
		}
		
		public override void OnSingleCellTap (DSGridCellView sender)
		{
			Console.WriteLine (String.Format ("{0}:{1}", sender.Y.ToString (), sender.X.ToString ()));
		}
		
	}
}

