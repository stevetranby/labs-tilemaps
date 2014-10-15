using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
using System.Xml.Linq;
#endif

public class OTXMLDataReader : OTTextDataReader {
	
	/// <summary>
	/// Gets the data object of this xml reader
	/// </summary>
	/// <remarks>
	/// The data object is of type XDocument uf the current device is Windows 8 (METRO) or Windows Phone 8
	/// In  all other cases it is a XmlDocument
	/// </remarks>
	public object data
	{
		get
		{
			return xml;
		}
	}
	
	#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
	XDocument xDoc = new XDocument();
	XDocument xml
	{
		get
		{
			return xDoc;
		}
	}
	#else
	XmlDocument xDoc = new XmlDocument();
	XmlDocument xml
	{
		get
		{
			return xDoc;
		}
	}
	#endif
	
	/// <summary>
	/// Gets the name of the root element of the xml file
	/// </summary>
	public string rootName
	{
		get
		{
			#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
			if (xDoc!=null && available)
				return xDoc.Root.Name.ToString();
			#else
			if (xDoc!=null && available)
				return xDoc.DocumentElement.Name;
			#endif
			else 
				return "";
		}
	}
	
	/// <summary>
	/// Gets the root element of this xml file
	/// </summary>
	/// <remarks>
	/// The root element is of type XElement if the current device is Windows 8 (METRO) or Windows Phone 8
	/// In  all other cases it is a XmlNode
	/// </remarks>
	public object rootElement
	{
		get
		{
			#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
			if (xDoc!=null && available)
				return xDoc.Root;
			#else
			if (xDoc!=null && available)
				return xDoc.DocumentElement;
			#endif
			else 
				return null;
		}
	}
	
	public OTXMLDataReader(string id, TextAsset txAsset) : base(id, txAsset)
	{
	}	
	
	public OTXMLDataReader(string id, string source) : base(id, source)
	{
	}	
	
	object DatasetElement(object dataset)
	{
		OTDataset ds = (OTDataset)dataset;
		if (ds.data!=null && !ds.EOF)
		{
			#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
			List<XElement> list = null;
			if (ds.data is List<XElement>)
				list = (ds.data) as List<XElement>;
			#else
			XmlNodeList list = null;
			if (ds.data is XmlNodeList)
				list = (ds.data) as XmlNodeList;
			#endif
			if (list!=null)
				return list[ds.row];
		}
		return null;
	}
	
