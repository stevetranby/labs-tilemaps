using UnityEngine;
using System.Collections;
using System.IO;

public class OTTextDataReader : OTDataReader {
			
	string source = "";
	protected TextAsset txAsset = null;
	protected string _text = "";
	/// <summary>
	/// Gets the text loaded in the 
	/// </summary>
	public string text
	{
		get
		{
			return _text;
		}
	}
		
	protected bool loadingUrl = false;
	
	public OTTextDataReader(string id, string source) : base(id)
	{
		this.source = source;
	}
	
	public OTTextDataReader(string id, TextAsset txAsset) : base(id)
	{
		this.txAsset = txAsset;
	}
	
	public override bool Open()
	{
		base.Open();
		if (txAsset!=null)
		{
			_text = txAsset.text;
			if (text!="")
			{
				if(!(this is OTXMLDataReader))
					Available();
				else
					_available = true;
			}
		}
		else		
		if (source.IndexOf("http://")==0 || source.IndexOf("https://")==0 || source.IndexOf("file://")==0)
		{
			// load as url using the Orthello OT main object
			loadingUrl=true;
			OT.LoadWWW(source,UrlLoaded);			
		}
		else
		{
#if UNITY_METRO			
			_text = source;
			Available();
#else
			if (File.Exists(source))
			{
				StreamReader streamReader = new StreamReader(source);
				_text = streamReader.ReadToEnd();
				streamReader.Close();
				if (text!="" && !(this is OTXMLDataReader))
					Available();
			}
			else
			{
				_text = source;
				Available();
			}
#endif
		}				
		return _available;
	}
	
	protected virtual void UrlLoaded(WWW www)
	{
		loadingUrl = false;
		_text = www.text;
		if (text!="")
				Available();
		return;
	}
	
}
