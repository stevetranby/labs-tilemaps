using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Sprite altlas imported from a Sparrow  XML data file
/// </summary>
/// <remarks>
/// Supports trimmed images.
/// </remarks>
public class OTSpriteAtlasBMFontXml : OTSpriteAtlasImportXML 
{

    /// <summary>
    /// Import atlasData from sparrow xml
    /// </summary>
    protected override OTAtlasData[] Import()
    {
        if (!ValidXML())
            return new OTAtlasData[] { };
				
        List<OTAtlasData> data = new List<OTAtlasData>();
		
        if (xml.rootName == "font")
        {
			OTDataset dsInfo = xml.Dataset("info");
			if (!dsInfo.EOF)
			{
				if (dsInfo.AsString("face")!="")
				{					
					if (name.IndexOf("Container (id=")==0)
					{			
						name = "Font "+dsInfo.AsString("face")+"-"+dsInfo.AsString("size");
						if (dsInfo.AsString("bold")=="1")
							name += "b";
						if (dsInfo.AsString("italic")=="1")
							name += "i";
					}
								
					metaType = "FONT";
					OTDataset dsChars = xml.Dataset("chars");
					while (!dsChars.EOF)
					{
	                	OTAtlasData ad = new OTAtlasData();
			
			            ad.name = ""+dsChars.AsInt("id");
			            ad.position = new Vector2(dsChars.AsInt("x"), dsChars.AsInt("y"));
			             ad.size = new Vector2(dsChars.AsInt("width"), dsChars.AsInt("height"));
			            ad.offset = new Vector2(dsChars.AsInt("xoffset"), dsChars.AsInt("yoffset"));		
							
						ad.AddMeta("dx",dsChars.AsString("xadvance"));
			            data.Add(ad);
						dsChars.Next();
					}
				}
			}
        }
        return data.ToArray();
    }
	
	protected override void LocateAtlasData()
	{
#if UNITY_EDITOR 
		
		if (_atlasDataFile==null)
		{		
			string path = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(texture))+"/"+texture.name+".fnt";
			string tpath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(texture))+"/"+texture.name+".xml";
			string fpath = Path.GetFullPath(path);
			string ftpath = Path.GetFullPath(tpath);
			if (File.Exists(fpath))
			{
				File.Copy(fpath,ftpath);			
				UnityEditor.AssetDatabase.DeleteAsset(path);
				UnityEditor.AssetDatabase.ImportAsset(tpath);
				File.Delete(fpath);
			}
			
			base.LocateAtlasData();			
		}
#endif
	}	
	
}