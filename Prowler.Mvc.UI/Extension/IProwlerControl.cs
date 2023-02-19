using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Prowler.Mvc.UI
{
    public interface IProwlerControl
    {
        IHtmlString Render(string name);
        IHtmlString Render();
        string GetName();
    }
}
