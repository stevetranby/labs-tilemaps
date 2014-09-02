using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		var targ = (Controller)target;
		if (targ)
		{
			targ.Code = EditorGUILayout.TextArea(targ.Code);
		
			if (Application.isPlaying)
			{
				if (GUILayout.Button("Apply"))
					targ.DoCode(targ.Code);
			}
		}
	}
}
