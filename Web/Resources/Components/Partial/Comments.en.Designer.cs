﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Resources.Components.Pages {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Comments_en {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Comments_en() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Web.Resources.Components.Pages.Comments_en", typeof(Comments_en).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string Comments_Header {
            get {
                return ResourceManager.GetString("Comments_Header", resourceCulture);
            }
        }
        
        internal static string Comments_NoComments {
            get {
                return ResourceManager.GetString("Comments_NoComments", resourceCulture);
            }
        }
        
        internal static string NewComment_Header {
            get {
                return ResourceManager.GetString("NewComment_Header", resourceCulture);
            }
        }
        
        internal static string NewComment_SubmitButton {
            get {
                return ResourceManager.GetString("NewComment_SubmitButton", resourceCulture);
            }
        }
        
        internal static string Error_AuthorizationRequired {
            get {
                return ResourceManager.GetString("Error_AuthorizationRequired", resourceCulture);
            }
        }
        
        internal static string Error_ReadOnlyMode {
            get {
                return ResourceManager.GetString("Error_ReadOnlyMode", resourceCulture);
            }
        }
    }
}
