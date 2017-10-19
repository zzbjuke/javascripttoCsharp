using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.ParseLanguage.Entity {
    public class CPoint {
        public CPoint()
            : this(0, 0) { }

        public CPoint(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public CPoint(CPoint p)
            : this(p.X, p.Y) { }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() {
            return Y + "_" + X;
        }

    }
}
