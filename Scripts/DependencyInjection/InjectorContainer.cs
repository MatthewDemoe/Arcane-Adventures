using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection
{
    public static class InjectorContainer
    {
        private static Injector _Injector;
        public static Injector Injector
        {
            get
            {
                if (_Injector is null)
                {
                    _Injector = new Injector();
                }

                return _Injector;
            }
        }
    }
}