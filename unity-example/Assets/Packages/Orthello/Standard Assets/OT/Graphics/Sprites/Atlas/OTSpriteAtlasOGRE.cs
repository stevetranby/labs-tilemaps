using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sprite altlas imported from a OGRE XML data file
/// </summary>
public class OTSpriteAtlasOGRE : OTSpriteAtlasImportXML 
{

    /// <summary>
    /// Import atlasData from sparrow xml
    /// </summary>
    protected override OTAtlasData[] Import()
    {
        if (!ValidXML())
            return new OTAtlasData[] { };

        List<OTAtlasData> data = new List<OTAtlasData>();
        if (xml.rootName == "Imageset")
        {
            OTDataset dsTextures = xml.Dataset("Image");
			while(!dsTextures.EOF)
			{
                OTAtlasData ad = new OTAtlasData();

                ad.name = dsTextures.AsString("Name");
                ad.position = new Vector2(dsTextures.AsInt("XPos"), dsTextures.AsInt("YPos"));
                ad.size = new Vector2(dsTextures.AsInt("Width"), dsTextures.AsInt("Height"));
                ad.frameSize = new Vector2(dsTextures.AsInt("Width"), dsTextures.AsInt("Height"));
                ad.offset = Vector2.zero;

                data.Add(ad);
				dsTextures.Next();
            }
        }
        return data.ToArray();
    }

}