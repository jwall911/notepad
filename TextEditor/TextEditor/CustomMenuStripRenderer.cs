using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextEditor
{
    class CustomMenuStripRenderer : ToolStripProfessionalRenderer
    {
        public CustomMenuStripRenderer() : base() { }
        public CustomMenuStripRenderer(ProfessionalColorTable table) : base(table) { }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextFormat &= ~TextFormatFlags.HidePrefix;
            base.OnRenderItemText(e);
        }
    }
}