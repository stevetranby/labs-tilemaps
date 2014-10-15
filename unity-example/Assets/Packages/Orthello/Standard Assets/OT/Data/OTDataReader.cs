using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base data reader class to read external data into the framework
/// </summary>
public class OTDataReader : Object {
	
	public delegate void DataReaderDelegate(OTDataReader reader);
	
	protected bool _available = false;
	/// <summary>
	/// This delegate is executed when the data becomes available
	/// </summary>
	public DataReaderDelegate onDataAvailable = null;
	
	/// <summary>
	/// If true, readers will stay open
	/// </summary>
	/// <remarks>
	/// When you are keeping the readers and register a new reader using OTDataReader.Register(newReader) and the new reader has the same id as 
	/// one that is already available the reader will re-use that already available reader.
	/// </remarks>
	static bool keepReaders = false;
	static List<OTDataReader> all = new List<OTDataReader>();
	static Dictionary<string, OTDataReader>lookup = new Dictionary<string, OTDataReader>();	
	
	/// <summary>
	/// Registeres the specified newReader.
	/// </summary>
	/// <remarks>
	/// When OTDataReader.keepReaders is true and you are registering a new reader using OTDataReader.Register(newReader) and the new reader has 
	/// the same id as one that is already available the reader will re-use that already available reader.
	/// </remarks>
	public static OTDataReader Register(OTDataReader newReader)
	{
		string id = newReader.id;
		
		if (lookup.ContainsKey(id) && !keepReaders)
			lookup[id].Close();
		
		if (!lookup.ContainsKey(id))
		{					
			all.Add(newReader);
			lookup.Add(id,newReader);
		}
		
		return lookup[id];
	}
	
	public virtual object GetRow (object dsData, int row)
	{
		return null;
	}
	
	public virtual object GetData (object dsData, int row, string variable)
	{
		return null;
	}
	
	public virtual int RowCount (object dsData)
	{
		return 0;
	}	
	
	
	protected void Available()
	{
		_available = true;
		if (onDataAvailable!=null)
			onDataAvailable(this);						
	}
	
	string _id;
	/// <summary>
	/// Gets the id of this reader
	/// </summary>
	public string id
	{
		get
		{
			return _id;
		}
	}
	
	/// <summary>
	/// Closes all readers
	/// </summary>
	static public void CloseAll()
	{
		while (all.Count>0)
		{
			if (all[0].available)
				all[0].Close();
			else
				all.RemoveAt(0);
		}
		
		all.Clear();
	}
	
	/// <summary>
	/// True if the reader is opened and available
	/// </summary>
	public bool available
	{
		get
		{
			return _available;
		}
	}
	
	/// <summary>
	/// Opens the reader
	/// </summary>
	public virtual bool Open()
	{
		_available = false;
		return _available;
	}	
	
	/// <summary>
	/// Closes the reader
	/// </summary>
	public virtual void Close()
	{
		if (lookup.ContainsKey(id))
			lookup.Remove(id);
		if (all.Contains(this))
			all.Remove(this);
	}	
	
	public OTDataReader(string id)
	{		
		this._id = id;			
	}
	
	protected virtual object OpenDataset(object data, string datasource)
	{
		return null;
	}
	
	protected object OpenDataset(string datasource)
	{
		return OpenDataset(null,datasource);
	}
	
	/// <summary>
	/// Loads a dataset
	/// </summary>
	/// <remarks>
	/// This first row of this dataset becomes the active dataset row
	/// </remarks>
	public OTDataset Dataset(string datasource)
	{		
		return new OTDataset(this,OpenDataset(datasource));
	}							
	
	/// <summary>
	/// Loads a sub dataset within the current row of a dataset
	/// </summary>
	/// <remarks>
	/// This first row of this dataset becomes the active dataset row
	/// </remarks>
	public OTDataset Dataset(object data,string datasource)
	{		
		return new OTDataset(this,OpenDataset(data,datasource));
	}							
}

