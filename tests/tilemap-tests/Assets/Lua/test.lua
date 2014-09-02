function create_cube()
  engine = luanet.UnityEngine
  cube = engine.GameObject.CreatePrimitive(engine.PrimitiveType.Cube)
  --cube.transform.localScale = engine.Vector3(32.0,32.0,32.0);
  return cube
end

function update_cube_rotation(cube, angle)
  engine = luanet.UnityEngine
  cube.transform.rotation = engine.Quaternion.AngleAxis(angle, engine.Vector3.right)

  -- NOTE: lua interface doesn't like this
  -- cube.transform.Rotate (engine.Vector3.right, 1.0);
end

function steve_add (a,b)
  return a + b
end

result = steve_add(3,4)
dlog(result)

myCube1 = create_cube()
dlog('myCube1 created!')
