using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2.Interfaces
{
    public interface IThrowable
    {
        void Throw(DrawableEntity e);
    }
}
