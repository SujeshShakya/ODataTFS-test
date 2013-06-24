using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
//using DSGrid.Sample.Controllers;
using DSGrid.Sample.Data;
using System.Collections.Generic;
using DSoft.UI.Grid.Themes;
//using DSGrid.Sample.Themes;
using DSoft.UI.Grid.Views;
using DSoft.Datatypes.Grid.Data;
using DSoft.UI.Grid;
using OdataTFS;
using System.Xml.Linq;
using System.Linq;


namespace OdataTFS
{
	public partial class ProductBackController : UIViewController
	{
	 
		//UITableView tableView;
		UIImage _bckImg;
		string _uName,_password,mtitle="";
		List<WorkItem> lst;
		//UIScrollView scrollView;
		//OdataTFSViewController viewController;
		WorkItemViewController _wrkItemView;


		#region Constuctors
		public ProductBackController (string _uname,string password,string mtitle)
		{
			this._uName = _uname;
			this._password = password;
			this.mtitle = mtitle;
		}
		
		#endregion
		
		#region Overrides
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Add handlers for single and double tap on each cell
			/*grdView.OnSingleCellTap += (DSGridCellView sender) => 
			{
				Console.WriteLine(String.Format("Cell clicked at location: X:{0} Y:{1}", sender.X, sender.Y));
			};
			
			grdView.OnDoubleCellTap += (DSGridCellView sender) => 
			{
				Console.WriteLine(String.Format("Cell double clicked at location: X:{0} Y:{1}", sender.X, sender.Y));
			};*/
			
		/*	//single row selected
			grdView.OnRowSelect += (DSGridRowView sender, DSDataRow Row) => 
			{
				Console.WriteLine(String.Format("Row selected at index: {0}", sender.RowIndex));
			};
			
			//row double tap
			grdView.OnRowDoubleTapped += (DSGridRowView sender, DSDataRow Row) => 
			{
				Console.WriteLine(String.Format("Row double clicked at index: {0}", sender.RowIndex));
			};
		*/
			
		}

