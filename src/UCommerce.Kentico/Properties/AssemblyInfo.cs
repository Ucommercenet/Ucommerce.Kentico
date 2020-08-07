using System.Reflection;
using System.Runtime.InteropServices;
using CMS;
using CMS.Activities;
using UCommerce.Kentico.Macros;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("UCommerce.Kentico")]
[assembly: AssemblyDescription("Ucommerce integration with Kentico")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Ucommerce")]
[assembly: AssemblyProduct("UCommerce.Kentico")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("fb309635-927b-444d-8a50-d1d550d8e177")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("8.4.0.19345")]
[assembly: AssemblyVersion("8.4.0.19345")]
[assembly: AssemblyFileVersion("8.4.0.19345")]
[assembly: AssemblyDiscoverable]
[assembly: RegisterExtension(typeof(ActivityInfoMethods), typeof(ActivityInfo))]
[assembly: RegisterModule(typeof(UcommerceMacroModule))]
[assembly: RegisterExtension(typeof(MacroMethodsBasket), typeof(UcommerceMacroNamespace))]

