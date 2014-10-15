using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

class OTAssetPostprocessor : AssetPostprocessor {

    static public void OnGeneratedCSProjectFiles () 
	{		
		// adjust solution to 
		WtSolution sln = new WtSolution();		
		sln.Adjust("Assembly-CSharp-vs.csproj");
		sln.Adjust("Assembly-CSharp.csproj");
		sln.Adjust("Assembly-CSharp-firstpass.csproj");
		sln.Adjust("Assembly-CSharp-firstpass-vs.csproj");
		sln.Adjust("Assembly-CSharp-Editor.csproj");		
		sln.Adjust("Assembly-CSharp-Editor-firstpass.csproj");		
		sln.Adjust("Assembly-CSharp-Editor-firstpass-vs.csproj");		
    }
				
	static public void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {				
		
		List<Texture2D> textures = new List<Texture2D>();
        foreach (string str in importedAssets)
		{
			Texture2D tx = AssetDatabase.LoadAssetAtPath(str, typeof(Texture2D)) as Texture2D;
			if (tx!=null)
				textures.Add(tx);
		}
		
		if (textures.Count>0)
		{
			for (int i=0; i<textures.Count; i++)
			{
				OTContainer c = OT.ContainerByTexture(textures[i]);
				if (c!=null)
				{
					if (c is OTSpriteAtlasImport)
					{
						(c as OTSpriteAtlasImport).reloadData = true;
						(c as OTSpriteAtlasImport).Reset(true);
					}
					else
						c.Reset(true);
					OT.Reset();
				}
			}
		}		
    }
}




class WtSolution
{	
	XmlDocument doc = null;
		
	void SetNode(string name, string value, XmlNamespaceManager nsmgr)
	{
		if (doc!=null && doc.DocumentElement!=null)
		{
			XmlNode n = doc.SelectSingleNode("/N:Project/N:PropertyGroup/N:"+name, nsmgr);
			if (n!=null) 
			{
				n.FirstChild.Value = value;
			}
		}
	}
	
	public void Adjust(string fileName)
	{		
		try
		{
			string path = Path.GetDirectoryName(Application.dataPath+"..");
			if (File.Exists(path+"/"+fileName))
			{
				TextReader tx = new StreamReader(path+"/"+fileName);
				if (doc == null)
					doc = new XmlDocument();
				doc.Load(tx);
				tx.Close();			
				if (doc!=null && doc.DocumentElement!=null)
				{			
					string xmlns = doc.DocumentElement.Attributes["xmlns"].Value;
					XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
					nsmgr.AddNamespace("N",xmlns);
						
					SetNode("TargetFrameworkVersion","v4.0", nsmgr);
					// SetNode("DefineConstants","TRACE;UNITY_3_5_6;UNITY_3_5;UNITY_EDITOR;ENABLE_PROFILER;UNITY_STANDALONE_WIN;ENABLE_GENERICS;ENABLE_DUCK_TYPING;ENABLE_TERRAIN;ENABLE_MOVIES;ENABLE_WEBCAM;ENABLE_MICROPHONE;ENABLE_NETWORK;ENABLE_CLOTH;ENABLE_WWW;ENABLE_SUBSTANCE", nsmgr);
					
					TextWriter txs = new StreamWriter(path+"/"+fileName);
					doc.Save(txs);
					txs.Close();
				}						
			}
		}
		catch(System.Exception)
		{
		}
	}	
}