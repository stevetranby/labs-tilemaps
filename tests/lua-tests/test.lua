UnityEngine = luanet.UnityEngine
dlog("[sa] test dt = " .. UnityEngine.Time.deltaTime)
if not cube2 then cube2 = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube) end 
t2 = UnityEngine.Time.realtimeSinceStartup
q2 = UnityEngine.Quaternion.AngleAxis(t2*50, UnityEngine.Vector3.right)
cube2.transform.rotation = q2