using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Model
{
    public interface Changable
    {
        void Changed(string property);
    }
}
