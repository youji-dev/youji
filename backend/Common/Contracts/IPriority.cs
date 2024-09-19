using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IPriority
    {
        /// <summary>
        /// The name of the priority.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The value of the priority.
        /// </summary>
        int Value { get; set; }
    }
}
