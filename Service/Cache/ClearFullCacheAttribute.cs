using System;
using System.Reflection;
using PostSharp.Aspects;

namespace Service.Cache
{
    [Serializable]
    internal class ClearFullCacheAttribute : MethodInterceptionAspect
    {
        private MethodInfo[] _affectedMethods;
        private string _className;

        public ClearFullCacheAttribute(string str)
        {
            _className = str;
        }

        public sealed override void OnInvoke(MethodInterceptionArgs args)
        {
            var classType = Type.GetType(_className);

            _affectedMethods = classType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var mi in _affectedMethods)
                MethodResultCache.GetCache(mi.Name).ClearCachedResults();
        }
    }
}