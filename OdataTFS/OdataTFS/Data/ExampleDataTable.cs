using System;
using System.Collections.Generic;
using DSoft.Datatypes.Grid.Data;
using DSoft.Datatypes.Types;
using MonoTouch.UIKit;
using DSoft.UI.Grid.Formatters;

namespace DSGrid.Sample.Data
{
	/// <summary>
	/// Example data table.
	/// </summary>
	public class ExampleDataTable : DSDataTable
	{
		public ExampleDataTable ()
		{
		
		}
		
		public ExampleDataTable (String Name) : base(Name)
		{
			
			var uimageArray = new UIImage[] { UIImage.FromFile("first.png"), UIImage.FromFile("second.png") };
			
			
			var ColumnsDefs = new Dictionary<String,float>();
			ColumnsDefs.Add ("Image", 30);
			ColumnsDefs.Add("ID",100);
			ColumnsDefs.Add("Date",124);
			ColumnsDefs.Add("Title",150);
			ColumnsDefs.Add("Description",550);
			ColumnsDefs.Add("Value",100);
			
			
		
			foreach (var aKey in ColumnsDefs.Keys)
			{
				// Create a column
	            var dc1 = new DSDataColumn(aKey);
	            dc1.Caption = aKey;
	            dc1.ReadOnly = true;
	            
	            if (!aKey.Equals("Image"))
	            {
	            	dc1.DataType = typeof(String);
	            	dc1.AllowSort = true;
	            }
	            else
	            {
	            	dc1.DataType = typeof(UIImage);
	            	dc1.AllowSort = false;
					dc1.Formatter = new DSImageFormatter (new DSSize(ColumnsDefs[aKey],ColumnsDefs[aKey]));
	            }

				dc1.Width = ColumnsDefs[aKey];
				
	            this.Columns.Add(dc1);
			}
            
            for (int loop = 0;loop < 20;loop++)
            {
            	var dr = new DSDataRow();
            	dr["ID"] = loop;
            	dr["Title"] = @"Test";
            	dr["Description"] = @"Some description would go here";
            	dr["Date"] = DateTime.Now.ToShortDateString();
            	dr["Value"] = "10000.00";
            	
            	//see if even or odd to pick an image from the array
            	var pos = loop % 2;
				dr ["Image"] = uimageArray [pos];
            	
            	this.Rows.Add(dr);
            }
            

                
		}
	}
}

