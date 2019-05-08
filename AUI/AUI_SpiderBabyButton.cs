using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace AUI
{
    public class AUI_SpiderBabyButton : AUI_Base
    {

        //button 
        //8 legs (just lines)

        //behavior:
        //randomly walks around, using it's legs
        //when clicked on, this spider removes itself

            
        //assuming spider is on list, it an remove itself
        List<AUI_Base> ListRef;
        public AUI_SpiderBabyButton(List<AUI_Base> LR)
        {
            ListRef = LR;

        }

        public override void Open()
        {

        }
        public override void Close()
        {

        }
        public override void Update()
        {

        }
        public override void Draw()
        {

        }

        //

        public void Kill()
        {
            ListRef.Remove(this);
        }




    }
}
