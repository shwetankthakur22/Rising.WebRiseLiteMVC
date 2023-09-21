using Rising.WebRise.Models;
using Rising.WebRiseProcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Repositories.Abstract
{
    public interface ISymbol
    {
        List<Symbol> GetSymbol();

    }
}