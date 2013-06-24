using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using DSGrid.Sample.Controllers;
using DSGrid.Sample.Data;
using System.Collections.Generic;
using DSoft.UI.Grid.Themes;
using DSGrid.Sample.Themes;
using DSoft.UI.Grid.Views;
using DSoft.Datatypes.Grid.Data;
using DSoft.UI.Grid;


namespace DSGrid.Sample
{
	public partial class SecondViewController : UIViewController
	{
	 
		#region Constuctors
		public SecondViewController () : base ("SecondViewController", null)
		{
		
		}
		
		#endregion
		
		#region Overrides
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Add handlers for single and double tap on each cell
			grdView.OnSingleCellTap += (DSGridCellView sender) => 
			{
				Console.WriteLine(String.Format("Cell clicked at location: X:{0} Y:{1}", sender.X, sender.Y));
			};
			
			grdView.OnDoubleCellTap += (DSGridCellView sender) => 
			{
				Console.WriteLine(String.Format("Cell double clicked at location: X:{0} Y:{1}", sender.X, sender.Y));
			};
			
			//single row selected
			grdView.OnRowSelect += (DSGridRowView sender, DSDataRow Row) => 
			{
				Console.WriteLine(String.Format("Row selected at index: {0}", sender.RowIndex));
			};
			
			//row double tap
			grdView.OnRowDoubleTapped += (DSGridRowView sender, DSDataRow Row) => 
			{
				Console.WriteLine(String.Format("Row double clicked at index: {0}", sender.RowIndex));
			};
		
			
		}

		[Obsolete ("Deprecated in iOS6. Replace it with both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation")]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		#endregion
		
		#region XIB selectors
		
		/// <summary>
		/// Handle the button press of the tables
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void didClickShowTables (MonoTouch.Foundation.NSObject sender)
		{
			var alert = new UIActionSheet("Tables");
			
			var tables = new ExampleDataSet().TableDictionary;
			
			foreach (var anItem in tables)
			{
				alert.AddButton(anItem);
			}
			
			alert.Clicked += (object action, UIButtonEventArgs e2) => 
			{	
				grdView.TableName = tables[e2.ButtonIndex];
			};
			
			alert.ShowFrom((UIBarButtonItem)sender, true);
			
		}
		
		/// <summary>
		/// Handle the button press for showing the themes
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void didClickShowTheme (MonoTouch.Foundation.NSObject sender)
		{
		
			var alert = new UIActionSheet("Tables");
			
			alert.AddButton("Default");
			alert.AddButton("New");
			alert.AddButton("iTunes");
			
			alert.Clicked += (object action, UIButtonEventArgs e2) => 
			{	
				DSGridViewTheme newtheme = null;
				
				switch (e2.ButtonIndex)
				{
					case 0:
					{
						newtheme = new DSGridViewDefaultTheme();
					}
					break;
					case 1:
					{
						newtheme = new AGridTheme();
					}
					break;
					case 2:
					{
						newtheme = new AGridThemeItunes();
					}
					break;
				}
				
				if (newtheme != null)
				{
					//set the current theme
					DSGridViewTheme.CurrentTheme = newtheme;
					
					//reload the grid
					grdView.ReloadData();
				}
			};
			
			alert.ShowFrom((UIBarButtonItem)sender, true);
			
		}
		#endregion
	}
}

