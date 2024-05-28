using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.NoBrand.Focuser
{
    [DeviceId("ASCOM.NoBrand.Focuser", DeviceName = "Focuser")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]

    internal partial class FocuserSettings
    {
    }
}
