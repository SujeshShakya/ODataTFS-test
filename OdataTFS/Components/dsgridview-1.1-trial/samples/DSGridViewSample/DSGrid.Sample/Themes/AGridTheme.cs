using System;
using DSoft.UI.Grid.Themes;
using MonoTouch.UIKit;

namespace DSGrid.Sample.Themes
{
	/// <summary>
	/// A Theme based on the default theme
	/// </summary>
	public class AGridTheme : DSGridViewDefaultTheme
	{
		
		public override float HeaderHeight {
			get {
				return 75.0f;
			}
		}
		
		public override MonoTouch.UIKit.UIColor CellBackgroundColor {
			get {
				return UIColor.Black;
			}
		}
		
		public override MonoTouch.UIKit.UIColor CellBackgroundColor2 {
			get {
				return CellBackgroundColor;
			}
		}
		
		public override UIColor CellTextColor {
			get {
				return UIColor.White;
			}
		}
		
		public override UIColor CellTextColor2 {
			get {
				return CellTextColor;
			}
		}
		public override UIColor CellHighlightColor {
			get {
				return UIColor.Gray;
			}
		}
		
		public override UIColor CellTextHighlightColor {
			get {
				return UIColor.Red;
			}
		}
		
		public override string ToString ()
		{
			return "AGridTheme";
		}
	}
}

