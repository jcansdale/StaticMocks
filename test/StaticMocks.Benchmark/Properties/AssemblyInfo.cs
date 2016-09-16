using System.Reflection;
using System.Runtime.InteropServices;
using BenchmarkDotNet.TestDriven;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("StaticMocks.Benchmark")]
[assembly: AssemblyTrademark("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("132c338b-1e50-44dc-9e21-e468f4696045")]

//[assembly: BenchmarkTestRunner(typeof(FastAndDirtyConfig))]
[assembly: BenchmarkTestRunner(typeof(FastAndDirtyClrAndCoreConfig))]
