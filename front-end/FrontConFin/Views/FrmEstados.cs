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

namespace FrontConFin.Views
{
    public partial class FrmEstados : Form
    {
        private PaginacaoResponse<Estado> paginacao = null;
        private bool novo = false;
        public FrmEstados()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            atualizaDados();
        }

        private void verificaBotoes()
        {
            buttonSalvar.Enabled = false;
            buttonCancelar.Enabled = false;
            buttonNovo.Enabled = permissaoUsuario();
            buttonAlterar.Enabled = false;
            buttonExcluir.Enabled = false;
            buttonPrimeira.Enabled = false;
            buttonAnterior.Enabled = false;
            buttonProxima.Enabled = false;
            buttonUltima.Enabled = false;
            buttonPesquisar.Enabled = true;

            if(paginacao.TotalLinhas > 0)
            {
                buttonPrimeira.Enabled = true;
                buttonAnterior.Enabled = true;
                buttonProxima.Enabled = true;
                buttonUltima.Enabled = true;
                buttonAlterar.Enabled = permissaoUsuario();
                buttonExcluir.Enabled = permissaoUsuario();
            }
        }

        private bool permissaoUsuario()
        {
            if(UsuarioSession.Funcao == "Gerente" || UsuarioSession.Funcao == "Empregado")
            {
                return true;
            }
            return false;
        }

        private async void atualizaDados()
        {
            int skip = int.Parse(textBoxSkip.Text);
            int take = int.Parse(textBoxTake.Text);
            paginacao = await EstadoServices.Paginacao(textBoxPesquisa.Text, skip, take, checkBoxOrdem.Checked);
            dataGridView1.DataSource = paginacao.Dados;
            atualizaCamposDetalhes();
            verificaBotoes();
        }

        private void buttonSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonPesquisar_Click(object sender, EventArgs e)
        {
            atualizaDados();
        }

        private void FrmEstados_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonPesquisar_Click(sender, e);
            }
        }

        private void buttonPrimeira_Click(object sender, EventArgs e)
        {
            if (paginacao.Skip > 1)
            {
                textBoxSkip.Text = "1";
                atualizaDados();
            }
        }

        private void buttonAnterior_Click(object sender, EventArgs e)
        {
            if (paginacao.Skip > 1)
            {
                paginacao.Skip--;
                textBoxSkip.Text = paginacao.Skip.ToString();
                atualizaDados();
            }
        }

        private void buttonProxima_Click(object sender, EventArgs e)
        {
            decimal paginas = paginacao.TotalLinhas / paginacao.Take;
            int quantidadePaginas = (int)Math.Ceiling(paginas);
            if (paginacao.Skip < quantidadePaginas)
            {
                paginacao.Skip++;
                textBoxSkip.Text = paginacao.Skip.ToString();
                atualizaDados();
            }
        }

        private void buttonUltima_Click(object sender, EventArgs e)
        {
            decimal paginas = paginacao.TotalLinhas / paginacao.Take;
            int quantidadePaginas = (int)Math.Ceiling(paginas);
            if (paginacao.Skip < quantidadePaginas)
            {
                textBoxSkip.Text = quantidadePaginas.ToString();
                atualizaDados();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            atualizaCamposDetalhes();
        }

        private void atualizaCamposDetalhes()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Estado estado = (Estado)dataGridView1.SelectedRows[0].DataBoundItem;
                textBoxSigla.Text = estado.Sigla;
                textBoxNome.Text = estado.Nome;
            }
        }

        private void buttonNovo_Click(object sender, EventArgs e)
        {
            novo = true;
            habilitaBotoes();
            habilitaCampos();
            tabControl1.SelectedTab = tabPage2;
            textBoxSigla.Focus();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            atualizaCamposDetalhes();
            verificaBotoes();
            desabilitaCampos();
        }

        private async void buttonSalvar_ClickAsync(object sender, EventArgs e)
        {
            Estado estado = new Estado()
            {
                Sigla = textBoxSigla.Text,
                Nome = textBoxNome.Text
            };

            var resultado = novo ? await EstadoServices.PostEstado(estado) : await EstadoServices.PutEstado(estado);

            if (resultado)
            {
                tabControl1.SelectedTab = tabPage1;
                atualizaDados();
                desabilitaCampos();
            }
        }

        private void habilitaCampos() 
        {
            if (novo)
            {
                textBoxSigla.ReadOnly = false;
                textBoxNome.ReadOnly = false;
                textBoxSigla.Clear();
                textBoxNome.Clear();
            }
            else
            {
                textBoxSigla.ReadOnly = true;
                textBoxNome.ReadOnly = false;
            }
        }

        private void desabilitaCampos() 
        {
            textBoxSigla.ReadOnly = true;
            textBoxNome.ReadOnly = true;
        }

        private void habilitaBotoes() 
        {
            buttonNovo.Enabled = false;
            buttonAlterar.Enabled = false;
            buttonExcluir.Enabled = false;
            buttonSalvar.Enabled = true;
            buttonCancelar.Enabled = true;
            buttonPrimeira.Enabled = false;
            buttonAnterior.Enabled = false;
            buttonProxima.Enabled = false;
            buttonUltima.Enabled = false;
            buttonPesquisar.Enabled = false;
        }

        private void buttonAlterar_Click(object sender, EventArgs e)
        {
            novo = false;
            habilitaBotoes();
            habilitaCampos();
            tabControl1.SelectedTab = tabPage2;
            textBoxSigla.Focus();
        }

        private async void buttonExcluir_ClickAsync(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) 
            {
                Estado estado = (Estado)dataGridView1.SelectedRows[0].DataBoundItem;
                DialogResult result = MessageBox.Show(null, $"Deseja excluir o estado {estado.Sigla}?", "Controle Financeiro", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if(result == DialogResult.OK)
                {
                    var resultado = await EstadoServices.DeleteEstado(estado.Sigla);
                    if(resultado)
                    {
                        tabControl1.SelectedTab = tabPage1;
                        atualizaDados();
                    }
                }
            }
        }
    }
}
