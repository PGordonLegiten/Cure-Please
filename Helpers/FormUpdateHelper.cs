using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurePlease.Helpers
{
    static class FormUpdateHelper
    {
        public static void UpdateLabel(Label label, string newText)
        {
            label.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                label.Text = newText;
            });
        }
    }
}
