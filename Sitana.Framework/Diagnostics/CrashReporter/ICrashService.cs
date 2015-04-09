﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitana.Framework.Diagnostics
{
    public abstract class CrashService
    {
        public virtual async Task<ExceptionData> SendOne(ExceptionData exceptionData)
        {
            await Task.Run(() => { });
            return exceptionData;
        }
    }
}
