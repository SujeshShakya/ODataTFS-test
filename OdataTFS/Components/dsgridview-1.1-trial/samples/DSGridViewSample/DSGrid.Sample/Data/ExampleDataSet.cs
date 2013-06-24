using System;
using System.Collections.Generic;
using DSoft.Datatypes.Grid.Data;

namespace DSGrid.Sample.Data
{
	public class ExampleDataSet : DSDataSet
	{
		public ExampleDataSet ()
		{
			this.Tables.Add(new ExampleDataTable("DT1"));
			this.Tables.Add(new ExampleDataTable2("DT2"));
		}
		
		
		/// <summary>
		/// Create a dicitionary of the available tables
		/// </summary>
		/// <returns>The dictionary.</returns>
		public List<String> TableDictionary
		{
			get
			{
				var dict = new List<String>();
				
				foreach (var aTable in Tables)
				{
				   dict.Add(aTable.Name);
				}
				
				return dict;
			}

		}
	}
}

