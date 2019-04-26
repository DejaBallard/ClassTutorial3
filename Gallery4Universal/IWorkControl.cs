using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery4Universal
{
    interface IWorkControl
    {
        void PushData(clsAllWork prWork);
        void UpdateControl(clsAllWork prWork);
    }
}
