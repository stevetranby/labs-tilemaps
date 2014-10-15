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
public class OTSpriteAtlasCocos2DFnt : OTSpriteAtlasImportText 
{
	int lineHeight = 0;	
    /// <summary>
    /// Import atlasData from sparrow xml
    /// </summary>
    protected override OTAtlasData[] Import()
    {
        if (!Parse())
            return new OTAtlasData[] { };
		List<OTAtlasData> data = new List<OTAtlasData>();
		
		First();
		if (Exists("info") && Exists("face"))
        {
			metaType = "FONT";
			
			if (name.IndexOf("Container (id=")==0)
			{			
				name = "Font "+Data("face")+"-"+Data ("size");
				if (Data ("bold")=="1")
					name += "b";
				if (Data ("italic")=="1")
					name += "i";
			}
						
			do
			{
				
				if (Exists ("common"))
				{
					if (lineHeight == 0)
					{
						lineHeight = System.Convert.ToInt16(Data ("lineHeight"));
					}
					if (Data("scaleW")!="")
						sheetSize = new Vector2(System.Convert.ToSingle(Data ("scaleW")),System.Convert.ToSingle(Data ("scaleH")));
				}
				
				if (Exists ("char"))
				{
		                OTAtlasData ad = new OTAtlasData();
		
		                ad.name = Data ("id");
		                ad.position = new Vector2(IData("x"), IData("y"));
		                ad.size = new Vector2(IData("width"), IData("height"));
		                ad.offset = new Vector2(IData("xoffset"), IData("yoffset"));		
						
						ad.AddMeta("dx",Data("xadvance"));
						ad.frameSize = new Vector2(IData("width"), lineHeight);
						
		                data.Add(ad);
		        }	
			} while (Next ());
        }		
        return data.ToArray();
    }
	
	protected override void LocateAtlasData()
	{
		
		if (atlasDataFile!=null && texture.name == atlasDataFile.name)
			return;		
				
#if UNITY_EDITOR 		
		string path = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(texture))+"/"+texture.name+".fnt";
		string tpath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(texture))+"/"+texture.name+".txt";
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
#endif
	}	
	
	

}