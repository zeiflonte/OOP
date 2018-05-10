﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZPaint
{
    public interface IPlugin
    {
        string Name();
        Shape Create(PointCollection point, Color fcolor, Color scolor, double strokeThickness);
    }
}
