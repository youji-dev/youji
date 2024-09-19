using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IState
    {
        /// <summary>
        /// The name of the state.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The color of the state.
        /// </summary>
        string Color { get; set; }
    }
}
