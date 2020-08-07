using CMS.DataEngine;
using CMS.MacroEngine;

namespace UCommerce.Kentico.Macros
{
    public class UcommerceMacroModule : Module
    {
        public UcommerceMacroModule() : base("UcommerceMacros") { }

        protected override void OnInit()
        {
            base.OnInit();

            // Register "Ucommerce" namespace into the macro engine.
            MacroContext.GlobalResolver.SetNamedSourceData("Ucommerce", UcommerceMacroNamespace.Instance);
        }
    }
}