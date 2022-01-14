using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInterface.Settings
{
    public interface IApiSettings
    {
        string BaseAddress { get; set; }
        string InventoryPath { get; set; }     
    }
}
