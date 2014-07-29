Readme:

	1.CircleView.cs: Circle View will implement following functions:
		1.1 single tapping: changing the circleview background color according to xml from "http://www.colourlovers.com/api/colors/random"
		1.2 double tapping: displaying the title of color in the centre of the circle and double tapping again to hide the title
		1.3 draging view: dragging circle view with the finger
		1.4 the circle view has fade animation with random duration and random opacity. This function can be disabled by commenting line 151.
		1.5 all touch events restrictly happen in the circle area instead of the view rect
		1.6 asynchronously loading xml data from the url. UI is not blocked during the data fetching.
		
	2. ColorDataStructure.cs
		2.1 defining color data structure
		2.2 defining rgb data structure
		
	3. ColorXMLParser.cs
		3.1 defining observer interface
		3.2 parsing xml data
		
	4. GlobalConfig.cs
		4.1 defining all global variables
