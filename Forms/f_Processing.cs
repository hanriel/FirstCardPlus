using System.Windows.Forms;

namespace FirstCardPlus.Forms
{
    public partial class f_Processing : Form
    {
        public f_Processing() => InitializeComponent();
        public void SetTitle(string s) => Invoke(new System.Action(() => { Text = s; }));
        public void SetPercent(sbyte i) => Invoke(new System.Action(() => { progressBar1.Value = i; }));
    }
}