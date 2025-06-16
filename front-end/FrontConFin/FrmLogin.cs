using FrontConFin.Models;
using FrontConFin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontConFin
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            // Verficação do  usuário
            UsuarioLogin login = new UsuarioLogin()
            {
                Login = textBoxLogin.Text,
                Password = textBoxPassword.Text
            };

            UsuarioResponse response = await UsuarioServices.Login(login);

            if (response == null)
            {
                MessageBox.Show("Usuário ou senha inválidos.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UsuarioSession.Id = response.Usuario.Id;
            UsuarioSession.Login = response.Usuario.Login;
            UsuarioSession.Nome = response.Usuario.Nome;
            UsuarioSession.Funcao = response.Usuario.Funcao;
            UsuarioSession.Token = response.Token;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                buttonLogin_Click(sender, e);
            }
        }
    }
}
