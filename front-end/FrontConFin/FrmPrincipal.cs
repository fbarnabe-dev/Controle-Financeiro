using FrontConFin.Models;
using FrontConFin.Views;
using System;
using System.Windows.Forms;

namespace FrontConFin
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            labelUsuario.Text = UsuarioSession.Nome;
            verificaPermissoesUsuario();
        }

        private void verificaPermissoesUsuario()
        {
            usuárioToolStripMenuItem.Enabled = false;
            if (UsuarioSession.Funcao == "Gerente")
            {
                usuárioToolStripMenuItem.Enabled = true;
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void estadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEstados form = new FrmEstados();
            form.ShowDialog();
        }

        private void cidadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCidades form = new FrmCidades();
            form.ShowDialog();
        }
    }
}
