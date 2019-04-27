using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUI
{
    public abstract class AUI_Base
    {   //base class for all aui classses
        public DisplayState displayState;
        public AUI_Base() { }
        public virtual void Open() { }
        public virtual void Close() { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }
}
