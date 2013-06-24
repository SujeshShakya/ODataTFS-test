DSGridView is a simple customisable data grid control for you to use in your mobile applications. 

## Features

* Customisable themes
* Works on iPhone and iPad
* Support for standard .Net DataSet and DataTable objects
* Lightweight and simple to use
* Support for Hot-Swapping of DataTables in a DataSet
* Column sorting
* Events for Row and Cell selection
* Support for Row and Cell Double Tap
* Works from an XIB, as a view or a view controller
* Image cells


## Usage

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
	

**Create Grid View**

Create the grid view as with any normal iOS View and then set the datasource of the DSGridView object.


	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	public override void ViewDidLoad ()
	{
		...
		
		//Create the grid view, assign the datasource and add the view as a subview
		var aGridView = new DSGridView(new RectangleF(0,0,1024,768));
		
		//assign the datatsource
		aGridView.DataSource = aDataSouce;
		
		//add to view
		this.View.AddSubview(aGridView);
	        
	}

**Register for events**

Once the grid is created you can register for the selection events


	using DSoft.Datatypes.Grid.Data;
	using DSoft.UI.Grid;
	public override void ViewDidLoad ()
	{
		...
		        
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
		aGridView.OnRowDoubleTapped += (DSGridRowView sender, DSDataRow Row) => 
		{
			//do something
		};
	        
	}

## Release Notes

**1.1**

* Added support for images in cells
* Added ability to adjust the tap delay for a single tap when double tap is enabled by setting DSGridView.DoubleTapTimeout
* Fixed drawing bug which left cells from a previous dataset/table visible if the new one had less columns.



**Trial**

The trial is fully functionally however a popup message is shown on the first click of a row and then every five subsequent clicks.  The full version has this restriction removed.

Screenshots created with [PlaceIt](http://placeit.breezi.com/) and may contain simulated functionality not included in the DSGridView.
