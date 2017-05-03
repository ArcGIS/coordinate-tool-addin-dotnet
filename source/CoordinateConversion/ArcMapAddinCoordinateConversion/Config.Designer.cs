//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArcMapAddinCoordinateConversion
{
    using ESRI.ArcGIS.Framework;
    using ESRI.ArcGIS.ArcMapUI;
    using System;
    using System.Collections.Generic;
    using ESRI.ArcGIS.Desktop.AddIns;
    using ESRI.ArcGIS.Carto;


    /// <summary>
    /// A class for looking up declarative information in the associated configuration xml file (.esriaddinx).
    /// </summary>
    internal static class ThisAddIn
    {

        internal static string Name
        {
            get
            {
                return "Coordinate Conversion";
            }
        }

        internal static string AddInID
        {
            get
            {
                return "{19b92973-746a-4114-9232-3467ca1fc631}";
            }
        }

        internal static string Company
        {
            get
            {
                return "Esri";
            }
        }

        internal static string Version
        {
            get
            {
                return "1.0.2";
            }
        }

        internal static string Description
        {
            get
            {
                return "Quickly convert coordinate between several formats";
            }
        }

        internal static string Author
        {
            get
            {
                return "Esri";
            }
        }

        internal static string Date
        {
            get
            {
                return "9/23/2015";
            }
        }

        internal static ESRI.ArcGIS.esriSystem.UID ToUID(this System.String id)
        {
            ESRI.ArcGIS.esriSystem.UID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = id;
            return uid;
        }

        /// <summary>
        /// A class for looking up Add-in id strings declared in the associated configuration xml file (.esriaddinx).
        /// </summary>
        internal class IDs
        {

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_ContextCopyDD', the id declared for Add-in Button class 'ContextCopyDD'
            /// </summary>
            internal static string ContextCopyDD
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_ContextCopyDD";
                }
            }

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_ContextCopyDDM', the id declared for Add-in Button class 'ContextCopyDDM'
            /// </summary>
            internal static string ContextCopyDDM
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_ContextCopyDDM";
                }
            }

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_ContextCopyDMS', the id declared for Add-in Button class 'ContextCopyDMS'
            /// </summary>
            internal static string ContextCopyDMS
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_ContextCopyDMS";
                }
            }

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_ContextCopyMGRS', the id declared for Add-in Button class 'ContextCopyMGRS'
            /// </summary>
            internal static string ContextCopyMGRS
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_ContextCopyMGRS";
                }
            }

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_ContextCopyUSNG', the id declared for Add-in Button class 'ContextCopyUSNG'
            /// </summary>
            internal static string ContextCopyUSNG
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_ContextCopyUSNG";
                }
            }

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_ContextCopyUTM', the id declared for Add-in Button class 'ContextCopyUTM'
            /// </summary>
            internal static string ContextCopyUTM
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_ContextCopyUTM";
                }
            }

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_CoordinateConversionButton', the id declared for Add-in Button class 'CoordinateConversionButton'
            /// </summary>
            internal static string CoordinateConversionButton
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_CoordinateConversionButton";
                }
            }

            /// <summary>
            /// Returns 'Esri_ArcMapAddinCoordinateConversion_MapPointTool', the id declared for Add-in Tool class 'MapPointTool'
            /// </summary>
            internal static string MapPointTool
            {
                get
                {
                    return "Esri_ArcMapAddinCoordinateConversion_MapPointTool";
                }
            }

            /// <summary>
            /// Returns 'ESRI_ArcMapAddinCoordinateConversion_DockableWindowCoordinateConversion', the id declared for Add-in DockableWindow class 'DockableWindowCoordinateConversion+AddinImpl'
            /// </summary>
            internal static string DockableWindowCoordinateConversion
            {
                get
                {
                    return "ESRI_ArcMapAddinCoordinateConversion_DockableWindowCoordinateConversion";
                }
            }
        }
    }

    internal static class ArcMap
    {
        private static IApplication s_app = null;
        private static IDocumentEvents_Event s_docEvent;
        private static int s_layerCount = 0;


        public static IApplication Application
        {
            get
            {
                if (s_app == null)
                    s_app = Internal.AddInStartupObject.GetHook<IMxApplication>() as IApplication;

                SyncActiveEvents();

                return s_app;
            }
            set { s_app = value; }
        }

        public static IMxDocument Document
        {
            get
            {
                if (Application != null)
                    return Application.Document as IMxDocument;

                return null;
            }
        }
        public static IMxApplication ThisApplication
        {
            get { return Application as IMxApplication; }
        }
        public static IDockableWindowManager DockableWindowManager
        {
            get { return Application as IDockableWindowManager; }
        }
        public static IDocumentEvents_Event Events
        {
            get
            {
                s_docEvent = Document as IDocumentEvents_Event;
                return s_docEvent;
            }
        }

        public static int LayerCount
        {
            get
            {
                return s_layerCount;
            }
        }

        private static void SyncActiveEvents()
        {            
            IActiveViewEvents_Event activeViewEvents = (IActiveViewEvents_Event)((IMxDocument)s_app.Document).FocusMap;
            activeViewEvents.ItemAdded += ActiveViewEvents_ItemAdded;
            activeViewEvents.ItemDeleted += ActiveViewEvents_ItemDeleted;

        }

        private static void ActiveViewEvents_ItemDeleted(object Item)
        {
            s_layerCount--;
        }

        private static void ActiveViewEvents_ItemAdded(object Item)
        {
            s_layerCount++;
        }
    }

    namespace Internal
    {
        [StartupObjectAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public sealed partial class AddInStartupObject : AddInEntryPoint
        {
            private static AddInStartupObject _sAddInHostManager;
            private List<object> m_addinHooks = null;

            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            public AddInStartupObject()
            {
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            protected override bool Initialize(object hook)
            {
                bool createSingleton = _sAddInHostManager == null;
                if (createSingleton)
                {
                    _sAddInHostManager = this;
                    m_addinHooks = new List<object>();
                    m_addinHooks.Add(hook);
                }
                else if (!_sAddInHostManager.m_addinHooks.Contains(hook))
                    _sAddInHostManager.m_addinHooks.Add(hook);

                return createSingleton;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            protected override void Shutdown()
            {
                _sAddInHostManager = null;
                m_addinHooks = null;
            }

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal static T GetHook<T>() where T : class
            {
                if (_sAddInHostManager != null)
                {
                    foreach (object o in _sAddInHostManager.m_addinHooks)
                    {
                        if (o is T)
                            return o as T;
                    }
                }

                return null;
            }

            // Expose this instance of Add-in class externally
            public static AddInStartupObject GetThis()
            {
                return _sAddInHostManager;
            }
        }
    }
}
