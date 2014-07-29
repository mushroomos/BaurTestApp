using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

/*	Readme:
 *  1.CircleView.cs: Circle View will implement following functions:
 * 		1.1 single tapping: changing the circleview background color according to xml from "http://www.colourlovers.com/api/colors/random"
 * 		1.2 double tapping: displaying the title of color in the centre of the circle and double tapping again to hide the title
 * 		1.3 draging view: dragging circle view with the finger
 * 		1.4 the circle view has fade animation with random duration and random opacity. This function can be disabled by commenting line 151.
 * 		1.5 all touch events restrictly happen in the circle area instead of the view rect
 *		1.6 asynchronously loading xml data from the url. UI is not blocked during the data fetching
 *  2. ColorDataStructure.cs
 * 		2.1 defining color data structure
 * 		2.2 defining rgb data structure
 * 	3. ColorXMLParser.cs
 * 		3.1 defining observer interface
 * 		3.2 parsing xml data
 * 	4. GlobalConfig.cs
 * 		4.1 defining all global variables
 */
namespace PhoneTest_iOS
{
	public partial class PhoneTest_iOSViewController : UIViewController
	{
		//CircleView circleView;
		UITapGestureRecognizer singleTapGesture;

		public PhoneTest_iOSViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.initData ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
			

		#endregion
		#region Instance Methods
		private void initData(){
			singleTapGesture = new UITapGestureRecognizer ((tGesture)=>{

				var point = tGesture.LocationInView(View);
				//*** Create Circle View at the touch point ***
				Random rnd = new Random();
				int rndLength = rnd.Next(60,81);
				CircleView circleView = new CircleView (){Frame = new RectangleF (point.X, point.Y, rndLength,rndLength)};
				circleView.Center = point;
				this.View.AddSubview(circleView);
			});
			this.View.AddGestureRecognizer (singleTapGesture);
		}
		#endregion
	}
}
