using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using OdataTFS;
using AlertView;
using RestSharp;
using System.Xml.Linq;
//using SampleBindingProject;
namespace OdataTFS
{

	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.

	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		#region Private Member Variables
		ProjectCollectionController proCollection;
		public UIWindow  window = new UIWindow(UIScreen.MainScreen.Bounds);
		public  OdataTFSViewController viewController;
		public SplitViewContoller _sp;
		DialogViewController _dvController;
		public UINavigationController _navController;
		LoginModel model = new LoginModel();	
		RootElement rootElement;
		EntryElement name, password;//url,
		RootElement testurl;
		RestClient client;
		LoadingOverlay lodOv;
		UIBarButtonItem _btnSubmit;
		UIAlertView alertView;
		UIImage _bckImg;
		string url="";
		#endregion
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		#region Public Method
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{

			UINavigationController _nav=RootLoginMethod ();
			window.RootViewController = _nav;
			window.MakeKeyAndVisible();

			return true;
		}
		public UINavigationController RootLoginMethod()
		{
			/*
			var request = new RestRequest("/Servers/", Method.GET);//get xml
			var content =ExecuteServer(request);
			try
			{
				XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";//namespace for element
				XNamespace m = "http://www.w3.org/2005/Atom";//namespace for descendants
				XNamespace p = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";//namespace for properties element



				XDocument feedDocument = XDocument.Parse(content);
				List<string> prlist = new List<string>();
				var entryElements = feedDocument.Root.Descendants(m + "entry");
				foreach (var element in entryElements)
				{
					var contentElement = element.Descendants(m + "content").FirstOrDefault();
					var properties = contentElement.Element(p + "properties");


					#region Projects
					ServerList pr = new ServerList();
					pr.server = properties.Element(d + "CollectionName").Value;
					pr.serverId =Convert.ToInt32(properties.Element(d + "Collection").Value);
					prlist.Add(pr.server);

					#endregion
				}
			}
			catch(Exception ex){
			}*/
			/*
			string serialNumber = "";
			try bn
			{
				Device_Info nativeDeviceInfo = new Device_Info ();
				NSString temp = nativeDeviceInfo.GetSerialNumberr();
				serialNumber = temp.ToString();
			}  
			catch (Exception ex) 
			{
				Console.WriteLine("Cannot get serial number {0} - {1}",ex.Message, ex.StackTrace);
			}
*/
			rootElement = new RootElement("Login Credential ") {
				new Section (){
				(testurl=new RootElement ("Server Url", new RadioGroup ("url", 0)) {
						new Section () {
						new RadioElement("https://ubasolutions.visualstudio.com", "url"),
							new RadioElement ("https://idevtfs.fonts.com:8081/tfs", "url"),
							new RadioElement("https://sujeshshakya1.visualstudio.com","url")
						}
					})
				},
				new Section () {
					//(url= new EntryElement ("Url", "Enter Url", "https://ubasolutions.visualstudio.com/"+model.Url)),
					(name=new EntryElement("UserName","Enter User Name","ubasolutions@hotmail.com"+model.Name)),

					(password= new EntryElement ("Password", "Enter Password","ubasolutions"+model.Password, true)),
	}

			};


			name.AutocorrectionType = UITextAutocorrectionType.Yes;
			_dvController = new DialogViewController(rootElement);
			_btnSubmit = new UIBarButtonItem("Connect", UIBarButtonItemStyle.Bordered, delegate { Login(model,testurl.RadioSelected, name.Value, password.Value); });
			_dvController.NavigationItem.RightBarButtonItem = _btnSubmit;
			_dvController.TableView.BackgroundView = null;     
			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			_dvController.View.BackgroundColor = UIColor.FromPatternImage(_bckImg);
			_navController = new UINavigationController(_dvController);
			this._navController.NavigationBar.BarStyle = UIBarStyle.Black;
			return _navController;

		}

