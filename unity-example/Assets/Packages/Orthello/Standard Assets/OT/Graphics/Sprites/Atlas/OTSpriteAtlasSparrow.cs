using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sprite altlas imported from a Sparrow  XML data file
/// </summary>
/// <remarks>
/// Supports trimmed images.
/// </remarks>
public class OTSpriteAtlasSparrow : OTSpriteAtlasImportXML 
{

    /// <summary>
    /// Import atlasData from sparrow xml
    /// </summary>
    protected override OTAtlasData[] Import()
    {
        if (!ValidXML())
            return new OTAtlasData[] { };

        List<OTAtlasData> data = new List<OTAtlasData>();
        if (xml.rootName == "TextureAtlas")
        {
            OTDataset dsTextures = xml.Dataset("SubTexture");
            while (!dsTextures.EOF)
            {
                OTAtlasData ad = new OTAtlasData();

                ad.name = dsTextures.AsString("name");
                ad.position = new Vector2(dsTextures.AsInt("x"), dsTextures.AsInt("y"));
                ad.size = new Vector2(dsTextures.AsInt("width"), dsTextures.AsInt("height"));
                ad.frameSize = new Vector2(dsTextures.AsInt("frameWidth"), dsTextures.AsInt("frameHeight"));
                ad.offset = new Vector2(dsTextures.AsInt("frameX"), dsTextures.AsInt("frameY")) * -1;

                data.Add(ad);
				dsTextures.Next();
            }
        }
        return data.ToArray();
    }

}