using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Abstractions
{
    public interface IGuard
    {
        void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message = "", IDictionary<string, string> dataCollection = null);
        void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message = "", params object[] dataCollection);

    }
}
