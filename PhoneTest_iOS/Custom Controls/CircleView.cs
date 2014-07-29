using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using MonoTouch.CoreGraphics;
using System.Threading.Tasks;
using System.Net.Http;

namespace PhoneTest_iOS
{
	[Register ("CircleView")]
	public class CircleView:UIView,ColorXMLParserObserver
	{
		UIPanGestureRecognizer panGesture; 
		UITapGestureRecognizer singleTapGesture;
		UITapGestureRecognizer doubleTapGesture;

		public ColorDS colorDS;
		public UILabel titleLabel;
		private UIColor circleColor = UIColor.Clear;

		float dx = 0;
		float dy = 0;

		public CircleView ()
		{
			BackgroundColor = UIColor.Clear;
			initTitleLabel ();
			initGestures ();
			AsyncLoadingHttp();
		}
		public CircleView(IntPtr handle):base(handle){
		
		}

		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);

			//get graphics context
			using (CGContext context = UIGraphics.GetCurrentContext ()) {
				circleColor.SetFill ();
				context.FillEllipseInRect (rect);
				context.FillPath ();
			}
		}
		private void initTitleLabel(){
			titleLabel = new UILabel (Frame = new RectangleF(0,0,this.Frame.Size.Width,this.Frame.Size.Height));
			titleLabel.TextAlignment = UITextAlignment.Center;
			titleLabel.BackgroundColor = UIColor.Clear;
			titleLabel.AdjustsFontSizeToFitWidth = true;
			titleLabel.Text = "Title";
			titleLabel.Hidden = true;
			titleLabel.UserInteractionEnabled = false;
			this.AddSubview (titleLabel);
		}
		#region Gestures Methods
		private void initGestures(){
			//init panGesture
			panGesture = new UIPanGestureRecognizer ((pGesture) => {
				var p0 = pGesture.LocationInView (Superview);

				if ((pGesture.State == UIGestureRecognizerState.Began || pGesture.State == UIGestureRecognizerState.Changed) && (pGesture.NumberOfTouches == 1)) {
					if (dx == 0)
						dx = p0.X - Center.X;
					if (dy == 0)
						dy = p0.Y - Center.Y;
					var p1 = new PointF (p0.X - dx, p0.Y - dy);
					Center = p1;
				} else if (pGesture.State == UIGestureRecognizerState.Ended) {
					dx = 0;
					dy = 0;
				}
			});

			//init singleTapGuesture
			singleTapGesture = new UITapGestureRecognizer ((tGesture)=>{
				Console.WriteLine("single tap");
				AsyncLoadingHttp();
			});
			//init doubleTapGuesture
			doubleTapGesture = new UITapGestureRecognizer ((tGesture)=>{
				titleLabel.Hidden = !titleLabel.Hidden;
			});

			singleTapGesture.NumberOfTapsRequired = 1;
			doubleTapGesture.NumberOfTapsRequired = 2;
			singleTapGesture.RequireGestureRecognizerToFail(doubleTapGesture);

			panGesture.ShouldReceiveTouch += (recognizer, touch) => { 
				var location = touch.LocationInView(this.Superview);
				return touchInArea (location);
			};
			singleTapGesture.ShouldReceiveTouch += (recognizer, touch) => { 
				var location = touch.LocationInView(this.Superview);
				return touchInArea (location);
			};
			doubleTapGesture.ShouldReceiveTouch += (recognizer, touch) => { 
				var location = touch.LocationInView(this.Superview);
				return touchInArea (location);
			};
			
			this.addAllGestures();
		}
		private void addAllGestures(){
			this.AddGestureRecognizer (singleTapGesture);
			this.AddGestureRecognizer (doubleTapGesture);
			this.AddGestureRecognizer (panGesture);
		}
		private void removeAllGestures(){
			this.RemoveGestureRecognizer (singleTapGesture);
			this.RemoveGestureRecognizer (doubleTapGesture);
			this.RemoveGestureRecognizer (panGesture);
		}
		//*** check if touch inside the circle area ***
		private bool touchInArea(PointF point ){
			double squareX = (point.X - Center.X)*(point.X - Center.X);
			double squareY = (point.Y - Center.Y)*(point.Y - Center.Y);
			double distance = Math.Sqrt(squareX+squareY);
			if (distance > this.Frame.Size.Width / 2)
				return false;
			else
				return true;
		}
		#endregion
		#region Asynch Http Methods
		async void AsyncLoadingHttp ()
		{
			var stringResult = await DownloadHomepage();
			Console.WriteLine(stringResult);
		}
		public async Task<string> DownloadHomepage()
		{
			var httpClient = new HttpClient(); 
			Task<string> contentsTask = httpClient.GetStringAsync(GlobalConfig.colorUrl);
			string contents = await contentsTask;
			ColorXMLParser parser = new ColorXMLParser ();
			parser.stringToParse = contents;
			parser.observer = this;
			parser.startParsing ();
			return contents; 
		}
		#endregion
		#region ColorXMLParserObserver Methods
		public void returnColorDS(ColorDS colorDS){
			this.colorDS = colorDS;
			this.circleColor = UIColor.FromRGB((int)colorDS.rgb.red,(int)colorDS.rgb.green,(int)colorDS.rgb.blue);
			this.titleLabel.Frame = new RectangleF (0, 0, this.Frame.Size.Width, this.Frame.Size.Height);
			this.titleLabel.Text = colorDS.title;
			this.SetNeedsDisplay ();
			startAnimation ();
		}
		public void returnColorDSException(String errorString){
		
		}
		#endregion
		#region Animation
		private void startAnimation(){
			Random rnd = new Random ();
			UIView.BeginAnimations ("flashingAnimation");
			UIView.SetAnimationDuration (rnd.Next(1,5));
			UIView.SetAnimationRepeatCount (Single.PositiveInfinity);
			UIView.SetAnimationRepeatAutoreverses (true);
			this.Alpha = (rnd.Next(5,10))/10.0f;
			UIView.CommitAnimations ();
		}
		#endregion
	}
}

