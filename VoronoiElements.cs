﻿/*
 * Created by SharpDevelop.
 * User: Burhan
 * Date: 17/06/2014
 * Time: 09:29 م
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

/*
	  Copyright 2011 James Humphreys. All rights reserved.
	
	Redistribution and use in source and binary forms, with or without modification, are
	permitted provided that the following conditions are met:
	
	   1. Redistributions of source code must retain the above copyright notice, this list of
	      conditions and the following disclaimer.
	
	   2. Redistributions in binary form must reproduce the above copyright notice, this list
	      of conditions and the following disclaimer in the documentation and/or other materials
	      provided with the distribution.
	
	THIS SOFTWARE IS PROVIDED BY James Humphreys ``AS IS'' AND ANY EXPRESS OR IMPLIED
	WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
	FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
	CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
	CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
	SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
	ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
	ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	
	The views and conclusions contained in the software and documentation are those of the
	authors and should not be interpreted as representing official policies, either expressed
	or implied, of James Humphreys.
 */

/*
 * C# Version by Burhan Joukhadar
 * 
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */


using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Voronoi2
{
	public class Point
	{
		public double x, y;
		
		public Point (double x, double y)
		{
            this.x = x;
            this.y = y;
		}
		
		public void setPoint ( double x, double y )
		{
			this.x = x;
			this.y = y;
		}

        public double Length()
        {
            return Math.Sqrt(x * x + y * y);
        }
	}
	
	// use for sites and vertecies
	public class Site
	{
		public Point coord;
		public int sitenbr;
        public List<Vector2> region;
        public Boolean water;
		
		public Site ()
		{
			coord = new Point(0, 0);
            region = new List<Vector2>();
            water = true;
		}

        public void AddRegion(GraphEdge g)
        {
            Vector2 v1 = new Vector2((float)g.x1, (float)g.y1);
            Vector2 v2 = new Vector2((float)g.x2, (float)g.y2);

            region.Add(v1);
            region.Add(v2);
        }
	}
	
	public class Edge
	{
		public double a = 0, b = 0, c = 0;
		public Site[] ep;
		public Site[] reg;
		public int edgenbr;
		
		public Edge ()
		{
			ep = new Site[2];
			reg = new Site[2];
		}
	}
	
	
	public class Halfedge
	{
		public Halfedge ELleft, ELright;
		public Edge ELedge;
		public bool deleted;
		public int ELpm;
		public Site vertex;
		public double ystar;
		public Halfedge PQnext;
		
		public Halfedge ()
		{
			PQnext = null;
		}
	}
	
	public class GraphEdge
	{
		public double x1, y1, x2, y2;
		public int site1, site2;
	}
	
	// للترتيب
	public class SiteSorterYX : IComparer<Site>
	{
		public int Compare ( Site p1, Site p2 )
		{
			Point s1 = p1.coord;
			Point s2 = p2.coord;
			if ( s1.y < s2.y )	return -1;
			if ( s1.y > s2.y ) return 1;
			if ( s1.x < s2.x ) return -1;
			if ( s1.x > s2.x ) return 1;
			return 0;
		}
	}
}