	void InitElement(ref object element, ref string elementName)
	{
		if (element is OTDataset)
			element = DatasetElement(element);						
		
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement && elementName.IndexOf("/")>=0)
		{
			string[] sa = elementName.Split('/');
			elementName = sa[sa.Length-1];
			for (int i=0; i<sa.Length-1; i++)
			{
				element = (element as XElement).Element(sa[i]);
				if (element == null)
					return;
			}
		}
		#endif
	}
	
	void InitElement(ref object element)
	{
		string el = "";
		InitElement(ref element, ref el);
	}	
	
	public object Element(OTDataset ds, int row)
	{
		if (ds.data!=null && ds.rowCount>row)
			#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
			return ((List<XElement>)ds.data)[row];
		#else			
		return ((XmlNodeList)ds.data)[row];
		#endif
		return null;
	}
	
	public object Element(string elementName)
	{
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		object element = xml.Root;
		InitElement(ref element, ref elementName);		
		if (element is XElement)
			return (element as XElement).Element(elementName);
		else
			return null;
		#else			
		return xml.DocumentElement.SelectSingleNode(elementName);
		#endif
	}
	public object Element(object element, string elementName)
	{
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		InitElement(ref element, ref elementName);
		if (element is XElement)
			return (element as XElement).Element(elementName);
		#else					
		InitElement(ref element);
		if (element is XmlNode)
			return (element as XmlNode).SelectSingleNode(elementName);
		#endif
		return null;
	}
	public object Elements(string elementName)
	{
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		object element = xml.Root;
		InitElement(ref element, ref elementName);		
		if (element is XElement)
			return new List<XElement>((element as XElement).Elements(elementName));
		else
			return null;
		#else
		return xml.DocumentElement.SelectNodes(elementName);
		#endif
	}
	public object Elements(object element,string elementName)
	{
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		InitElement(ref element, ref elementName);
		if (element is XElement)
			return new List<XElement>((element as XElement).Elements(elementName));
		#else
		InitElement(ref element);
		if (element is XmlNode)
			return (element as XmlNode).SelectNodes(elementName);
		#endif
		return null;
	}	
	
	public string Value(object element)
	{
		string _v = "";
		InitElement(ref element);
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement)
			_v = (element as XElement).Value;
		#else
		if (element is XmlNode)
			_v = (element as XmlNode).InnerText;
		#endif
		if (_v == "")
			_v = Attr (element,"value");
		return _v;
	}
	
	public string Attr(object element, string attr)
	{
		InitElement(ref element);
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement)
			return (element as XElement).Attribute(attr).Value;
		#else
		if (element is XmlNode)
			return (element as XmlNode).Attributes[attr].Value;		
		#endif
		return "";
	}
	
	public string Name(object element)
	{		
		InitElement(ref element);
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement)
			return (element as XElement).Name.ToString();
		#else
		if (element is XmlNode)
			return (element as XmlNode).Name;
		#endif
		
		return "";
	}
	
	public object Next(object element)
	{
		InitElement(ref element);
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement)
		{
			List<XElement> nextElements = new List<XElement>((element as XElement).ElementsAfterSelf());
			if (nextElements.Count>0)
				return nextElements[0];
			else
				return null;
		}
		#else
		if (element is XmlNode)
			return ((XmlNode)element).NextSibling;
		#endif
		return null;
	}
	
	public object Previous(object element)
	{
		InitElement(ref element);
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement)
		{
			List<XElement> prevElements = new List<XElement>((element as XElement).ElementsBeforeSelf());
			if (prevElements.Count>0)
				return prevElements[prevElements.Count-1];
			else
				return null;
		}
		#else
		if (element is XmlNode)
			return ((XmlNode)element).PreviousSibling;
		#endif
		return null;
	}
	
	public object First(object element)
	{
		InitElement(ref element);
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement)
		{
			List<XElement> elements = new List<XElement>((element as XElement).Parent.Elements());
			if (elements.Count>0)
				return elements[0];
			else
				return null;
		}
		#else
		if (element is XmlNode)
			return ((XmlNode)element).ParentNode.FirstChild;
		#endif
		return null;
	}
	
	public object Last(object element)
	{
		InitElement(ref element);
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		if (element is XElement)
		{
			List<XElement> elements = new List<XElement>((element as XElement).Parent.Elements());
			if (elements.Count>0)
				return elements[elements.Count-1];
			else
				return null;
		}
		#else
		if (element is XmlNode)
			return ((XmlNode)element).ParentNode.LastChild;
		#endif
		return null;
	}
	
	public object FindValue(object element, string elementName,  string value)
	{		
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		InitElement(ref element, ref elementName);
		if (element is XElement)
		{
			List<XElement> elements = new List<XElement>((element as XElement).Elements(elementName));
			for (int i=0; i<elements.Count; i++)
				if (elements[i].Value == value)
					return elements[i];
		}
		#else
		InitElement(ref element);
		if (element is XmlNode)
		{
			XmlNode el = (element as XmlNode).SelectSingleNode(elementName+"[text()=\""+value+"\"]");
			return el;
		}
		#endif
		return null;
	}	
	
	string XPathLower(string propName)
	{
		return "translate(@"+propName+", 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')";
	}	
	
	public object FindProp(object element, string elementName,  string propName, string value)
	{		
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		InitElement(ref element, ref elementName);
		if (element is XElement)
		{
			List<XElement> elements = new List<XElement>((element as XElement).Elements(elementName));
			for (int i=0; i<elements.Count; i++)
				if (elements[i].Attribute(propName).ToString().ToLower() == value.ToLower())
					return elements[i];
		}
		#else
		InitElement(ref element);
		if (element is XmlNode)
		{
			XmlNode el = (element as XmlNode).SelectSingleNode(elementName+"["+XPathLower(propName)+"='"+value.ToLower()+"']");
			return el;
		}
		#endif
		return null;
	}	
	
	protected override object OpenDataset (object pdata, string elementName)
	{
		object nodes = null;
		if (available)
		{
			if (pdata==null)
				nodes = this.Elements(elementName);
			else
			{
				if (pdata is OTDataset)
				{
					OTDataset dsData = (pdata as OTDataset);
					if (dsData.data == null || dsData.EOF)
						return null;
					else
						nodes = this.Elements(this.Element(dsData, dsData.row),elementName);
				}
				else
					nodes = this.Elements(pdata,elementName);
			}							
		}		
		return nodes;
	}	
	
	public override int RowCount (object dsData)
	{
		if (dsData == null)
			return 0;
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		List<XElement> rowNodes = (List<XElement>)dsData;
		#else
		XmlNodeList rowNodes = (XmlNodeList)dsData;
		#endif
		return rowNodes.Count;
	}	
	
	void RemoveDOCTYPE()
	{
		int p = text.IndexOf("<!DOCTYPE ");
		int e = p;
		if (p>=0)
		{
			for (int i=p; i<text.Length; i++)
				if (text[i]=='>')
			{
				e = i+2;
				break;
			}
			if (e>p)
			{
				string tmp = text;
				_text = tmp.Substring(0,p);
				_text += tmp.Substring(e, tmp.Length-e);
			}
		}					
	}
	
	public override object GetRow(object dsData, int row)
	{
		if (dsData == null)
			return null;
		try
		{
			#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
			List<XElement> rowNodes = (List<XElement>)dsData;
			return rowNodes[row];
			#else
			XmlNodeList rowNodes = (XmlNodeList)dsData;
			return rowNodes[row];
			#endif
		}
		catch(System.Exception)
		{
		}
		return null;
	}
	
	
	public override object GetData(object dsData, int row, string variable)
	{
		if (dsData == null)
			return null;
		
		#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
		List<XElement> rowNodes = (List<XElement>)dsData;
		XElement dataNode = rowNodes[row];
		// check if variable is an attribute of the row node
		if (dataNode.Attribute(variable)!=null)
			return dataNode.Attribute(variable).Value;
		// check if variable is a childnode
		XElement lNode = dataNode.Element(variable);
		if (lNode!=null)
			return lNode.Value;
		#else
		XmlNodeList rowNodes = (XmlNodeList)dsData;
		XmlNode dataNode = rowNodes[row];
		// check if variable is an attribute of the row node
		if (dataNode.Attributes[variable]!=null)
			return dataNode.Attributes[variable].Value;
		// check if variable is a childnode
		XmlNode lNode = dataNode.SelectSingleNode(variable);
		if (lNode!=null)
			return lNode.InnerText;
		#endif
		return null;
	}
	
	public override bool Open()
	{
		if (base.Open())
		{
			try
			{												
				#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
				RemoveDOCTYPE();
				xDoc = XDocument.Parse(text);			
				if (xDoc.Root!=null)
					#else
					xDoc.LoadXml(text);				
				if (xDoc.DocumentElement!=null)
					#endif
				{
					Available();
					return true;
				}
			}
			catch (System.Exception err)
			{
				Debug.Log("error "+err.Message);
			}				
		}
		_available = false;
		return false;
	}
	
	protected override void UrlLoaded(WWW www)
	{
		loadingUrl = false;
		_text = www.text;
		if (text!="")
		{
			try
			{
				#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
				RemoveDOCTYPE();
				xDoc = XDocument.Parse(text);
				if (xDoc.Root!=null)
					#else
					xDoc.LoadXml(text);				
				if (xDoc.DocumentElement!=null)
					#endif
					Available();
			}
			catch (System.Exception)
			{
			}				
		}
	}
	
	
	
}
