A DSGridView can be added to a view or view controller in a few simple steps

**Create DataSource**

First create the data source object to use with the data grid


	using DSoft.Datatypes.Grid.Data;
	
	public override void ViewDidLoad ()
	{
		//create the datatable object and set a name
		var aDataSource = new DSDataTable("ADT");
		//add a column
		var dc1 = new DSDataColumn("Title");
		dc1.Caption = "Title";
		dc1.ReadOnly = true;
		dc1.DataType = typeof(String);
		dc1.AllowSort = true;
		dc1.Width = ColumnsDefs[aKey];
		
		aDataSource.Columns.Add(dc1);
		   
		//add a row to the datatable
		var dr = new DSDataRow();
		dr["ID"] = loop;
		dr["Title"] = @"Test";
		dr["Description"] = @"Some description would go here";
		dr["Date"] = DateTime.Now.ToShortDateString();
		dr["Value"] = "10000.00";
		
		//set the value as an image
		dr["Image"] =  UIImage.FromFile("first.png")
		
		    	
		aDataSource.Rows.Add(dr); 
		
		
		...
	}
	

Both DSDataSet and DSDataTable have constuctors that will take their .Net counterparts(DataSet and DataTable) as parameters.


**Create Grid View**

Create the grid view as with any normal iOS View and then set the datasource of the DSGridView object.


	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	
	public override void ViewDidLoad ()
	{
		//create the datatable object and set a name
		var aDataSource = new DSDataTable("ADT");
		
		//add a column
		var dc1 = new DSDataColumn("Title");
		dc1.Caption = "Title";
		dc1.ReadOnly = true;
		dc1.DataType = typeof(String);
		dc1.AllowSort = true;
		dc1.Width = ColumnsDefs[aKey];
		
		aDataSource.Columns.Add(dc1);
		
		//add a row to the datatable
		var dr = new DSDataRow();
		dr["ID"] = loop;
		dr["Title"] = @"Test";
		dr["Description"] = @"Some description would go here";
		dr["Date"] = DateTime.Now.ToShortDateString();
		dr["Value"] = "10000.00";    	
		aDataSource.Rows.Add(dr); 
		
		//Create the grid view, assign the datasource and add the view as a subview
		var aGridView = new DSGridView(new RectangleF(0,0,1024,768));
		aGridView.DataSource = aDataSouce;
		this.View.AddSubview(aGridView);
	        
	}

You can also use the DSGridViewController class to load the grid automatically into a Viewcontroller.  



**Register for events**

Once the grid is created you can register for the selection events.



	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	public override void ViewDidLoad ()
	{
		//create the datatable object and set a name
		var aDataSource = new DSDataTable("ADT");
		
		//add a column
		var dc1 = new DSDataColumn("Title");
		dc1.Caption = "Title";
		dc1.ReadOnly = true;
		dc1.DataType = typeof(String);
		dc1.AllowSort = true;
		dc1.Width = ColumnsDefs[aKey];
		
		aDataSource.Columns.Add(dc1);
		   
		//add a row to the datatable
		var dr = new DSDataRow();
		dr["ID"] = loop;
		dr["Title"] = @"Test";
		dr["Description"] = @"Some description would go here";
		dr["Date"] = DateTime.Now.ToShortDateString();
		dr["Value"] = "10000.00";    	
		aDataSource.Rows.Add(dr); 
		
		//Create the grid view, assign the datasource and add the view as a subview
		var aGridView = new DSGridView(new RectangleF(0,0,1024,768));
		aGridView.DataSource = aDataSouce;
		this.View.AddSubview(aGridView);
		        
		// Add handlers for single and double tap on each cell
		aGridView.OnSingleCellTap += (DSGridCellView sender) => 
		{
			//do something
		};
			
		aGridView.OnDoubleCellTap += (DSGridCellView sender) => 
		{
			//do something
		};
			
		//single row selected
		aGridView.OnRowSelect += (DSGridRowView sender, DSDataRow Row) => 
		{
			//do something
		};
			
		//row double tap
		grdView.OnRowDoubleTapped += (DSGridRowView sender, DSDataRow Row) => 
		{
			//do something
		};
	        
	}

** DataTable Hot-Swapping **

If you provide a DSDataSet object as the datasource for the grid you can use hot-swapping to quickly switch between the DSDataTables objects within it.

The DSGridView has a property called TableName for the purpose. Setting the property will switch the data to the specified DataTable and then reload the gridview with the new data.



	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	public override void ViewDidLoad ()
	{
		var aDataSet = new DSDataSet();
		aDataSet.Tables.Add(new DSDataTable("ADT1"));
		aDataSet.Tables.Add(new DSDataTable("ADT2"));
		
		aGridView.DataSource = aDataSet;
		
		//set to the first datatable
		aGridView.TableName = ((DSDataSet) aDataSet).Tables[0].Name;
	}
	
	public void OnClickSwitchDataTable()
	{
		aGridView.TableName = "ADT2";
	}


** Themes and styling**

You can create a custom theme by subclassing either an existing them(such as DSGridViewDefaultTheme) or creating a new theme based on the the DSGridViewTheme abstract class.

To change the look of the grid simply override the property that you want to change and set it to the values you want to use.

As an example to change the color of the default header you could simple create a new Class called AGridViewTheme, as a subclass of DSGridViewDefaultTheme, and override the HeaderColor property.


	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	public class AGridTheme : DSGridViewDefaultTheme
	{
		public override MonoTouch.UIKit.UIColor HeaderColor 
		{
			get 
			{
				return UIColor.LightGray();
			}
		}
	}

For background or selection properties you can also use UIColor objects built with pattern images.

This is an example of the above AGridTheme class but returning a UIColor object using an image instead.


	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	public class AGridTheme : DSGridViewDefaultTheme
	{
		public override MonoTouch.UIKit.UIColor HeaderColor 
		{
			get 
			{
				return UIColor.FromPatternImage(new UIImage("header.png"));
			}
		}
	}


You can then use the new Theme class by setting the CurrentTheme static proprerty on the DSGridViewTheme class


	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	public override void ViewDidLoad ()
	{
		DSGridViewTheme.CurrentTheme = new AGridTheme();
	}




