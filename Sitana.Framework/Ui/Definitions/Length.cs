﻿// SITANA - Copyright (C) The Sitana Team.
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitana.Framework.Ui.Core;

namespace Sitana.Framework.Ui
{
    public struct Length
    {
        public readonly static Length Default = new Length(true);
        public readonly static Length Stretch = new Length(0,1);
        public readonly static Length Zero = new Length();

        double _length;
        double _percent;
        int    _pixels;

        bool _auto;

        public bool IsAuto
        {
            get
            {
                return _auto;
            }
        }

        public double Value
        {
            get
            {
                return _length;
            }
        }

        public Length(bool auto)
        {
            _auto = auto;
            _length = 0;
            _percent = 0;
            _pixels = 0;
        }

        public Length(double length = 0, double percent = 0, int pixels = 0)
        {
            _auto = false;
            _length = length;
            _percent = percent;
            _pixels = pixels;
        }

        public int Compute()
        {
            return Compute(0);
        }

        public int Compute(int size)
        {
            return (int)Math.Ceiling(_percent * size + UiUnit.Unit * _length) + _pixels;
        }
    }
}
