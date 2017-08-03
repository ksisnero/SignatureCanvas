using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;

namespace SignatureProject
{
    public class InkCanvasControl : InkCanvas
    {
        public virtual StrokeCollection sig { get; set; }
    }
}
