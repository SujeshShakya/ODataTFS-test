using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using System.Net;
using System.IO;
using RestSharp;
using System.Xml;
using System.Xml.Linq;
using RestSharp.Deserializers;
using System.Text;
using System.Threading;
using MonoTouch.ObjCRuntime;
using ODataTFS;
using AlertView;

namespace OdataTFS
{
	public class ProjectCollectionController:UIViewController
	{
		string uname,password,url="";
		UITableView tableView;
		OdataTFSViewController viewController;
		UIBarButtonItem _btnCancel;
		UIImageView _imgProject;
		List<UIButton> lstProject;
		UIScrollView _scrollView;
		RestClient client;
		UIAlertView alertView;
		UIImage _bckImg;
		float h = 70.0f;
		float w = 300.0f;
		float padding = 10.0f;
		float m=0;
		float x=0;
		int l=0;
		float y=30;
		public ProjectCollectionController (string username,string password)
		{
			uname = username;
			this.password = password;
		}
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{
			this.Title = "Project Collections";
			View.Frame = UIScreen.MainScreen.Bounds;

			_bckImg = UIImage.FromFile ("Images/blue-wood-pattern.jpg");
			View.BackgroundColor = UIColor.FromPatternImage (_bckImg);
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			base.ViewDidLoad();
		}
		public bool GetConnection(string urls)
		{

			tableView = new UITableView(View.Bounds);
			var request = new RestRequest("/ProjectCollections/", Method.GET);//get xml
			var content =Execute(request,urls);
			this.url = urls;

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
					Projects pr = new Projects();
					pr.Name = properties.Element(d + "CollectionName").Value;
					//pr.Collection = properties.Element(d + "Collection").Value;
					prlist.Add(pr.Name);

					#endregion
				}

				//List Image Button Purpose
				int n=prlist.Count;
				lstProject=new List<UIButton>();

				_scrollView=new UIScrollView(new RectangleF(20,10, View.Frame.Width,
				                                            200));

				_scrollView.PagingEnabled = true;
				_scrollView.Bounces = true;
				_scrollView.DelaysContentTouches = true;
				_scrollView.ShowsHorizontalScrollIndicator = true;

				_scrollView.ContentSize = new System.Drawing.SizeF (View.Frame.Width,200);
				//_scrollView.ScrollRectToVisible (new RectangleF (0, 0,View.Frame.Width, View.Frame.Height), true);

				for (int i=0; i<n; i++) {
					var button=new UIButton();
					_imgProject=new UIImageView(UIImage.FromFile("Images/ProjectCollections.png"));
					button.SetBackgroundImage(_imgProject.Image,UIControlState.Normal);
					button.SetTitle (prlist[i].ToString(), UIControlState.Normal);
					//button.TitleLabel.LineBreakMode=UILineBreakMode.WordWrap;
					button.TitleLabel.TextAlignment = UITextAlignment.Center;
					button.SetTitleColor(UIColor.Black,UIControlState.Normal);
					if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait
					    || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown)
					{
						if(l<=3)
						{

							x=padding*(l+1)+(l*w);
							button.Frame = new RectangleF (x,y, w, h); 
							button.Tag=i;
							l++;
						}
						else
						{
							l=0;
							x=padding*(l+1)+(l*w);
							y=y+2*padding+h;
							button.Frame=new RectangleF(x,y,w,h);
							button.Tag=i;
							l++;
						}
					}
					else
					{
						if(l<=4)
						{

							x=padding*(l+1)+(l*w);
							button.Frame = new RectangleF (x,y, w, h); 
							button.Tag=i;
							l++;
						}
						else
						{
							l=0;
							x=padding*(l+1)+(l*w);
							y=y+2*padding+h;
							button.Frame=new RectangleF(x,y,w,h);
							button.Tag=i;
							l++;
						}
					}


					button.TouchDown+= HandleData;
					_scrollView.AddSubview (button);
					lstProject.Add (button);
				}
				View.AddSubview(_scrollView);
				return true;
			}
			catch (Exception ex)
			{
				return false;

			}

		}

		private void HandleData(object sender,EventArgs e)

		{

			ThreadPool.QueueUserWorkItem((f) =>
			                             {
				InvokeOnMainThread(delegate
				                   {
					/*spinner = new LoadingOverlay(new RectangleF(0,0,View.Frame.Width,View.Frame.Height));
					View.AddSubview(spinner);*/
					MBHUDView.HudWithBody (
						body: "Loading Data.....", 
						aType: MBAlertViewHUDType.ActivityIndicator, 
						delay: 1.0f, 
						showNow: true
						);

				});
				InvokeOnMainThread(delegate
				                   {
					UIButton btn = (UIButton)sender;
					string title = btn.Title (UIControlState.Normal);
						viewController=new OdataTFSViewController(uname,password,title);
						bool res=  viewController.GetConnection(url);
						if (res)
						{

							this.NavigationController.PushViewController(viewController, true);
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



				});


			});

		}
		public void Return()
		{
			this.NavigationController.PopToViewController (this, true);
		}
		public string Execute(RestRequest request,string url)
		{
			object objError = "";
			string ErrorMessage = "";
			client = new RestClient();
			#region Url comment
			//string Url="https://ubasolutions.visualstudio.com/DefaultCollection";
			// string Url = "https://sujeshshakya1.visualstudio.com";
			client.BaseUrl = "http://182.93.89.99/tfsodata";
			#endregion


			//client.BaseUrl = "http://192.168.1.210/ODataTFS.Web";
			client.Authenticator = new HttpBasicAuthenticator(uname, password);
			//Get Authentiction for client.Execute
			request.RequestFormat = RestSharp.DataFormat.Xml;
			request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
			request.AddHeader("ServerUrl", url);
			request.AddHeader("Username", uname);
			request.AddHeader("Password", password);
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
	}
}