public class OTDataset
{
	object _data;
	OTDataReader reader;
	
	public object data
	{
		get
		{
			return _data;
		}
	}
	
	public object currentRow
	{
		get
		{
			return CurrentRow();
		}
	}
	
	/// <summary>
	/// Gets a (sub)dataset from the current record of this dataset
	/// </summary>
	public OTDataset Dataset(string datasource)
	{		
		return reader.Dataset(this,datasource);
	}							
	
	public OTDataset(OTDataReader reader, object data)
	{
		this.reader = reader;
		this._data = data;
		
		_row = -1;
		_bof = _eof = true;
		
		if (data!=null)
		{
			_row = 0;
			_bof = _eof = false;
		}
		
	}
	
	/// <summary>
	/// Gets or sets the active row of the current dataset
	/// </summary>
	public int rowCount
	{
		get
		{
			if (data == null)
				return 0;
			else
				return reader.RowCount(data);
		}
	}
	
	int _row = -1;
	/// <summary>
	/// Gets or sets the active row of the current dataset
	/// </summary>
	public int row
	{
		get
		{
			return _row;
		}
		set
		{
			if (data==null || value<0 || value>=rowCount)
				throw new System.Exception("Invalid dataset row!");
			else
			{
				_row = value;											
				_bof = false;
				_eof = false;
			}
		}
	}
	
	/// <summary>
	/// To first row of current dataset
	/// </summary>
	public void First()
	{
		_bof = false;
		_eof = false;
		row = 0;
	}
	/// <summary>
	/// To last row of current dataset
	/// </summary>
	public void Last()
	{
		_bof = false;
		_eof = false;
		row = rowCount-1;
	}
	/// <summary>
	/// To previous row of current dataset
	/// </summary>
	public void Previous()
	{
		if (row>1)
			row--;
		else
		{
			_bof = true;
			_row = 0;
		}
	}
	/// <summary>
	/// To next row of current dataset
	/// </summary>
	public void Next()
	{
		if (row<rowCount-1)
			row++;
		else
		{
			_eof = true;
			_row = rowCount-1;
		}
	}
	
	bool _eof = false;
	/// <summary>
	/// True if we got to the last row of the dataset
	/// </summary>
	public bool EOF
	{
		get
		{
			if (data==null)
				return true;			
			return _eof;
		}
	}
	
	bool _bof = false;
	/// <summary>
	/// True if we got to the first row of the dataset
	/// </summary>
	public bool BOF
	{
		get
		{
			if (data==null)
				return true;
			return _bof;
		}
	}
	
	
	protected virtual object GetData(string variable)
	{
		return null;
	}
	
	object Data(string variable)
	{
		return reader.GetData(data,row,variable);
	}
	
	object CurrentRow()
	{
		return reader.GetRow(data,row);
	}
	
	
	/// <summary>
	/// Gets data as a string from the reader
	/// </summary>
	public string AsString(string variable)
	{
		try
		{
			return System.Convert.ToString(Data(variable));
		}
		catch(System.Exception)
		{
			return "";
		}
	}
	
	/// <summary>
	/// Gets data as an integer from the reader
	/// </summary>
	public int AsInt(string variable)
	{
		try
		{
			return System.Convert.ToInt32(Data(variable));
		}
		catch(System.Exception)
		{			
			return 0;
		}
	}
	
	/// <summary>
	/// Gets data as an integer from the reader
	/// </summary>
	public float AsFloat(string variable)
	{
		try
		{
			return System.Convert.ToSingle(Data(variable));
		}
		catch(System.Exception)
		{			
			return 0.0f;
		}
	}
	
	/// <summary>
	/// Gets data as an integer from the reader
	/// </summary>
	public bool AsBool(string variable)
	{
		try
		{
			return System.Convert.ToBoolean(Data(variable));
		}
		catch(System.Exception)
		{			
			return false;
		}
	}	
}
