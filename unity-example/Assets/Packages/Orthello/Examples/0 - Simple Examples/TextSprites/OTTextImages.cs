using UnityEngine;
using System.Collections.Generic;

public class OTTextImages : MonoBehaviour {

	OTTextSprite text;

	// Use this for initialization
	void Start () {
		text = GetComponent<OTTextSprite>();	
		Fill();
	}
	
	List<string> imgMatrix = new List<string>();
	void Fill()
	{
		if (imgMatrix.Count==10)
			imgMatrix.RemoveAt(0);
		
		while (imgMatrix.Count<10)
		{
			string s = "";
			for (int i=0; i<4; i++)
			{
				switch( (int)Mathf.Floor(Random.value * 3.99f))
				{
					case 0: s+="A"; break;
					case 1: s+="B"; break;
					case 2: s+="C"; break;
					case 3: s+="D"; break;
				}
			}
			imgMatrix.Add(s);
		}
		
		
		string tx = "";
		for (int i=0; i<imgMatrix.Count; i++)
		{
			tx += imgMatrix[i];
			if (i<imgMatrix.Count-1) 
				tx +="\n";
		}
		text.text = tx;		
	}
	
	float timer;
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > .33f)
		{
			Fill();
			timer = 0;
		}
	}
}
