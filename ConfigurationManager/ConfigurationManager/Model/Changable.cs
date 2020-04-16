using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Model
{
    /// <summary>
    /// Mark a class as Changeable if you want the containing objects to notify you whenever they are changed.
    /// </summary>
    public interface Changable
    {
        void Changed(string property);
    }
}