		#region  Delegate LoginMethod
		public void Login(LoginModel model,int rdoSelected, string _uname, string _password)
		{

			if (rdoSelected == 0) 
			{
				url = "https://ubasolutions.visualstudio.com";
			} 
			else if (rdoSelected == 1)
			{
				url = "https://idevtfs.fonts.com:8081/tfs";
			} 
			else if (rdoSelected == 2)
			{
				url = "https://sujeshshakya1.visualstudio.com";
			}
			//viewController = new OdataTFSViewController(_uname, _password);
			proCollection = new ProjectCollectionController (_uname, _password);
			ThreadPool.QueueUserWorkItem((f) =>
			                             {
				InvokeOnMainThread(delegate
				                   {
					MBHUDView.HudWithBody (
						body: "Signing In...", 
						aType: MBAlertViewHUDType.ActivityIndicator, 
						delay: 2.0f, 
						showNow: true
						);
					//lodOv = new LoadingOverlay(UIScreen.MainScreen.Bounds);
					//window.AddSubview(lodOv);

				});
				InvokeOnMainThread(delegate
				                   {
					if (url!=""&&_uname != "" && _password != "")
					{
						//bool res=  viewController.GetConnection(url);
						bool res=proCollection.GetConnection(url);
						if (res)
						{

							this._navController.PushViewController(proCollection, true);
						}
						else
						{

							//this._navController.PopToRootViewController(true);
							var alert = MBAlertView.AlertWithBody (
								body: "Error!!! Connection Failure",
								buttonTitle: "OK",
								handler: () => Return ()
								);

							alert.AddButtonWithText (
								text: "Cancel", 
								bType:MBAlertViewItemType.Destructive, 
								handler: () =>Return()
								);
							alert.AddToDisplayQueue ();
							//alertView = new UIAlertView("Error!!!!", "Connection Failure.....", null, "Ok", "Cancel");
							//alertView.Show();
						}
					}
					else
					{
						var alert = MBAlertView.AlertWithBody (
							body: "Error!!! Please Enter All Entry Fields",
							buttonTitle: "OK",
							handler: () => Return ()
							);

						alert.AddButtonWithText (
							text: "Cancel", 
						    bType:MBAlertViewItemType.Destructive, 
							handler: () =>Return()
							);

						alert.AddToDisplayQueue ();
						//this._navController.PopToRootViewController(true);
						//alertView = new UIAlertView("Error!!!!", "Please Enter All Entry Fields", null, "Ok", "Cancel");
						//alertView.Show();
					}
				});

				//InvokeOnMainThread(delegate
				  //                 {
				//	lodOv.Hide();

				//});

			});

		}
		public void Return()
		{
			this._navController.PopToRootViewController (true);
		}


	/*public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
		{
			return UIInterfaceOrientationMask.Landscape;
		}

		public bool shouldAutorotate()
		{

			return true;
		}*/
		public string ExecuteServer(RestRequest request)
		{
			object objError = "";
			string ErrorMessage = "";
			client = new RestClient();
			#region Url comment
			client.BaseUrl = "http://182.93.89.99/tfsodata";
			#endregion
			request.RequestFormat = RestSharp.DataFormat.Xml;
			request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
			IRestResponse response = client.Execute(request);

			List<Parameter> Items = response.Headers.ToList();
			try
			{
				var query = from s in Items
					where s.Name == "Error"
						select s;
				Parameter p = query.Single();
				objError = p.Value;
				ErrorMessage = objError.ToString();
			}
			catch(Exception ex)
			{
				ErrorMessage = "";
			}


			// ResponseStatus status = response.ResponseStatus;

			if (ErrorMessage != "")
			{

				var alert = MBAlertView.AlertWithBody (
					body: "Error!!! Invalid",
					buttonTitle: "OK",
					handler: () => Show ()
					);

				alert.AddToDisplayQueue ();
				return "";

			}
			else
			{
				return response.Content;
			}
		}
		public void Show()
		{

		}

		#endregion
		#endregion
	}
  
}

