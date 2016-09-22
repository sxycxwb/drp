using System;

namespace DRP.Code.Reflection
{
    public interface ITypeFinder
    {
        Type[] Find(Func<Type, bool> predicate);

        Type[] FindAll();
    }
}