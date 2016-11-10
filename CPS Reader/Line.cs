using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Readers {

    class Line {
        public static Line Empty = new Line("", 0);

        public int Tab { get; set; }

        public string Text { get; set; }

        public Line(string Text, int Tab = 0) {
            this.Text = Text;
            this.Tab = Tab;
        }
    }
}
