
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace DSGrid.Sample.Controllers
{
	public class APopupMenuControllerCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("APopupMenuControllerCell");
		
		public APopupMenuControllerCell () : base (UITableViewCellStyle.Default, Key)
		{
			// TODO: add subviews to the ContentView, set various colors, etc.
			//TextLabel.Text = "TextLabel";
		}
	}
}

