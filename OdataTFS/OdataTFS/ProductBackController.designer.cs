// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace OdataTFS
{
	[Register ("ProductBackController")]
	partial class ProductBackController
	{
		[Outlet]
		AGridView grdView { get; set; }

		[Action ("didClickShowTables:")]
		partial void didClickShowTables (MonoTouch.Foundation.NSObject sender);

		[Action ("didClickShowTheme:")]
		partial void didClickShowTheme (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (grdView != null) {
				grdView.Dispose ();
				grdView = null;
			}
		}
	}
}
