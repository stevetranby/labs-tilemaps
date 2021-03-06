using System;

namespace ST
{
	public class Size
	{
		public float width;
		public float height;
		public Size (float w, float h)
		{
			this.width = w;
			this.height = h;
		}
	}

	public class TileCoord
	{
		public int c;
		public int r;
		public int h;

		public TileCoord (int c, int r, int h)
		{
			this.c = c;
			this.r = r;
			this.h = h;
		}

		public TileCoord (int c, int r)
		{
			this.c = c;
			this.r = r;
			this.h = 0;
		}

		public override string ToString ()
		{
			return "TileCoord:[" + this.c + ", " + this.r + ", " + this.h + "]";
		}
	}
}

