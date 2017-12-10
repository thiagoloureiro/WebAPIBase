using System;
using PostSharp.Aspects;

namespace Service.Cache
{
    [Serializable]
    internal class AffectedCacheableMethodsAttribute : MethodInterceptionAspect
    {
        private readonly string[] _affectedMethods;

        public AffectedCacheableMethodsAttribute(params string[] methods)
        {
            _affectedMethods = methods;
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            base.OnInvoke(args);

            foreach (var mi in _affectedMethods)
                MethodResultCache.GetCache(mi).ClearCachedResults();
        }
    }
}