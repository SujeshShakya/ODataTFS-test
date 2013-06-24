using System;
using DSoft.UI.Grid;
using MonoTouch.Foundation;
using DSGrid.Sample.Data;
using DSoft.Datatypes.Grid.Data;


namespace OdataTFS
{
/// <summary>
/// An subclass of DSGridView to use in the XIB
/// </summary>
	[Register("AGridView")]
	public class AGridView : DSGridView
	{
	    #region Constructors
		
		public AGridView(IntPtr handle) : base(handle) 
		{
			
			//turn on showing of the selection
			this.ShowSelection = true;
			
			//allow the scrolling to bounce
			this.Bounces = true;
			
			//set the data source to be a DataSet with multiple datatables
			DataSource = new ExampleDataSet();

			//set the first database as the initial grid source
			this.TableName = ((DSDataSet)DataSource).Tables[0].Name;

			//set the double tap timeout
			DSGridView.DoubleTapTimeout = 0.21f;
		}
		
		#endregion
	}
}

