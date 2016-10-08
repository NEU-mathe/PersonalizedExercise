namespace ExamSysWinform.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode]
    internal class Resources
    {
        private static CultureInfo resourceCulture = CultureInfo.CurrentCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static Bitmap pictureBox1_Image
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("pictureBox1.Image", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("ExamSysWinform.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap tsbtnQuiz_Image
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("tsbtnQuiz.Image", resourceCulture);
            }
        }

        internal static Bitmap waiting
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("waiting", resourceCulture);
            }
        }
    }
}