		public UIViewController GetProductBackLogItem(string ProjectName,string url)
		{
			try
			{
				/*
				_wrkItemView = new WorkItemViewController (_uName, _password,mtitle);
				List<WorkItem> workItem = _wrkItemView.GetWorkItemList(ProjectName,url);

					var query = from s in workItem
					where s.Type == "Product Backlog Item"  
						select s;
				lst = query.ToList ();
				tableView = new UITableView (new RectangleF(0,0,View.Frame.Width+View.Frame.Width/2,View.Frame.Height), UITableViewStyle.Plain);
				tableView.Source = new TableSource (lst, this,ProjectName,url);
				scrollView = new UIScrollView (new RectangleF (0, 0,tableView.Frame.Width,tableView.Frame.Height));
				scrollView.AlwaysBounceHorizontal = true;
				scrollView.ShowsHorizontalScrollIndicator = true;
				scrollView.ShowsVerticalScrollIndicator = false;
				scrollView.AlwaysBounceVertical = false;
				scrollView.SizeToFit ();
				scrollView.ContentSize = new System.Drawing.SizeF (tableView.Frame.Width*2, tableView.Frame.Height);
				scrollView.AddSubview (tableView);
				View.AddSubview (scrollView);*/

				 var aDataSource = new DSDataTable("ADT");

				_wrkItemView = new WorkItemViewController (_uName, _password,mtitle);
				List<WorkItem> workItem = _wrkItemView.GetWorkItemList(ProjectName,url);

				var query = from s in workItem
					where s.Type == "Product Backlog Item"  
						select s;
				lst = query.ToList ();


				var ColumnsDefs = new Dictionary<String,float>();

				ColumnsDefs.Add("Id",100);
				ColumnsDefs.Add("Title",500);
				ColumnsDefs.Add("AreaPath",150);
				ColumnsDefs.Add("IterationPath",300);
				ColumnsDefs.Add("CreatedBy",174);
				ColumnsDefs.Add("State",300);
				ColumnsDefs.Add("Type",300);
				ColumnsDefs.Add("Reason",150);
				ColumnsDefs.Add("Created Date",300);
				ColumnsDefs.Add("Assigned To",300);
				ColumnsDefs.Add("Revision",50);
				ColumnsDefs.Add("Description",200);
				ColumnsDefs.Add("RegroSteps",100);
				ColumnsDefs.Add("IntegratedInBuild",50);
				ColumnsDefs.Add("Project",150);

				foreach (var aKey in ColumnsDefs.Keys)
				{
					// Create a column
					var dc1 = new DSDataColumn(aKey);
					dc1.Caption = aKey;
					dc1.ReadOnly = true;
					dc1.DataType = typeof(String);
					dc1.AllowSort = true;
					dc1.Width = ColumnsDefs[aKey];

					aDataSource.Columns.Add(dc1);
				}

				for (int loop = 0;loop < lst.Count ;loop++)
				{
					var dr = new DSDataRow();
					dr["Id"] = lst[loop].Id;
					dr["Title"] = lst[loop].Title;
					dr["AreaPath"] = lst[loop].AreaPath;
					dr["IterationPath"] = lst[loop].IterationPath;
					dr["CreatedBy"] = lst[loop].CreatedBy;
					dr["State"] = lst[loop].State;
					dr["Type"] = lst[loop].Type;
					dr["Reason"] = lst[loop].Reason;
					dr["Created Date"] = lst[loop].CreatedDate;
					dr["Assigned To"] = lst[loop].AssignedTo;
					dr["Revision"] = lst[loop].Revision;
					dr["Description"] = lst[loop].Description;
					dr["RegroSteps"] = lst[loop].ReproSteps;
					dr["IntegratedInBuild"] = lst[loop].IntegratedInBuild;
					dr["Project"] = lst[loop].Project;


					aDataSource.Rows.Add(dr);
		
				}

				//Create the grid view, assign the datasource and add the view as a subview
				var aGridView = new DSGridView(new RectangleF(0,0,1024,768));
				aGridView.DataSource = aDataSource;
				this.View.AddSubview(aGridView);

				return this;
			}
			catch(Exception ex) {
				return null;
			}

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
						//newtheme = new AGridTheme();
					}
					break;
					case 2:
					{
						//newtheme = new AGridThemeItunes();
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

	public class TableSource : UITableViewSource
	{
		protected List<WorkItem> _tableItems;
		protected string _cellIdentifier = "TableCell";
		ProductBackController vc;
		UIBarButtonItem _btnCancel;
		WorkItem[] lstSprint = null;
		string projectName="";
		BoardViewController _brdController;
		UIBarButtonItem _btnDetails;
		UINavigationController _nav;
		string url="";
		ProductBackLogDetail _popOverController;
		public TableSource(List<WorkItem> product,ProductBackController vcon,string ProjectName,string url)
		{
			this.lstSprint = product.ToArray();
			this.vc=vcon;
			this.projectName=ProjectName;
			this.url=url;
		}



		public override int RowsInSection(UITableView tableview, int section)
		{
			if (lstSprint != null)
			{
				return this.lstSprint.Length;
			}
			else
			{
				return 0;
			}
		}
		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 35;
		}
		public override UIView GetViewForHeader(UITableView tableView, int section)
		{

			return BuildSectionHeaderView("ProductBackLog Items",tableView.Frame.Width);
		}

		public static UIView BuildSectionHeaderView(string caption,float width)
		{
			UIView view = new UIView(new System.Drawing.RectangleF(0, 0, 320, 20));
			UILabel label = new UILabel();
			//UIImage _headImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			label.BackgroundColor = UIColor.LightGray;
			label.Opaque = false;
			label.TextColor = UIColor.Black;
			label.Font = UIFont.FromName("Helvetica-Bold", 14f);
			label.Frame = new System.Drawing.RectangleF(0, 0, width, 20);
			label.Text = caption;
			view.AddSubview(label);
			return view;
		}
		public override string TitleForHeader(UITableView tableView, int section)
		{
			return "ProductBackLog Items";
		}


		public static void ClearEmptyTableViewSpacer(UITableView oTableView)
		{
			UIView oViewSpacerClear = new UIView(new RectangleF(0, 0, oTableView.Frame.Width, 10));
			oViewSpacerClear.BackgroundColor = UIColor.Clear;
			oTableView.TableFooterView = oViewSpacerClear;

		}

		public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				tableView.BackgroundView = null;
			}
			else
			{
				tableView.BackgroundColor = null;
			}
			//---- declare vars
			UITableViewCell cell = tableView.DequeueReusableCell(this._cellIdentifier);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Value1, this._cellIdentifier);

			}

			cell.ImageView.Image = UIImage.FromFile ("Images/Clipboard.png");
			cell.TextLabel.Text = lstSprint[indexPath.Row].Title;
			cell.DetailTextLabel.Text = lstSprint [indexPath.Row].State;
			NSIndexPath path = NSIndexPath.FromRowSection (0, 0);
			tableView.SelectRow (path, true, UITableViewScrollPosition.Bottom);
			ClearEmptyTableViewSpacer(tableView);
			return cell;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{

			/*	_brdController = new BoardViewController (lstSprint[indexPath.Row]);
			     _nav = new UINavigationController (_brdController)
				{ 
					ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
					ModalPresentationStyle = UIModalPresentationStyle.FullScreen,

				};

				_btnCancel= new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {Back();});
				_brdController.NavigationItem.LeftBarButtonItem = _btnCancel;
				_btnCancel.TintColor=UIColor.Red;
				_btnDetails=new UIBarButtonItem("Details",UIBarButtonItemStyle.Bordered,delegate{Details(lstSprint[indexPath.Row]);});
				 vc.ToolbarItems = new UIBarButtonItem[] { _btnDetails};
				 
				_brdController.NavigationItem.RightBarButtonItems = vc.ToolbarItems;
				vc.View.Subviews[0].RemoveFromSuperview ();
				vc.PresentViewController(_nav, true,null);*/

			/*popOverController = new ProductBackLogDetail(lstSprint[indexPath.Row]);
				UINavigationController _nav = new UINavigationController (_popOverController)
				{ 
					ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
					ModalPresentationStyle = UIModalPresentationStyle.FormSheet,

				};
				_btnCancel = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, delegate {_popOverController.Close();});
				_popOverController.NavigationItem.LeftBarButtonItem = _btnCancel;
				_nav.NavigationBar.BarStyle = UIBarStyle.Black;
				this.vc.PresentModalViewController (_nav, true);*/
		}


	}

	public partial class ProductBackDetailController : UIViewController
	{

		//UITableView tableView;
		UIImage _bckImg;
		string _uName,_password,mtitle="";
		List<WorkItem> lst;
		//UIScrollView scrollView;
		//OdataTFSViewController viewController;
		WorkItemViewController _wrkItemView;


		#region Constuctors
		public ProductBackDetailController ()
		{

		}

		#endregion

		#region Overrides

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Add handlers for single and double tap on each cell
			/*grdView.OnSingleCellTap += (DSGridCellView sender) => 
			{
				Console.WriteLine(String.Format("Cell clicked at location: X:{0} Y:{1}", sender.X, sender.Y));
			};
			
			grdView.OnDoubleCellTap += (DSGridCellView sender) => 
			{
				Console.WriteLine(String.Format("Cell double clicked at location: X:{0} Y:{1}", sender.X, sender.Y));
			};*/

			/*	//single row selected
			grdView.OnRowSelect += (DSGridRowView sender, DSDataRow Row) => 
			{
				Console.WriteLine(String.Format("Row selected at index: {0}", sender.RowIndex));
			};
			
			//row double tap
			grdView.OnRowDoubleTapped += (DSGridRowView sender, DSDataRow Row) => 
			{
				Console.WriteLine(String.Format("Row double clicked at index: {0}", sender.RowIndex));
			};
		*/

		}

		public UIViewController GetProductBackLogDetail(List<WorkItem> lst) 
		{
			try
			{

				var aDataSource = new DSDataTable("ADT");

				var ColumnsDefs = new Dictionary<String,float>();

				ColumnsDefs.Add("Id",100);
				ColumnsDefs.Add("Title",500);
				ColumnsDefs.Add("AreaPath",150);
				ColumnsDefs.Add("IterationPath",300);
				ColumnsDefs.Add("CreatedBy",174);
				ColumnsDefs.Add("State",300);
				ColumnsDefs.Add("Type",300);
				ColumnsDefs.Add("Reason",150);
				ColumnsDefs.Add("Created Date",300);
				ColumnsDefs.Add("Assigned To",300);
				ColumnsDefs.Add("Revision",50);
				ColumnsDefs.Add("Description",200);
				ColumnsDefs.Add("RegroSteps",100);
				ColumnsDefs.Add("IntegratedInBuild",50);
				ColumnsDefs.Add("Project",150);

				foreach (var aKey in ColumnsDefs.Keys)
				{
					// Create a column
					var dc1 = new DSDataColumn(aKey);
					dc1.Caption = aKey;
					dc1.ReadOnly = true;
					dc1.DataType = typeof(String);
					dc1.AllowSort = true;
					dc1.Width = ColumnsDefs[aKey];

					aDataSource.Columns.Add(dc1);
				}

				for (int loop = 0;loop < lst.Count ;loop++)
				{
					var dr = new DSDataRow();
					dr["Id"] = lst[loop].Id;
					dr["Title"] = lst[loop].Title;
					dr["AreaPath"] = lst[loop].AreaPath;
					dr["IterationPath"] = lst[loop].IterationPath;
					dr["CreatedBy"] = lst[loop].CreatedBy;
					dr["State"] = lst[loop].State;
					dr["Type"] = lst[loop].Type;
					dr["Reason"] = lst[loop].Reason;
					dr["Created Date"] = lst[loop].CreatedDate;
					dr["Assigned To"] = lst[loop].AssignedTo;
					dr["Revision"] = lst[loop].Revision;
					dr["Description"] = lst[loop].Description;
					dr["RegroSteps"] = lst[loop].ReproSteps;
					dr["IntegratedInBuild"] = lst[loop].IntegratedInBuild;
					dr["Project"] = lst[loop].Project;


					aDataSource.Rows.Add(dr);

				}

				//Create the grid view, assign the datasource and add the view as a subview
				var aGridView = new DSGridView(new RectangleF(0,0,1024,768));
				aGridView.DataSource = aDataSource;
				this.View.AddSubview(aGridView);

				return this;
			}
			catch(Exception ex) {
				return null;
			}

		}


		[Obsolete ("Deprecated in iOS6. Replace it with both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation")]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		#endregion

		/*
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
					//newtheme = new AGridTheme();
				}
					break;
					case 2:
				{
					//newtheme = new AGridThemeItunes();
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
		*/
	}
}


