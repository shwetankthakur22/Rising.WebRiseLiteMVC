using Rising.WebRise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Rising.WebRiseProecss.Models;

namespace Rising.WebRise.Repositories.Abstract
{
    public interface ICommonRef
    {
        List<Exchange> GetExchange();
        

    }
}