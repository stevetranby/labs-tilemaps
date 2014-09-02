using UnityEngine;
using System.Collections;

namespace ST
{
	public class OGAEntity
	{
		private OGAMap map;
		private GameObject gameObject;
		private Vector2 basePos;
		private TileCoord curTile;
		private TileCoord waypointTile;
		private TileCoord goalTile;
		private Direction curDir;

		// feet location offset
		private float yoff = 20.0f;
		private float speed = 0.0f;
		private float speedMax = 60.0f;
		private Animator anim;

		public OGAEntity (OGAMap m, float feetOffset)
		{
			this.map = m;
			curTile = null;
			waypointTile = null;
			goalTile = null;
			yoff = feetOffset;
		}

		// Use this for initialization
		public void Start (GameObject go)
		{
			this.gameObject = go;
			setTile (new TileCoord (5, 10));

			anim = go.GetComponent<Animator> ();
		}

		public enum Direction
		{
			None,
			NorthEast,
			NorthWest,
			SouthEast,
			SouthWest,
		}

		public Vector3 vectorForDirection (Direction dir)
		{
			switch (dir) {
			case Direction.NorthEast:
				return new Vector3 (1f, 0.5f);
			case Direction.NorthWest:
				return new Vector3 (-1f, 0.5f);
			case Direction.SouthEast:
				return new Vector3 (1f, -0.5f);
			case Direction.SouthWest:
				return new Vector3 (-1f, 0.5f);
			}
			return new Vector3 (0, 0, 0);
		}

		// currently hacking around to get the height to work well
		public void setPosition (Vector3 p)
		{
			basePos = new Vector2 (p.x, p.y);

			var z = map.zOrderForTile (curTile) - .5f;
			var h = map.heightForTile (curTile);
			//Debug.Log("h = " + h + " @ " + curTile);

			Vector3 pos = new Vector3 (p.x, p.y + h + yoff, z);
			gameObject.transform.localPosition = pos;
		}

		public void updateFlip ()
		{
			// TODO: only change if different
			Vector3 scale = gameObject.transform.localScale;
			
			bool flip = scale.x <= 0 && (curDir == Direction.NorthEast || curDir == Direction.SouthEast);
			flip = flip || (scale.x >= 0 && (curDir == Direction.NorthWest || curDir == Direction.SouthWest));
			if (flip)
				scale.x *= -1;
			gameObject.transform.localScale = scale;
		}

		public void updateAnim ()
		{
			// NOTE: can't wait for dir change as current code is written or never move into idle
//			if (oldDir != curDir) {
			string animName = null;

			switch (curDir) {
			case Direction.NorthEast:
			case Direction.NorthWest:
				if (speed > 0)
					animName = "bear-walk-ne";
				else
					animName = "bear-idle-ne";
				break;
			case Direction.SouthEast:
			case Direction.SouthWest:
				if (speed > 0)
					animName = "bear-walk-se";
				else
					animName = "bear-idle-se";
				break;
			}

			if (null != animName) {
				int hash = Animator.StringToHash (animName);
				anim.CrossFade (hash, 0.1f);
				anim.Play (hash);
			}
//			}
		}

		public void update ()
		{
			Vector2 curPos = basePos;

			if (null != waypointTile || null != goalTile) {

				TileCoord nextTile = waypointTile != null ? waypointTile : goalTile != null ? goalTile : null;
				if (null != nextTile) {
					var pos3 = map.posForTile (nextTile);
					Vector2 pos = new Vector2 (pos3.x, pos3.y);

					var delta = pos - curPos;

					// TODO: should change direction only when getting next tile
					// direction based on position change 
					if (delta.y > 0) {
						curDir = delta.x < 0 ? Direction.NorthWest : Direction.NorthEast;
					} else {	
						curDir = delta.x < 0 ? Direction.SouthWest : Direction.SouthEast;
					}

					speed = speedMax;
					var vel = delta.normalized * speed * UnityEngine.Time.deltaTime;
					vel = new Vector2 (vel.x, vel.y);
					//Debug.Log ("velocity = " + velocity);
					curPos += vel;

					var epsilon = 1E-04f;
					var dist = Vector3.Distance (curPos, pos);

					if (dist < epsilon || dist <= 2.5f * vel.magnitude) {
						//map.changeTileIndex (nextTile, 1);
						map.changeTileColor (nextTile, new Color (1f, 1f, 1f));
						curPos = map.posForTile (nextTile);
						curTile = nextTile;

						if (null != waypointTile) {
							Debug.Log ("finished waypoint");
							waypointTile = null;
						} else {
							Debug.Log ("finished goal");
							goalTile = null;
						}
					} else {
						// check if we've moved to a new tile
						var coord = map.tileForPos (curPos);
						if (curTile == null || coord.c != curTile.c || coord.r != curTile.r) {
							curTile = coord;
						}
					}
				}
			} else {
				speed = 0.0f;
			}

			setPosition (new Vector3 (curPos.x, curPos.y));

			updateFlip ();
			updateAnim ();

		}

		// Cheap "seek" pathfinding
		public void setGoalTile (TileCoord coord)
		{
			if (! map.validTileCoord (coord))
				return;

			if (null != waypointTile)
				map.changeTileColor (waypointTile, new Color (1f, 1f, 1f));
			if (null != goalTile)
				map.changeTileColor (goalTile, new Color (1f, 1f, 1f));

			
			var pos = basePos;
			curTile = map.tileForPos (pos);
			goalTile = coord;

			int dc = goalTile.c - curTile.c;
			int dr = goalTile.r - curTile.r;

			curDir = Direction.None;
			if (Mathf.Abs (dc) > Mathf.Abs (dr)) {
				// move along c-axis
				curDir = dc < 0 ? Direction.NorthWest : Direction.SouthEast;
				waypointTile = new TileCoord (goalTile.c, curTile.r, -1);
			} else {	
				// move along r-axis
				curDir = dr < 0 ? Direction.SouthWest : Direction.NorthEast;
				waypointTile = new TileCoord (curTile.c, goalTile.r, -1);
			}

			map.changeTileColor (waypointTile, new Color (1f, 0.3f, 0.3f));
			map.changeTileColor (goalTile, new Color (0.3f, 0.3f, 1f));
		}

		public void setTile (TileCoord coord)
		{
//			if (null != curTile)
//				map.changeTileIndex (curTile, 1);
			curTile = coord;
//			map.changeTileIndex (curTile, 2);

			var xy = map.posForTile (curTile);
			setPosition (new Vector3 (xy.x, xy.y));
		}
	}
}
