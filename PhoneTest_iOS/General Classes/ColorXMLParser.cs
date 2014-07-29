using System;
using System.Xml;

namespace PhoneTest_iOS
{
	public class ColorXMLParser
	{
		public string stringToParse{ get; set;}
		public ColorXMLParserObserver observer{ get; set;}
		public ColorDS colorDS = new ColorDS();

		public ColorXMLParser ()
		{
		}
		public void startParsing(){
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(stringToParse);
				XmlNode colorsNode = xmlDoc.SelectSingleNode("colors");
				XmlNode colorNode = colorsNode.SelectSingleNode("color");
				colorDS.title = colorNode.SelectSingleNode("title").InnerText.ToString();

				XmlNode rgbNode = colorNode.SelectSingleNode("rgb");
				colorDS.rgb = new RgbDS();
				colorDS.rgb.red = float.Parse(rgbNode.SelectSingleNode("red").InnerText.ToString());
				colorDS.rgb.green = float.Parse(rgbNode.SelectSingleNode("green").InnerText.ToString());
				colorDS.rgb.blue = float.Parse(rgbNode.SelectSingleNode("blue").InnerText.ToString());
				if (observer!=null)
					observer.returnColorDS(colorDS);
			}
			catch (SystemException ex)
			{
				Console.WriteLine("Exception = "+ex);
				if (observer!=null)
					observer.returnColorDSException("failed on parsing xml");
			}
		}//
	}

	public interface ColorXMLParserObserver { 
		void returnColorDS(ColorDS colorDS);
		void returnColorDSException(String errorString);
	}
}

