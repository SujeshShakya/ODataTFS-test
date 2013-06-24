using System;
using DSoft.UI.Grid.Themes;
using MonoTouch.UIKit;

namespace DSGrid.Sample.Themes
{
	public class AGridThemeItunes : DSGridViewDefaultTheme
	{
	
		#region Header properties
		public override MonoTouch.UIKit.UIColor HeaderColor 
		{
			get 
			{
				return UIColor.FromPatternImage(new UIImage("header.png"));
			}
		}
		
		public override float HeaderHeight {
			get {
				return 22;
			}
		}
		
		public override UIColor HeaderTextColor {
			get {
				return UIColor.DarkGray;
			}
		}
		
		public override UIFont HeaderFont {
			get {
				return UIFont.BoldSystemFontOfSize(14.0f);
			}
		}
		
		#endregion
		
		#region Cell Properties
		public override float RowHeight {
			get {
				return 24.0f;
			}
		}
		
		public override UIFont CellFont {
			get {
				return UIFont.SystemFontOfSize(14.0f);
			}
		}
		public override DSoft.UI.Grid.Enums.BorderStyle CellBorderStyle {
			get {
				return DSoft.UI.Grid.Enums.BorderStyle.HorizontalOnly;
			}
		}
		
		#endregion
		public override string ToString ()
		{
			return "AGridThemeItunes";
		}
	}
}

