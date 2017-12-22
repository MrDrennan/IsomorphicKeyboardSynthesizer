using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace IsomorphicKeyboardSynthesizer
{
    public class Oscillator : GroupBox
    {
        public Oscillator()
        {
            this.Controls.Add(new Button()
            {
                Name = "BtnSine",
                Location = new Point(10, 25),
                Text = "Sine",
            });
            this.Controls.Add(new Button()
            {
                Name = "BtnSquare",
                Location = new Point(100, 25),
                Text = "Square",
            });
            this.Controls.Add(new Button()
            {
                Name = "BtnSaw",
                Location = new Point(190, 25),
                Text = "Saw",
            });
            this.Controls.Add(new Button()
            {
                Name = "BtnTriangle",
                Location = new Point(10, 65),
                Text = "Triangle",
            });
            this.Controls.Add(new Button()
            {
                Name = "BtnNoise",
                Location = new Point(100, 65),
                Text = "Noise",
            });

            foreach (Control control in this.Controls)
            {
                control.Size = new Size(90, 40);
                control.Font = new Font(FontFamily.GenericSansSerif, 6.75f);
            }
        }
    }
}
